#!/usr/bin/env bash
path=temp
mkdir $path
cleanup() {
    popd
    unset Operation
    rm -rf $path
}

cp -R src/Calculator $path
pushd $path/Calculator
export Operation=SUBTRACTION

trap cleanup SIGINT
func start -p 7072 --csharp
