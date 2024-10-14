# AdoptionHub
Our project “Adoption Hub” will be a web application for use by foster adoption agencies. The purpose of the project is to allow agencies to manage the pets that are in their system and present them to potential adopters.  

## Features 
The following features will make up the core functionality of the application: 
- Two login portals, one for backend users and one for frontend users. 
- A backend dashboard which allows administrators to add pets and fosters to edit information about the pets. 
- A front end which displays pets to potential adopters and allows them to contact the foster agency. 

## Setup
- Clone the repo to your local machine.
- Add `appsettings.Development.json` as per the template `appsettings.Development.json.example` and add the connection string for your local database.

## Update Models and Database

### To update models from changes made to the database

- Ensure you have `dotnet` insalled or install with `dotnet tool install --global dotnet-ef
`.
- Run `dotnet ef dbcontext scaffold "YOUR CONNECTION STRING" Pomelo.EntityFrameworkCore.MySql --output-dir Models --force --context ApplicationDbContext --no-onconfiguring --context-dir Contexts`.
- Run `Add-Migration delete` to update the ModelSnapshot.cs file.
- Delete the newly created `delete` migration file so that `Update-Database` won't try to apply the changes that were scaffolded from the database.

### To update database from changes made to models

- Run `Add-Migration <migration-name>`
- Run `Update-Database`
 
