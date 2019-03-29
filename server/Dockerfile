FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy solution and restore as distinct layers
COPY src/ ./
RUN dotnet restore

# run the unit tests
ENV TARDISBANK_DB_CON='Host=127.0.0.1;Port=5432;Database=tardisbank;User Id=some_user;Password=some_password;'
ENV TARDISBANK_KEY='somekey'
ENV TARDISBANK_SMTP_SERVER=':25:false'
ENV TARDISBANK_SMTP_CREDENTIALS='test:test'

RUN dotnet test ./TardisBank.UnitTests --filter FullyQualifiedName~TardisBank.UnitTests

# copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/TardisBank.Api/out/ ./

ENTRYPOINT ["dotnet", "TardisBank.Api.dll"]