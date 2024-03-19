#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PosTech.CadPac.Api/PosTech.CadPac.Api.csproj", "PosTech.CadPac.Api/"]
COPY ["PosTech.CadPac.Services/PosTech.CadPac.Services.csproj", "PosTech.CadPac.Services/"]
COPY ["PosTech.CadPac.Domain/PosTech.CadPac.Domain.csproj", "PosTech.CadPac.Domain/"]
COPY ["PosTech.CadPac.Domain.Shared/PosTech.CadPac.Domain.Shared.csproj", "PosTech.CadPac.Domain.Shared/"]
COPY ["PosTech.CadPac.Repository/PosTech.CadPac.Repository.csproj", "PosTech.CadPac.Repository/"]
RUN dotnet restore "PosTech.CadPac.Api/PosTech.CadPac.Api.csproj"
COPY . .
WORKDIR "/src/PosTech.CadPac.Api"
RUN dotnet build "PosTech.CadPac.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PosTech.CadPac.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PosTech.CadPac.Api.dll"]