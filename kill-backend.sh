#!/usr/bin/env bash
# kills process on error

kill -9 "$(pgrep -f "dotnet exec")"
kill -9 "$(pgrep -f "dotnet run")"
