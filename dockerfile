FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.19-amd64 AS build
WORKDIR /src
COPY ./CompanyName.DDD.API ./
RUN dotnet publish ./CompanyName.DDD.API/CompanyName.DDD.API.csproj --framework net10.0 -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.19-amd64
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV TZ=America/Lima
RUN apk update && apk add libgdiplus icu-libs libc6-compat tzdata
EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
WORKDIR /app
RUN mkdir Documents
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CompanyName.DDD.API.dll"]