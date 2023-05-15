FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine
COPY bin/Publish/HMS.Service.Data/ App/
COPY docker-entrypoint.sh App/
WORKDIR /App

RUN chmod +x docker-entrypoint.sh
ENTRYPOINT ["sh", "docker-entrypoint.sh"]