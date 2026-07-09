Sustav za prodaju kino ulaznica

## Prvi testni deploy

Minimalni cilj prvog deploya je potvrditi da se aplikacija digne na testnom
okruzenju, spoji na SQL Server bazu, primijeni postojece migracije i omoguci
prijavu barem jednom korisniku s administracijskom rolom.

### Potrebne varijable

Obavezno:

```powershell
ASPNETCORE_ENVIRONMENT=Staging
ConnectionStrings__CinemaDbContext=<sql-server-connection-string>
SeedUsers__Admin__Email=<admin-email>
SeedUsers__Admin__Password=<admin-password>
SeedUsers__Admin__OIB=<11-znamenki>
SeedUsers__Admin__JMBAG=<13-znamenki>
```

Opcionalno, ako treba Manager korisnik odmah na testnom okruzenju:

```powershell
SeedUsers__Manager__Email=<manager-email>
SeedUsers__Manager__Password=<manager-password>
SeedUsers__Manager__OIB=<11-znamenki>
SeedUsers__Manager__JMBAG=<13-znamenki>
```

Opcionalno, samo ako se testira Google prijava:

```powershell
Authentication__Google__ClientId=<google-client-id>
Authentication__Google__ClientSecret=<google-client-secret>
```

Ako Google vrijednosti nisu postavljene, Google prijava se ne registrira i
nece se prikazati kao opcija prijave.

Opcionalno, ali preporuceno za test okruzenje koje se redeploya ili koristi
container:

```powershell
UploadStorage__RootPath=<persistent-upload-folder>
UploadStorage__RequestPath=/uploads
```

Ako `UploadStorage__RootPath` nije postavljen, aplikacija koristi
`wwwroot/uploads`. Na test serveru taj folder mora biti zapisiv i po mogucnosti
persistentan izmedu deployeva.

### Redoslijed deploya

```powershell
dotnet restore Vjezba\Vjezba.sln
dotnet build Vjezba\Vjezba.sln --configuration Release --no-restore
dotnet test Vjezba\Vjezba.sln --configuration Release --no-build
dotnet ef database update --project Vjezba\DAL\Vjezba.DAL.csproj --startup-project Vjezba\Web\Vjezba.Web.csproj --configuration Release
dotnet publish Vjezba\Web\Vjezba.Web.csproj --configuration Release --output <publish-folder>
```

Nakon deploya provjeri:

```text
GET /health
```

Ocekivani odgovor je `Healthy`. Health endpoint provjerava dostupnost baze i
provjerava da nema pending migracija.

Aplikacija se nece pokrenuti ako baza nije dostupna, migracije nisu
primijenjene, identity seed ne uspije ili upload storage nije spreman.

Nakon prvog uspjesnog deploya preporuceno je maknuti `SeedUsers__*__Password`
varijable iz trajne konfiguracije okruzenja. Seeder ce postojecem korisniku
i dalje moci potvrditi rolu ako je korisnik vec kreiran.
