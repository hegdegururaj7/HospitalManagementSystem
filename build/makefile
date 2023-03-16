Platform = Any CPU
Configuration = Release
ContainerName = psl-microservicetemplate

all: service chart

service: restore build publish create_images

restore:
	dotnet restore ../src/PSL.MicroserviceTemplate.sln --configfile ../src/nuget.config

build:
	dotnet build ../src/PSL.MicroserviceTemplate.sln -c $(Configuration)

publish:
	dotnet publish ../src/PSL.MicroserviceTemplate.Api/PSL.MicroserviceTemplate.Api.csproj -o ../src/PSL.MicroserviceTemplate.Api/bin/Publish/PSL.MicroserviceTemplate.Api -c $(Configuration)

create_images:
	docker build -t $(ContainerName) ../src/PSL.MicroserviceTemplate.Api

# Helm commands to package as a helm chart and installation helpers for Kubernetes

chart:
	helm package -u ../deploy/$(ContainerName)

install:
	helm install --set apiTag=latest $(ContainerName) ../deploy/$(ContainerName)

upgrade:
	helm upgrade --set apiTag=latest $(ContainerName) ../deploy/$(ContainerName)

uninstall:
	helm uninstall $(ContainerName)

# Local Development commands

compose:
	docker-compose -p $(ContainerName) up --no-start

compose_and_run:
	docker-compose -p $(ContainerName) up -d

run: service compose_and_run
