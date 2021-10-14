#!/usr/bin/env bash

kill -15 "$(pgrep -f "dotnet exec")"
kill -9 "$(pgrep -f "dotnet run")"
