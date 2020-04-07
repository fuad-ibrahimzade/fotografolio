#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Fotografolio/Fotografolio.csproj", "Fotografolio/"]
RUN dotnet restore "Fotografolio/Fotografolio.csproj"
COPY . .
WORKDIR "/src/Fotografolio"
RUN apt-get install --yes curl
RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
RUN apt-get install --yes nodejs
WORKDIR "/src/Fotografolio/ClientApp"
RUN npm run build
WORKDIR "/src/Fotografolio"
RUN dotnet build "Fotografolio.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fotografolio.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Fotografolio.dll