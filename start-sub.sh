#!/usr/bin/env bash
pushd ./src/Calculator
export Operation=SUBTRACTION
func start -p 7072 --csharp
popd
