# KinoKlik

KinoKlik je ASP.NET Core aplikacija za upravljanje kinima, filmovima, projekcijama, sjedalima, kupcima i ulaznicama. Javna kupnja vodi korisnika kroz odabir kina, filma, projekcije i sjedala, a administrativne role upravljaju podacima i API-jem.

## Arhitektura

- `Model` — entiteti i poslovni modeli.
- `DAL` — `CinemaDbContext`, EF Core konfiguracija, seed podaci i migracije.
- `Web` — MVC sučelje, API kontroleri, Identity, TicketBuilder i upload postera.
- `Tests` — integracijski testovi API-ja, autorizacije, globalne pretrage, health endpointa i sigurnosnih pravila ulaznica.

## Lokalni preduvjeti

- .NET 8 SDK
- SQL Server ili LocalDB
- `dotnet-ef` alat, istog glavnog izdanja kao EF Core paketi

## Lokalno pokretanje

```powershell
dotnet restore Vjezba\Vjezba.sln
dotnet build Vjezba\Vjezba.sln --configuration Release --no-restore
dotnet test Vjezba\Vjezba.sln --configuration Release --no-build
dotnet ef database update --project Vjezba\DAL\Vjezba.DAL.csproj --startup-project Vjezba\Web\Vjezba.Web.csproj
dotnet run --project Vjezba\Web\Vjezba.Web.csproj
```

Ako `dotnet ef` nije globalno instaliran, možeš ga instalirati kao lokalni alat iz manifesta:

```powershell
dotnet tool restore --tool-manifest Vjezba\Web\dotnet-tools.json
```

## Migracije

Migracije se ne primjenjuju automatski pri produkcijskom startupu. Prije deploya koji sadrži novu migraciju prvo primijeni migraciju na ciljnu bazu, a tek zatim deployaj aplikaciju.

```powershell
dotnet ef database update --project Vjezba\DAL\Vjezba.DAL.csproj --startup-project Vjezba\Web\Vjezba.Web.csproj --configuration Release --connection "<production-connection-string>"
```

## Konfiguracija okruženja

U produkciji se vrijednosti postavljaju kroz Azure App Service Environment variables / Connection strings, nikad u repozitorij. Potrebna su samo imena varijabli, bez javnih vrijednosti:

```text
ASPNETCORE_ENVIRONMENT
ConnectionStrings__CinemaDbContext
UploadStorage__RootPath
UploadStorage__RequestPath
Authentication__Google__ClientId
Authentication__Google__ClientSecret
```

`UploadStorage__RootPath` treba pokazivati na zapisivu i trajnu lokaciju ako se uploadani posteri moraju zadržati između deployeva.

## Role i pristup

- `Admin` — puni pristup, uključujući brisanje.
- `Manager` — upravljanje podacima bez administrativnog brisanja.
- Anonimni korisnik — javni katalog, globalna pretraga javnih podataka i TicketBuilder kupnja.

Produkcijski korisnici i njihove lozinke provisioniraju se odvojeno od izvornog koda. Lozinke se ne objavljuju u README-u niti u seed podacima za produkciju.

## Deploy

Azure aplikacija: [KinoKlik](https://cinema-bv-fuheftdfbyazaqea.italynorth-01.azurewebsites.net/)

GitHub Actions workflow radi restore, build, test, publish, deploy na Azure App Service i nakon deploya provjerava `/health`. Workflow koristi GitHub Secret za publish profile; publish profili i connection stringovi nisu tracked datoteke.

Health endpointi:

- `/health` — readiness: baza je dostupna i nema pending migracija.
- `/health/live` — liveness: ASP.NET proces radi bez ovisnosti o bazi.

## Demo podaci i vizuali

Podaci, nazivi filmova, kina i vizuali u projektu su izmišljeni. Za demonstraciju se ne koriste stvarni OIB, JMBAG, privatni telefon ni privatna e-pošta. Statički demo posteri trebaju biti u commitanoj mapi izvan `wwwroot/uploads`; uploadani sadržaj ostaje runtime podatak.
