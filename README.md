Sustav za prodaju kino ulaznica

Za lab 3:

U lab-3 folderu se nalazi semantic model i sitemap

Docker i migracije:
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=***REMOVED***" -p 1433:1433 --name cinemaDB --hostname cinemaDB -d mcr.microsoft.com/mssql/server:latest

dotnet ef migrations add InitialCreate --project .\DAL\ --startup-project .\Web\ --context CinemaDbContext

dotnet ef database update --project .\DAL\ --startup-project .\Web\ --context CinemaDbContext
