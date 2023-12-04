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
...

# Using the Chirp.Migrations project

## How to run:

**[macos]**
1) open Terminal

		export ASPNETCORE_ENVIRONMENT=<Development | Production>

**[windows]**
1) open Command Console
  
		set ASPNETCORE_ENVIRONMENT=<Development | Production>

**[macos & windows]**
2) go to project folder:

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

