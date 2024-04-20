#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app/publish
RUN dotnet dev-certs https --clean
RUN dotnet dev-certs https -ep Viribus_Unitis.pfx  -p crypticpassword
WORKDIR /src
COPY ["Viribus_Unitis/Viribus_Unitis.csproj", "Viribus_Unitis/"]
RUN dotnet restore "./Viribus_Unitis/Viribus_Unitis.csproj"
COPY . .
WORKDIR "/src/Viribus_Unitis"
RUN dotnet build "./Viribus_Unitis.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Viribus_Unitis.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN chmod og+r Viribus_Unitis.pfx
WORKDIR /app/wwwroot
RUN chmod 777 files -R
RUN chown www-data files -R
RUN chgrp www-data files -R
USER app
WORKDIR /app
ENTRYPOINT ["dotnet", "Viribus_Unitis.dll"]