# Chirp
Repo for the Chirp project related to BDSA course on ITU 3. semester of SWU Bsc

# Razor Application
https://bdsagroup22chirprazor.azurewebsites.net/

# Creating new release with Version tag - Workflow
* First add a tag - replace x with the relevant version

 		git tag vx.x.x

* Then push tag to trigger the release workflow

		git push --tags

# Running the mssql image with Docker Desktop
**[macos]**
(https://hub.docker.com/_/microsoft-azure-sql-edge)
1) Pull docker image Azure SQL Edge (for macosArm)
	```docker pull mcr.microsoft.com/azure-sql-edge```
2) Run docker image
	```docker run --cap-add SYS_PTRACE -e 'ACCEPT_EULA=1' -e 'MSSQL_SA_PASSWORD=<ChoosePassword>' -p 1433:1433 --name <ChooseDatavaseServerName> -d mcr.microsoft.com/azure-sql-edge```

3) From now on start/stop image from docker desktop

**[windows]**
(https://hub.docker.com/_/microsoft-mssql-server)
1) Pull docker image Microsoft SQL Server 
	```docker pull mcr.microsoft.com/mssql/server```
2) Run docker image
	```docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<ChoosePassword>" -p 1433:1433 -name <ChooseDatavaseServerName> -d mcr.microsoft.com/mssql/server:latest```

3) From now on start/stop image from docker desktop
   

# Using the Chirp.Migrations project

## How to run:

**[macos]**
open Terminal

	export ASPNETCORE_ENVIRONMENT=<Development | Production>

**[windows]**
open Command Console
  
	set ASPNETCORE_ENVIRONMENT=<Development | Production>

**[macos & windows]**
go to project folder:

	cd chirp/src/Chirp.Migrations

### Available commands:
(For EF commands see: https://learn.microsoft.com/en-us/ef/core/cli/dotnet )

* Adding migrations to project first time:

    	dotnet ef migrations add Initial -o Migrations

* Adding migrations to project with changes:

    	dotnet ef migrations add <nameofchange> -o Migrations


* Removing migrations from project:

    	dotnet ef migrations remove


* Creating or updating database with migrations:

    	dotnet ef database update

* Removing database:

    	dotnet ef database drop


* Adding seeding data to database:

    	dotnet run seed	

* Adding recreating data to database:

    	dotnet run recreate

