#!/usr/bin/env bash
pushd src/Calculator

addEndpoint=($(dotnet user-secrets list | grep addEndpoint))
addEndpoint=${addEndpoint[-1]}

subEndpoint=($(dotnet user-secrets list | grep subEndpoint))
subEndpoint=${subEndpoint[-1]}

popd

cd src/CalculatorTest

bash curltests.sh $addEndpoint add
bash curltests.sh $subEndpoint sub
