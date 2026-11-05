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

sudo apt install mono-complete

mono --version

if [[ "${NO_MONO_COMPILATION:-}" != "true" ]]; then
  msbuild -version
fi
