#!/usr/bin/env bash
if [[ "$#" == 0 ]]; then
    url=$(cat ./secrets/url.secret 2>/dev/null)
else
    url=$1
fi
echo --------Running HTTP-tests with curl-------------
out=$(bash curltests.sh "$url")
if [[ $? -ne 0 ]]; then
    echo "
    $out
    "
    echo ------------Testing failed-----------------------

    exit 1
else
    echo "
    Test Score: $out
    "
    echo --------------End of test------------------------
fi

# Since you are using a HTTP Trigger make sure your url is in the following format
# https://<APP_NAME>.azurewebsites.net/api/<FUNCTION_NAME>?code=<API_KEY>
# http://<APP_NAME>.azurewebsites.net/api/<FUNCTION_NAME>
