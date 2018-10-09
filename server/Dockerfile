FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy solution and restore as distinct layers
COPY src/ ./
RUN dotnet restore

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TardisBank.Api/out/ ./
ENTRYPOINT ["dotnet", "TardisBank.Api.dll"]