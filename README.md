<p align="center">
  <img src="KinoKlik/Web/wwwroot/images/brand/logo.svg" alt="KinoKlik" width="220">
</p>

<h1 align="center">KinoKlik</h1>

<p align="center">
  ASP.NET Core aplikacija za pregled kino programa i vođenu rezervaciju sjedala.
</p>

<p align="center">
  <a href="https://cinema-bv-fuheftdfbyazaqea.italynorth-01.azurewebsites.net/"><strong>Otvori live demo</strong></a>
  ·
  <a href="https://github.com/Bovna/kinoklik/actions/workflows/main_cinema-bv.yml">Build i deploy</a>
  ·
  <a href="https://github.com/Bovna/kinoklik/actions/workflows/secret-scan.yml">Secret scan</a>
</p>

[![Build and deploy](https://github.com/Bovna/kinoklik/actions/workflows/main_cinema-bv.yml/badge.svg)](https://github.com/Bovna/kinoklik/actions/workflows/main_cinema-bv.yml)
[![Pull request CI](https://github.com/Bovna/kinoklik/actions/workflows/ci.yml/badge.svg)](https://github.com/Bovna/kinoklik/actions/workflows/ci.yml)
[![Secret scan](https://github.com/Bovna/kinoklik/actions/workflows/secret-scan.yml/badge.svg)](https://github.com/Bovna/kinoklik/actions/workflows/secret-scan.yml)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)

KinoKlik je portfolio projekt koji demonstrira razvoj i produkcijsko održavanje cjelovite ASP.NET Core MVC aplikacije. Anonimni korisnik može pregledavati filmove, kina i projekcije te proći tok rezervacije, dok su upravljanje podacima i promjene kroz API zaštićeni rolama.

> [!NOTE]
> Aplikacija je demonstracijska: nema stvarne naplate, svi početni podaci su izmišljeni i u obrasce ne treba unositi stvarne osobne podatke.

## Isprobaj aplikaciju

1. Otvori [live demo](https://cinema-bv-fuheftdfbyazaqea.italynorth-01.azurewebsites.net/).
2. Odaberi **Kupi ulaznicu**.
3. Prođi tok kino → film → projekcija → sjedalo → potvrda.
4. Za checkout koristi isključivo izmišljene podatke.
5. Pregledaj javnu [OpenAPI dokumentaciju](https://cinema-bv-fuheftdfbyazaqea.italynorth-01.azurewebsites.net/swagger).

Administratorski i managerski pristup nisu javno objavljeni. Javni demo namjerno prikazuje korisnički tok bez dijeljenja privilegiranih produkcijskih računa.

## Izdvojene mogućnosti

- javni katalog filmova, kina i projekcija
- globalna i AJAX pretraga te autocomplete kontrole
- vođeni booking u pet koraka sa sjedalom, cijenom i potvrdom
- zaštita od dvostruke rezervacije i na aplikacijskoj i na SQL razini
- ASP.NET Core Identity, opcionalna Google prijava i <code>Admin</code>/<code>Manager</code> autorizacija
- MVC sučelje i REST API s odvojenim DTO modelima
- javni Swagger/OpenAPI pregled API ruta
- soft delete za domenske podatke i upravljanje filmskim vizualima
- integracijski testovi API-ja, autorizacije, pretrage, uploada, health checkova i cijelog booking toka
- pravi SQL Server concurrency test filtriranog indeksa protiv dvostruke rezervacije
- automatizirani build, test i Azure deploy uz provjeru zdravlja aplikacije
- Application Insights telemetrija i automatsko skeniranje repozitorija za tajne

## Tehnologije

| Područje | Tehnologije |
| --- | --- |
| Backend | .NET 8, ASP.NET Core MVC, Razor Pages, Web API |
| Podaci | Entity Framework Core 8, SQL Server, EF migracije |
| Sigurnost | ASP.NET Core Identity, role, antiforgery zaštita, Google OAuth |
| Frontend | Razor, Bootstrap, JavaScript, AJAX |
| Testovi | xUnit, FluentAssertions, WebApplicationFactory, SQL Server integration |
| Produkcija | Azure App Service, GitHub Actions, Application Insights |

## Arhitektura

~~~mermaid
flowchart LR
    browser[Web preglednik] --> web[Web<br/>MVC, Razor, API, Identity]
    web --> dal[DAL<br/>EF Core i migracije]
    dal --> model[Model<br/>domenski entiteti]
    dal --> db[(SQL Server)]
    tests[Tests<br/>xUnit integracijski testovi] --> web
    actions[GitHub Actions] --> azure[Azure App Service]
    azure --> insights[Application Insights]
~~~

Repozitorij je podijeljen na četiri projekta:

- <code>KinoKlik/Model</code> — domenski entiteti i enumeracije
- <code>KinoKlik/DAL</code> — <code>CinemaDbContext</code>, konfiguracija baze, seed podaci i migracije
- <code>KinoKlik/Web</code> — MVC sučelje, API kontroleri, Identity, booking i upload
- <code>KinoKlik/Tests</code> — integracijski i sigurnosni testovi

Među važnijim tehničkim odlukama su filtrirani jedinstveni SQL indeks za aktivnu rezervaciju sjedala, GUID kod potvrde, DTO modeli koji ne izlažu interne entitete te odvojeni readiness i liveness endpointi.

## Lokalno pokretanje

### Preduvjeti

- .NET 8 SDK
- SQL Server ili SQL Server LocalDB

### Postavljanje

~~~powershell
dotnet tool restore --tool-manifest KinoKlik\Web\dotnet-tools.json
dotnet restore KinoKlik\KinoKlik.sln
dotnet ef database update --project KinoKlik\DAL\KinoKlik.DAL.csproj --startup-project KinoKlik\Web\KinoKlik.Web.csproj
dotnet run --project KinoKlik\Web\KinoKlik.Web.csproj
~~~

Zadani razvojni connection string koristi LocalDB. Za drugi SQL Server postavi <code>ConnectionStrings__CinemaDbContext</code> kroz environment varijablu ili .NET user secrets.

### Razvojni administratorski račun

Razvojne korisnike konfiguriraj izvan repozitorija:

~~~powershell
dotnet user-secrets set "SeedUsers:Admin:Email" "admin@example.test" --project KinoKlik\Web\KinoKlik.Web.csproj
dotnet user-secrets set "SeedUsers:Admin:Password" "<strong-local-password>" --project KinoKlik\Web\KinoKlik.Web.csproj
~~~

Isti obrazac vrijedi za <code>SeedUsers:Manager:*</code>. OIB i JMBAG nisu obavezni; ako ih želiš koristiti u lokalnom demo računu, dostupni su opcionalni ključevi <code>SeedUsers:{Role}:OIB</code> i <code>SeedUsers:{Role}:JMBAG</code>. Razvojni seed korisnici kreiraju se samo u <code>Development</code> okruženju.

## Testovi

~~~powershell
dotnet test KinoKlik\KinoKlik.sln --configuration Release
~~~

Testni projekt provjerava javne i zaštićene API rute, autorizaciju po rolama, validacijske pogreške, pretragu, upload ograničenja, health endpoint, Swagger dokument, puni booking tok te sigurnosna pravila potvrde i rezervacije sjedala. Pravi SQL Server concurrency test automatski se izvršava u pull request CI-ju, a lokalno samo kada je postavljen <code>TEST_SQL_CONNECTION_STRING</code>.

## Konfiguracija

Produkcijske vrijednosti postavljaju se kroz Azure App Service Configuration i GitHub Secrets; stvarne vrijednosti ne pripadaju repozitoriju.

| Ključ | Obavezno | Namjena |
| --- | --- | --- |
| <code>ConnectionStrings__CinemaDbContext</code> | da | SQL Server veza |
| <code>ASPNETCORE_ENVIRONMENT</code> | na hostu | naziv okruženja |
| <code>UploadStorage__RootPath</code> | ne | trajna lokacija za uploadane vizuale |
| <code>UploadStorage__RequestPath</code> | ne | javna URL putanja, zadano <code>/uploads</code> |
| <code>Authentication__Google__ClientId</code> | ne | Google prijava |
| <code>Authentication__Google__ClientSecret</code> | ne | Google prijava |
| <code>APPLICATIONINSIGHTS_CONNECTION_STRING</code> | ne | Azure telemetrija |

## Deploy i nadzor

GitHub Actions na promjenu aplikacije radi restore, Release build, testove i publish. Nakon uspješnog builda artefakt se deploya na Azure App Service, a workflow potvrđuje readiness preko <code>/health</code>.

- <code>/health</code> — provjerava dostupnost baze i postoje li neprihvaćene migracije
- <code>/health/live</code> — potvrđuje da ASP.NET proces radi bez provjere baze

Migracije se namjerno ne izvršavaju automatski pri produkcijskom startupu. Prije deploya verzije s novom migracijom treba je zasebno primijeniti na ciljnu bazu.

Azure SQL serverless baza može se nakon dulje neaktivnosti pokretati do približno jedne minute. Aplikacija zato dopušta do 90 sekundi za početno povezivanje i koristi EF Core transient retry, bez periodičnog keep-warm prometa.

## Sigurnost i demo podaci

- Produkcijski korisnici i lozinke ne seedaju se pri startupu.
- Azure publish profil, connection stringovi i OAuth tajne pohranjuju se izvan Gita.
- Gitleaks workflow skenira cijelu Git povijest pri svakom pushu na <code>main</code> i u pull requestovima.
- Početni filmovi, kina, osobe i kontaktni podaci su izmišljeni.
- Privilegirane role nisu dio javnog demo računa.

## Trenutačna ograničenja

- Checkout simulira potvrdu rezervacije; nije spojen na payment provider.
- Aplikacija trenutačno podržava rezervaciju jednog sjedala po kupnji.
- Uploadani vizuali zahtijevaju trajnu Azure pohranu kako bi preživjeli svaki deploy.
