#!/usr/bin/env bash

# exit when any command fails
set -e
echo "***************************************"
echo starting HMS.Service.Data.dll
date
echo "args: " $@
echo "***************************************"

# start the app
dotnet HMS.Service.Data.dll $@