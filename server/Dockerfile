FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app


COPY src/*/*.csproj ./
RUN dotnet restore

FROM microsoft/dotnet:2.1-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TardisBank/out ./
ENTRYPOINT ["dotnet", "TardisBank.dll"]