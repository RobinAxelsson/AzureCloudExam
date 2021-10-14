#!/bin/sh

# fswatch -or ./validate.csx | xargs -n1 -I{} dotnet script validate.csx
# fswatch -or ./curltests.sh | xargs -n1 -I{} bash curltests.sh "http://localhost:7071/api/HttpTrigger"

# rebuildFunc() {
#     echo $(pidof func)
#     kill -15 $(pidof func)
#     bash start.sh
#     echo $(pidof func)
# }

fswatch -or . -e .* -i \.cs$ | xargs -n1 -I{} eval "$(
    echo $(pidof func)
    kill -15 $(pidof func)
    bash start.sh &
    pidof func
)"
# fswatch -or . -e .* -i \.cs$ | xargs -n1 -I{} dotnet test
# To filter:
# Add an exclusion filter matching any string: .*.
# To include files with a given extension ext, you add an inclusion filter matching any path ending with .ext: \.ext$. In this case you need to escape the dot . to match the literal dot, then the extension ext and then matching the end of the path with $.

# Only watch extensions, -e exclude .* all -i include \.ext
# $ fswatch [options] -e .* -i \.ext
# If you want case insensitive filters -I.
