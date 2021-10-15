#!/usr/bin/env bash
accountEndpoint=$(cat local-dev/secrets/accountEndpoint.secret)
accountKey=$(cat local-dev/secrets/accountKey.secret)
AdditionEndpoint=$(cat local-dev/secrets/addEndpoint.secret)
SubtractionEndpoint=$(cat local-dev/secrets/subEndpoint.secret)

dotnet src/WebCalcTest/bin/Debug/netcoreapp3.1/publish/WebCalcTest.dll accountKey=$accountKey accountEndpoint=$accountEndpoint AdditionEndpoint=$AdditionEndpoint SubtractionEndpoint=$SubtractionEndpoint
