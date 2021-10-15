#!/usr/bin/env bash

pngpaste - >img/"$1".png
echo "![$1](./img/$1.png)" | pbcopy
