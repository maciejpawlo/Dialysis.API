#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dialysis.API/Dialysis.API.csproj", "Dialysis.API/"]
COPY ["Dialysis.BE/Dialysis.BE.csproj", "Dialysis.BE/"]
COPY ["Dialysis.DAL/Dialysis.DAL.csproj", "Dialysis.DAL/"]
COPY ["Dialysis.BLL/Dialysis.BLL.csproj", "Dialysis.BLL/"]
RUN dotnet restore "Dialysis.API/Dialysis.API.csproj"
COPY . .
WORKDIR "/src/Dialysis.API"
RUN dotnet build "Dialysis.API.csproj" -c Release -o /app/build

#migrations utility
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS migration
WORKDIR /src
COPY . .
RUN dotnet restore "Dialysis.Migration/Dialysis.Migration.csproj"
COPY . .
WORKDIR "/src/Dialysis.Migration"
RUN dotnet build "Dialysis.Migration.csproj" -c Release -o /app/migration

FROM build AS publish
RUN dotnet publish "Dialysis.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /migration
COPY --from=migration /app/migration .

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dialysis.API.dll"]
