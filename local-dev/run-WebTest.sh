#!/usr/bin/env bash
accountEndpoint=$(cat local-dev/secrets/accountEndpoint.secret)
export accountEndpoint

accountKey=$(cat local-dev/secrets/accountKey.secret)
export accountKey

cd src/WebTest
dotnet run -- 666+0=666 ADDITION
