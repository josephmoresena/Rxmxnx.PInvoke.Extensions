#!/usr/bin/env bash
# Install Mono 6.12 from the Ubuntu 20.04 archive on every Ubuntu release.
# Ubuntu 26.04 ships Mono 6.14. That runtime implements .NET Standard 2.1
# incorrectly, so it is removed and is not a substitute.
#
# Focal libgdiplus still depends on libtiff5 and the pre-t64 library names.
# Those names are gone on Ubuntu 26.04. The distro libgdiplus 6.1 links
# libtiff6 and the t64 libraries, so it is installed first and pinned. Apt
# then cannot replace it with the focal build, and the 6.12 runtime does not
# need --allow-downgrades.
#
# NO_MONO_COMPILATION=true skips the msbuild check. Cross-platform CI sets it
# when only the runtime is required.
set -euo pipefail

mono_version=6.12.0.200-0xamarin2+ubuntu2004b1
mono_origin=download.mono-project.com

remove_distro_mono() {
  local package installed=()
  local packages=(
    mono-runtime
    mono-runtime-sgen
    mono-runtime-common
    mono-devel
    mono-complete
    mono-mcs
    mono-roslyn
    mono-gac
    mono-utils
    mono-libraries
    ca-certificates-mono
  )
  for package in "${packages[@]}"; do
    if dpkg-query -W -f='${Status}' "${package}" 2>/dev/null | grep -q 'install ok installed'; then
      installed+=("${package}")
    fi
  done
  if ((${#installed[@]} > 0)); then
    sudo apt-get remove -y "${installed[@]}"
  fi
}

pin_mono_612() {
  sudo tee /etc/apt/preferences.d/mono-6.12 >/dev/null <<EOF
Package: mono-complete mono-runtime mono-runtime-sgen mono-runtime-common mono-devel mono-mcs mono-roslyn mono-gac mono-utils libmono-2.0-1 libmono-2.0-dev ca-certificates-mono
Pin: version ${mono_version}
Pin-Priority: 1001

Package: libgdiplus
Pin: release o=Ubuntu
Pin-Priority: 1001
EOF
}

remove_distro_mono

# Distro libgdiplus, before the focal archive is added, so apt records 6.1
# (libtiff6 / t64) rather than focal 6.0.5 (libtiff5).
sudo apt-get update
sudo apt-get install -y ca-certificates gnupg libgdiplus

sudo rm -f /usr/share/keyrings/mono-official-archive-keyring.gpg
sudo gpg --homedir /tmp --no-default-keyring \
  --keyring gnupg-ring:/usr/share/keyrings/mono-official-archive-keyring.gpg \
  --keyserver hkp://keyserver.ubuntu.com:80 \
  --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
sudo chmod 644 /usr/share/keyrings/mono-official-archive-keyring.gpg
echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] https://${mono_origin}/repo/ubuntu stable-focal main" \
  | sudo tee /etc/apt/sources.list.d/mono-official-stable.list >/dev/null
pin_mono_612
sudo apt-get update

sudo apt-get install -y \
  "mono-complete=${mono_version}" \
  "mono-runtime=${mono_version}" \
  "mono-devel=${mono_version}" \
  mono-vbnc \
  mono-dbg

mono --version
if ! mono --version | grep -q 'version 6\.12\.'; then
  echo "Mono 6.12 is required. Ubuntu's Mono 6.14 is not a substitute." >&2
  exit 1
fi

if [[ "${NO_MONO_COMPILATION:-}" != "true" ]]; then
  msbuild -version
fi
