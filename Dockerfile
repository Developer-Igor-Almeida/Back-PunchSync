FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["PunchSync.Api/PunchSync.Api.csproj", "PunchSync.Api/"]
COPY ["PunchSync.Application/PunchSync.Application.csproj", "PunchSync.Application/"]
COPY ["PunchSync.Domain/PunchSync.Domain.csproj", "PunchSync.Domain/"]
COPY ["PunchSync.Infra/PunchSync.Infra.csproj", "PunchSync.Infra/"]
COPY ["PunchSync.Ioc/PunchSync.Ioc.csproj", "PunchSync.Ioc/"]
RUN dotnet restore "PunchSync.Api/PunchSync.Api.csproj"
COPY . .
RUN dotnet build "PunchSync.Api/PunchSync.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PunchSync.Api/PunchSync.Api.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PunchSync.Api.dll"]
