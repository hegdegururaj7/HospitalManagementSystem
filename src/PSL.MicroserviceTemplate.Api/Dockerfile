FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

COPY bin/Publish/PSL.MicroserviceTemplate.Api/ App/
COPY docker-entrypoint.sh App/

WORKDIR /App
EXPOSE 7011

RUN chmod +x docker-entrypoint.sh
ENTRYPOINT ["sh", "docker-entrypoint.sh"]