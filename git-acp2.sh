#!/bin/sh
if [[ $# -ne 1 ]]; then
    echo adds, commits and pushes repo
    echo "one parameter needed: <commit-message>"
    exit 1
fi
git add -A
git commit -m "$1"
git push
git push tentadeploy
