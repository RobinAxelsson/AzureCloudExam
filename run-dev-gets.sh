#!/usr/bin/env bash

#"AdditionEndpoint": "http://localhost:7071/api/HttpTrigger"
#"SubtractionEndpoint": "http://localhost:7072/api/HttpTrigger"
pushd src/Tests
dotnet script get.csx http://localhost:7071/api/HttpTrigger 1 -2
dotnet script get.csx http://localhost:7072/api/HttpTrigger 1 -2
popd
