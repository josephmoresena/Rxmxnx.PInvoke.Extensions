#!/usr/bin/env bash
# Ubuntu 24.04 and older can install Mono 6.12 (mono-complete) from the
# Ubuntu 20.04 archive. Ubuntu 26.04 cannot: those packages pin libraries
# that were renamed. There, use the distro Mono 6.14 and add msbuild 16.10,
# which is a managed application and does not replace the runtime.
set -euo pipefail

sudo apt install ca-certificates gnupg
sudo gpg --homedir /tmp --no-default-keyring --keyring gnupg-ring:/usr/share/keyrings/mono-official-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
sudo chmod +r /usr/share/keyrings/mono-official-archive-keyring.gpg
echo "deb [signed-by=/usr/share/keyrings/mono-official-archive-keyring.gpg] https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
sudo apt update

mkdir -p /tmp/mono612-debs
cd /tmp/mono612-debs

apt-get download \
  mono-complete \
  mono-vbnc \
  mono-dbg \
  mono-runtime \
  mono-runtime-sgen \
  mono-devel \
  mono-llvm-support \
  mono-utils \
  mono-roslyn

for package in ./*.deb; do
    echo "===== $package ====="
    dpkg-deb -f "$package" Package Version Depends
done

sudo dpkg --force-depends -i ./*.deb

mono --version

if [[ "${NO_MONO_COMPILATION:-}" != "true" ]]; then
  msbuild -version
fi
