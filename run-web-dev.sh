#!/usr/bin/env bash

echo "--------------------
This is a recursive dotnet run
Kill process with script kill-web-dev.sh
-------------------"
pushd src/WebCalc
ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT

#Rebuilds/reruns on ctrl+c
function cleanup() {
    popd
    bash run-web-dev.sh
}
trap cleanup SIGINT
dotnet run
