#!/usr/bin/env bash
accountEndpoint=$(cat local-dev/secrets/accountEndpoint.secret)
export accountEndpoint

accountKey=$(cat local-dev/secrets/accountKey.secret)
export accountKey

dotnet src/DbClientTest/bin/Debug/netcoreapp3.1/publish/DbClientTest.dll accountKey=$accountKey accountEndpoint=$accountEndpoint
