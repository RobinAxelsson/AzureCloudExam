#!/usr/bin/env bash
accountEndpoint=$(cat local-dev/secrets/accountEndpoint.secret)
accountKey=$(cat local-dev/secrets/accountKey.secret)
AdditionEndpoint=$(cat local-dev/secrets/addEndpoint.secret)
SubtractionEndpoint=$(cat local-dev/secrets/subEndpoint.secret)

dotnet publish src/WebCalcTest/WebCalcTest.csproj -c Release -o tempapp
dotnet tempapp/WebCalcTest.dll accountKey=$accountKey accountEndpoint=$accountEndpoint AdditionEndpoint=$AdditionEndpoint SubtractionEndpoint=$SubtractionEndpoint
rm -rf tempapp
