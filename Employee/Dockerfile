﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG type=default
ARG id=0
ENV APP_TYPE=$type
ENV APP_ID=$id

WORKDIR /src
COPY ["Employee/Employee.csproj", "Employee/"]
RUN dotnet restore "Employee/Employee.csproj"
COPY . .
WORKDIR "/src/Employee"
RUN dotnet build "Employee.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Employee.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.dll"]
