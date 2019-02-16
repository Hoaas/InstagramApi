FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["InstagramApi/InstagramApi.csproj", "InstagramApi/"]
RUN dotnet restore "InstagramApi/InstagramApi.csproj"
COPY . .
WORKDIR "/src/InstagramApi"
RUN dotnet build "InstagramApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "InstagramApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "InstagramApi.dll"]