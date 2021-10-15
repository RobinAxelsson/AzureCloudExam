#!/usr/bin/env bash

# Runs the app locally but overrides the e
ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_ENVIRONMENT

accountEndpoint=$(cat local-dev/secrets/accountEndpoint.secret)
export accountEndpoint

accountKey=$(cat local-dev/secrets/accountKey.secret)
export accountKey

AdditionEndpoint=$(cat local-dev/secrets/addEndpoint.secret)
export AdditionEndpoint

SubtractionEndpoint=$(cat local-dev/secrets/subEndpoint.secret)
export SubtractionEndpoint

cd src/WebCalc
dotnet run
