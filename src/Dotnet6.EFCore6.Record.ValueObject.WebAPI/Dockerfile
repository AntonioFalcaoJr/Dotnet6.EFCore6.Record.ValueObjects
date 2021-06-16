ARG ASPNET_VERSION="6.0.0-preview.4-alpine3.13"
ARG SDK_VERSION="6.0.100-preview.4-alpine3.13"
ARG BASE_ADRESS="mcr.microsoft.com/dotnet"

FROM $BASE_ADRESS/aspnet:$ASPNET_VERSION AS base
WORKDIR /app
EXPOSE 5000

RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

FROM $BASE_ADRESS/sdk:$SDK_VERSION AS build
WORKDIR /src

COPY ./src/Dotnet6.EFCore6.Record.ValueObject.Domain/*.csproj ./Dotnet6.EFCore6.Record.ValueObject.Domain/
COPY ./src/Dotnet6.EFCore6.Record.ValueObject.Repositories/*.csproj ./Dotnet6.EFCore6.Record.ValueObject.Repositories/
COPY ./src/Dotnet6.EFCore6.Record.ValueObject.WebAPI/*.csproj ./Dotnet6.EFCore6.Record.ValueObject.WebAPI/

COPY ./NuGet.Config ./

RUN dotnet restore ./Dotnet6.EFCore6.Record.ValueObject.WebAPI/

COPY ./src/Dotnet6.EFCore6.Record.ValueObject.Domain/. ./Dotnet6.EFCore6.Record.ValueObject.Domain/
COPY ./src/Dotnet6.EFCore6.Record.ValueObject.Repositories/. ./Dotnet6.EFCore6.Record.ValueObject.Repositories/
COPY ./src/Dotnet6.EFCore6.Record.ValueObject.WebAPI/. ./Dotnet6.EFCore6.Record.ValueObject.WebAPI/

WORKDIR /src/Dotnet6.EFCore6.Record.ValueObject.WebAPI
RUN dotnet build -c Release --no-restore -o /app/build 

FROM build AS publish
RUN dotnet publish -c Release --no-restore -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet6.EFCore6.Record.ValueObject.WebAPI.dll"]
