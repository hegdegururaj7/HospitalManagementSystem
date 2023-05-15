Platform = Any CPU
Configuration = Release
ContainerName = hms-service

all: service chart

service: restore build publish create_images

restore:
	dotnet restore ../src/HMS.Service.sln --configfile ../src/nuget.config

build:
	dotnet build ../src/HMS.Service.sln -c $(Configuration)

publish:
	dotnet publish ../src/HMS.Api/HMS.Service.Api.csproj -o ../src/HMS.Api/bin/Publish/HMS.Api -c $(Configuration)

create_images:
	docker build -t $(ContainerName) ../src/HMS.Api

# Local Development commands

compose:
	docker-compose -p $(ContainerName) up --no-start

compose_and_run:
	docker-compose -p $(ContainerName) up -d

run: service compose_and_run
