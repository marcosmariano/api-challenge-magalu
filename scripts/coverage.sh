#!/bin/sh
set -x
set -e

cd "$(dirname "$0")/.."

if ! [ -x "$(command -v coverlet)" ]; then
  echo 'Installing Coverlet.' >&2
  dotnet tool install --global coverlet.console --version 1.7.2
  
  export PATH="$PATH:/root/.dotnet/tools"
fi

dotnet build

echo "--------------- COVERLET ----------------"

coverlet ./test/LuizaLabs.Challenge.Test/bin/Debug/net5.0/LuizaLabs.Challenge.Test.dll \
    --target "dotnet" \
    --targetargs "test ./test/LuizaLabs.Challenge.Test/LuizaLabs.Challenge.Test.csproj --no-build" \
    --output "./coverage" \
    --exclude-by-file "**/Infra/**" \
    --exclude-by-file "**/Migrations/*" \
    --exclude-by-file "**/Models/**" \
    --exclude-by-file "**/ViewModels/**" \
    --exclude-by-file "**/Dto/**" \
    --exclude-by-file "**/Repositories/**" \
    --exclude-by-file "**/*Extensions*" \
    --exclude-by-file "**/*Startup*" \
    --exclude-by-file "**/Program.cs" \
    --exclude-by-file "**/BaseBusiness.cs" \
    --exclude-by-file "**/Services/**" \
    --format "opencover"