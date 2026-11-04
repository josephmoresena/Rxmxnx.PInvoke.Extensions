#!/usr/bin/env bash
# Ubuntu 24.04 and older can install Mono 6.12 (mono-complete) from the
# Ubuntu 20.04 archive. Ubuntu 26.04 cannot: those packages pin libraries
# that were renamed. There, use the distro Mono 6.14 and add msbuild 16.10,
# which is a managed application and does not replace the runtime.
set -euo pipefail

mono_repo=https://download.mono-project.com/repo/ubuntu

ubuntu_major() {
  if [ ! -r /etc/os-release ]; then
    echo 0
    return
  fi
  # shellcheck disable=SC1091
  . /etc/os-release
  echo "${VERSION_ID%%.*}"
}

install_openssl_compat() {
  local workdir="$1"
  curl -fsSL -o "${workdir}/libssl1.1.deb" \
    https://archive.ubuntu.com/ubuntu/pool/main/o/openssl/libssl1.1_1.1.0g-2ubuntu4_amd64.deb
  curl -fsSL -o "${workdir}/libssl1.0.0.deb" \
    https://archive.ubuntu.com/ubuntu/pool/main/o/openssl1.0/libssl1.0.0_1.0.2n-1ubuntu5.13_amd64.deb
  sudo dpkg -i "${workdir}/libssl1.1.deb" "${workdir}/libssl1.0.0.deb"
}

install_mono_612() {
  local workdir="$1"
  sudo apt-get install -y ca-certificates gnupg
  sudo rm -f /usr/share/keyrings/mono-official-archive-keyring.gpg
  # gnupg-ring: keeps the v4 keyring apt accepts. GnuPG 2.4 otherwise writes a keybox apt ignores.
  sudo gpg --homedir /tmp --no-default-keyring \
    --keyring gnupg-ring:/usr/share/keyrings/mono-official-archive-keyring.gpg \
    --keyserver hkp://keyserver.ubuntu.com:80 \
    --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
  sudo chmod 644 /usr/share/keyrings/mono-official-archive-keyring.gpg
  echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] ${mono_repo} stable-focal main" \
    | sudo tee /etc/apt/sources.list.d/mono-official-stable.list >/dev/null
  sudo apt-get update
  install_openssl_compat "${workdir}"
  sudo apt-get install -y mono-complete mono-vbnc mono-dbg gdb libssl3
}

install_mono_614() {
  local workdir="$1"
  sudo apt-get install -y mono-devel mono-libraries-debug gdb
  cd "${workdir}"
  curl -fsSL -O "${mono_repo}/pool/main/c/core-setup/msbuild-libhostfxr_3.0.0.2019.04.16.02.13-0xamarin4+ubuntu2004b1_amd64.deb"
  curl -fsSL -O "${mono_repo}/pool/main/m/msbuild/msbuild-sdkresolver_16.10.1+xamarinxplat.2021.05.26.14.00-0xamarin2+ubuntu2004b1_all.deb"
  curl -fsSL -O "${mono_repo}/pool/main/m/msbuild/msbuild_16.10.1+xamarinxplat.2021.05.26.14.00-0xamarin2+ubuntu2004b1_all.deb"
  curl -fsSL -O "${mono_repo}/pool/main/m/mono-basic/libmono-cecil-vb0.9-cil_4.7-0xamarin3+ubuntu2004b1_all.deb"
  curl -fsSL -O "${mono_repo}/pool/main/m/mono-basic/libmono-microsoft-visualbasic10.0-cil_4.7-0xamarin3+ubuntu2004b1_all.deb"
  curl -fsSL -O "${mono_repo}/pool/main/m/mono-basic/mono-vbnc_4.7-0xamarin3+ubuntu2004b1_all.deb"
  # Local debs only. apt satisfies libmono-* from mono-libraries Provides
  # instead of downloading the 6.12 packages that require mono-runtime 6.12.
  sudo apt-get install -y ./*.deb
}

workdir="$(mktemp -d)"
trap 'rm -rf "${workdir}"' EXIT
sudo apt-get update

if [ "$(ubuntu_major)" -lt 26 ]; then
  install_mono_612 "${workdir}"
else
  install_mono_614 "${workdir}"
fi

msbuild -version
mono --version
