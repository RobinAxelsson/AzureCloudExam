#!/usr/bin/env bash
path="./src/Calculator2"
cleanup() {
    popd
    unset Operation
    rm -rf $path
}

cp -R src/Calculator $path
pushd $path
export Operation=SUBTRACTION

trap cleanup SIGINT
func start -p 7072 --csharp
