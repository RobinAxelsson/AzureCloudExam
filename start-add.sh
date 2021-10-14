#!/usr/bin/env bash
pushd ./src/Calculator
export Operation=ADDITION
func start -p 7071 --csharp
popd
