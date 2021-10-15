#!/usr/bin/env bash

ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT

accountEndpoint=$(cat secrets/accountEndpoint.secret)
export accountEndpoint

accountKey=$(cat secrets/accountKey.secret)
export accountKey

AdditionEndpoint=$(cat ./secrets/addEndpoint.secret)
export AdditionEndpoint

SubtractionEndpoint=$(cat ./secrets/addEndpoint.secret)
export SubtractionEndpoint

cd src/WebCalc
dotnet run
