#!/usr/bin/env bash

pushd src/WebCalc
ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT

dotnet run
