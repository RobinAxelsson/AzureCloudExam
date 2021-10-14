#!/usr/bin/env bash

addEndpoint=($(dotnet user-secrets list | grep addEndpoint))
addEndpoint=${addEndpoint[-1]}

subEndpoint=($(dotnet user-secrets list | grep subEndpoint))
subEndpoint=${subEndpoint[-1]}

cd ../..
bash curltests.sh $addEndpoint add
bash curltests.sh $subEndpoint sub
