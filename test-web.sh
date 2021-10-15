#!/usr/bin/env bash
accountEndpoint=$(cat secrets/accountEndpoint.secret)
export accountEndpoint

accountKey=$(cat secrets/accountKey.secret)
export accountKey

cd src/WebTest
dotnet run -- 666+0=666 ADDITION
