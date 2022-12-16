#!/usr/bin/env bash

# exit when any command fails
set -e
cp /mnt/secrets-store/pslhx-backend-ca.crt  /usr/local/share/ca-certificates/pslhx-backend-ca.crt
update-ca-certificates
echo "*******************************"
echo starting PSL.MicroserviceTemplate.Api.dll
date
echo "*******************************"

# start the app
dotnet PSL.MicroserviceTemplate.Api.dll