#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
#WORKDIR /app
#EXPOSE 80
#
#FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
#WORKDIR /src
#COPY ["Report.csproj", ""]
#RUN dotnet restore "./Report.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "Report.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "Report.csproj" -c Release -o /app/publish
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Report.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY /app/publish .
ENTRYPOINT ["dotnet", "Report.dll"]