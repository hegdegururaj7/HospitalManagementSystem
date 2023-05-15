FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine

COPY bin/Publish/HMS.Api/ App/
COPY docker-entrypoint.sh App/

WORKDIR /App
EXPOSE 3000

RUN chmod +x docker-entrypoint.sh
ENTRYPOINT ["sh", "docker-entrypoint.sh"]