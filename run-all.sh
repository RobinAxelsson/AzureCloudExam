#!/usr/bin/env bash
ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT

cleanup() {
    kill $(jobs -p)
    unset ASPNETCORE_ENVIRONMENT
}
trap cleanup SIGINT
bash run-add-func.sh &
bash run-sub-func.sh &
cd src/WebCalc
dotnet run
