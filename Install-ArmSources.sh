#!/usr/bin/env bash
# Register arm64 and armhf only when the image sources do not already name them.
# Host amd64 sources are left in place. Rewriting them makes apt downgrade the
# packages the runner image already installed.
set -euo pipefail

if [ ! -r /etc/os-release ]; then
  echo "Missing /etc/os-release." >&2
  exit 1
fi
# shellcheck disable=SC1091
. /etc/os-release
suite="${UBUNTU_CODENAME:-$VERSION_CODENAME}"
ubuntu_major="${VERSION_ID%%.*}"

source_has_arch() {
  grep -R -E -q \
    -e "Architectures:.*[[:space:]]${1}([[:space:]]|\$)" \
    -e "arch=[^]]*${1}" \
    /etc/apt/sources.list /etc/apt/sources.list.d 2>/dev/null
}

has_arm64=false
has_armhf=false
source_has_arch arm64 && has_arm64=true
source_has_arch armhf && has_armhf=true

sudo dpkg --add-architecture arm64
sudo dpkg --add-architecture armhf
sudo rm -f /etc/apt/sources.list.d/arm64.list /etc/apt/sources.list.d/armhf.list \
  /etc/apt/sources.list.d/arm-ports.list

# Resolute's mirror already publishes arm64 and ignores armhf.
# Noble's mirror publishes neither, so both come from ports.
port_archs=()
if [[ "$has_arm64" != true && "$ubuntu_major" -lt 26 ]]; then
  port_archs+=(arm64)
fi
if [[ "$has_armhf" != true ]]; then
  port_archs+=(armhf)
fi
if [[ ${#port_archs[@]} -gt 0 ]]; then
  joined=$(IFS=,; echo "${port_archs[*]}")
  printf '%s\n' \
    "deb [arch=${joined}] https://ports.ubuntu.com/ubuntu-ports/ ${suite} main restricted universe multiverse" \
    "deb [arch=${joined}] https://ports.ubuntu.com/ubuntu-ports/ ${suite}-updates main restricted universe multiverse" \
    "deb [arch=${joined}] https://ports.ubuntu.com/ubuntu-ports/ ${suite}-backports main restricted universe multiverse" \
    | sudo tee /etc/apt/sources.list.d/arm-ports.list >/dev/null
fi

# Allow the host mirror to serve arm64 only when nothing already lists it,
# and never open those stanzas to armhf.
host_archs=(amd64)
if [[ "$has_arm64" != true && "$ubuntu_major" -ge 26 ]]; then
  host_archs+=(arm64)
fi
host_archs_csv=$(IFS=,; echo "${host_archs[*]}")
host_archs_words="${host_archs[*]}"
sudo sed -i -e "s/deb http/deb [arch=${host_archs_csv}] http/g" /etc/apt/sources.list
sudo sed -i -e "s/deb mirror/deb [arch=${host_archs_csv}] mirror/g" /etc/apt/sources.list
sudo HOST_ARCHS="$host_archs_words" HAS_ARM64="$has_arm64" HAS_ARMHF="$has_armhf" python3 - << 'PY'
import os
from pathlib import Path
path = Path("/etc/apt/sources.list.d/ubuntu.sources")
if not path.exists():
    raise SystemExit(0)
has_arm64 = os.environ["HAS_ARM64"] == "true"
has_armhf = os.environ["HAS_ARMHF"] == "true"
blocks = [b for b in path.read_text().split("\n\n") if b.strip()]
kept = []
for block in blocks:
    lines = [ln for ln in block.splitlines() if ln.strip()]
    if not lines:
        continue
    if any(ln.startswith("Architectures:") for ln in lines):
        kept.append("\n".join(lines))
        continue
    if has_arm64 and has_armhf:
        kept.append("\n".join(lines))
        continue
    archs = os.environ["HOST_ARCHS"]
    inserted = False
    rewritten = []
    for ln in lines:
        rewritten.append(ln)
        if ln.startswith("Components:"):
            rewritten.append("Architectures: " + archs)
            inserted = True
    kept.append("\n".join(rewritten if inserted else lines + ["Architectures: " + archs]))
path.write_text("\n\n".join(kept) + "\n")
PY
sudo apt-get update
