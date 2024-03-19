#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PosTech.CadPac.MicroService.Consumer/PosTech.CadPac.MicroService.Consumer.csproj", "PosTech.CadPac.MicroService.Consumer/"]
COPY ["PosTech.CadPac.Domain/PosTech.CadPac.Domain.csproj", "PosTech.CadPac.Domain/"]
COPY ["PosTech.CadPac.Domain.Shared/PosTech.CadPac.Domain.Shared.csproj", "PosTech.CadPac.Domain.Shared/"]
COPY ["PosTech.CadPac.Repository/PosTech.CadPac.Repository.csproj", "PosTech.CadPac.Repository/"]
RUN dotnet restore "PosTech.CadPac.MicroService.Consumer/PosTech.CadPac.MicroService.Consumer.csproj"
COPY . .
WORKDIR "/src/PosTech.CadPac.MicroService.Consumer"
RUN dotnet build "PosTech.CadPac.MicroService.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PosTech.CadPac.MicroService.Consumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .cd
ENTRYPOINT ["dotnet", "PosTech.CadPac.MicroService.Consumer.dll"]