#!/usr/bin/env bash

kill -9 "$(pgrep -f "dotnet exec")"
kill -9 "$(pgrep -f "dotnet run")"
