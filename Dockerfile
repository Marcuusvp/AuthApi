FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7028

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AuthApi.Api/AuthApi.Api.csproj", "AuthApi.Api/"]
COPY ["AuthApi.Aplicacao/AuthApi.Aplicacao.csproj", "AuthApi.Aplicacao/"]
COPY ["AuthApi.Repositorio/AuthApi.Repositorio.csproj", "AuthApi.Repositorio/"]
COPY ["AuthApi.Dominio/AuthApi.Dominio.csproj", "AuthApi.Dominio/"]
RUN dotnet restore "AuthApi.Api/AuthApi.Api.csproj"
COPY . .
WORKDIR "/src/AuthApi.Api"
RUN dotnet build "AuthApi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AuthApi.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN rm -rf /app/build
ENTRYPOINT ["dotnet", "AuthApi.Api.dll"]