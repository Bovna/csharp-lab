# Razvoj i održavanje

Ovaj dokument sadrži tehničke upute za lokalni razvoj, testiranje i održavanje aplikacije KinoKlik. Kratak pregled projekta i live demo nalaze se u glavnom [README-u](../README.md).

## Preduvjeti

- .NET 8 SDK
- SQL Server ili SQL Server LocalDB
- EF Core CLI alat iz repozitorija

## Lokalno pokretanje

Iz korijena repozitorija pokreni:

```powershell
dotnet tool restore --tool-manifest KinoKlik\Web\dotnet-tools.json
dotnet restore KinoKlik\KinoKlik.sln
dotnet ef database update --project KinoKlik\DAL\KinoKlik.DAL.csproj --startup-project KinoKlik\Web\KinoKlik.Web.csproj
dotnet run --project KinoKlik\Web\KinoKlik.Web.csproj
```

Zadani razvojni connection string koristi LocalDB. Za drugi SQL Server postavi `ConnectionStrings__CinemaDbContext` kroz environment varijablu ili .NET user secrets.

## Razvojni korisnici

Razvojne administratorske i managerske račune konfiguriraj izvan repozitorija:

```powershell
dotnet user-secrets set "SeedUsers:Admin:Email" "admin@example.test" --project KinoKlik\Web\KinoKlik.Web.csproj
dotnet user-secrets set "SeedUsers:Admin:Password" "<strong-local-password>" --project KinoKlik\Web\KinoKlik.Web.csproj
```

Isti obrazac vrijedi za `SeedUsers:Manager:*`. OIB i JMBAG nisu obavezni; za lokalni demo račun mogu se koristiti opcionalni ključevi `SeedUsers:{Role}:OIB` i `SeedUsers:{Role}:JMBAG`. Razvojni seed korisnici kreiraju se samo u `Development` okruženju.

## Konfiguracija

Produkcijske vrijednosti postavljaju se kroz Azure App Service Configuration i GitHub Secrets. Stvarne vrijednosti i tajne ne pripadaju repozitoriju.

| Ključ | Obavezno | Namjena |
| --- | --- | --- |
| `ConnectionStrings__CinemaDbContext` | da | SQL Server veza |
| `ASPNETCORE_ENVIRONMENT` | na hostu | naziv okruženja |
| `UploadStorage__RootPath` | ne | trajna lokacija za uploadane vizuale |
| `UploadStorage__RequestPath` | ne | javna URL putanja, zadano `/uploads` |
| `Authentication__Google__ClientId` | ne | Google prijava |
| `Authentication__Google__ClientSecret` | ne | Google prijava |
| `APPLICATIONINSIGHTS_CONNECTION_STRING` | ne | Azure telemetrija |

## Testovi

Pokreni cijeli testni projekt u Release konfiguraciji:

```powershell
dotnet test KinoKlik\KinoKlik.sln --configuration Release
```

Testovi provjeravaju javne i zaštićene API rute, autorizaciju po rolama, validacijske pogreške, pretragu, upload ograničenja, health endpoint, Swagger dokument, cijeli booking tok te pravila potvrde i rezervacije sjedala.

Pravi SQL Server concurrency test filtriranog jedinstvenog indeksa automatski se izvršava u pull request CI-ju. Za lokalno izvršavanje postavi connection string prema zasebnoj testnoj bazi:

```powershell
$env:TEST_SQL_CONNECTION_STRING = "<test-sql-connection-string>"
dotnet test KinoKlik\KinoKlik.sln --configuration Release
```

Test ne treba usmjeravati prema razvojnoj ili produkcijskoj bazi.

## Deploy, migracije i nadzor

GitHub Actions pri promjeni aplikacije izvršava restore, Release build, testove i publish. Nakon uspješnog builda artefakt se deploya na Azure App Service, a workflow provjerava readiness endpoint.

- `/health` — provjerava dostupnost baze i postoje li neprimijenjene migracije
- `/health/live` — potvrđuje da ASP.NET proces radi bez provjere baze

Migracije se namjerno ne izvršavaju automatski pri produkcijskom startupu. Prije deploya verzije s novom migracijom treba je zasebno primijeniti na ciljnu bazu.

Azure SQL serverless baza može se nakon dulje neaktivnosti pokretati do približno jedne minute. Aplikacija zato dopušta do 90 sekundi za početno povezivanje i koristi EF Core transient retry, bez periodičnog keep-warm prometa.

Uploadani vizuali zahtijevaju trajnu Azure pohranu kako bi preživjeli svaki deploy.

## Sigurnost i demo podaci

- Produkcijski korisnici i lozinke ne seedaju se pri startupu.
- Azure publish profil, connection stringovi i OAuth tajne pohranjuju se izvan Gita.
- Gitleaks workflow skenira Git povijest pri svakom pushu na `main` i u pull requestovima.
- Početni filmovi, kina, osobe i kontaktni podaci su izmišljeni.
- Privilegirane role nisu dio javnog demo računa.

Među važnijim implementacijskim odlukama su filtrirani jedinstveni SQL indeks za aktivnu rezervaciju sjedala, GUID kod potvrde, DTO modeli koji ne izlažu interne entitete te odvojeni readiness i liveness endpointi.
