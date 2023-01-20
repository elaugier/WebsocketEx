#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8603
EXPOSE 8604

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WSServer/WSServer.csproj", "WSServer/"]
RUN dotnet restore "WSServer/WSServer.csproj"
COPY . .
WORKDIR "/src/WSServer"
RUN dotnet build "WSServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WSServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WSServer.dll"]