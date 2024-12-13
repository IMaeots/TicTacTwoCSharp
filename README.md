## icd0008-24f

### Personal information about the owner
Full name: **Indrek Mäeots**<br>
Student Code: **233046IADB**<br>
School email: **inmaeo@taltech.ee**<br>
Uni-id: **inmaeo**

### File Structure:

- **Common**: Shared strings, utilities, entities.
- **ConsoleApp**: Contains all components necessary for the console application, including controllers and user interface elements.
- **Data**: Manages data access, manipulation, and persistence for the application.
- **GameLogic**: Holds UI game models and encapsulates their respective game logic & manipulation.
- **WebApp**: Contains all components necessary for the web application, including views & models.

### Helpful dev commands:

Overall
~~~sh
dotnet tool install --global dotnet-ef 
dotnet tool update --global dotnet-ef

dotnet tool install --global dotnet-aspnet-codegenerator
dotnet tool update --global dotnet-aspnet-codegenerator
~~~

Run from solution folder
~~~sh
dotnet ef migrations add InitialCreate --project Data --startup-project ConsoleApp
dotnet ef database update --project Data --startup-project ConsoleApp
dotnet ef database drop --project Data --startup-project ConsoleApp 
~~~

Run from web folder (scaffolding)
~~~sh
dotnet aspnet-codegenerator razorpage -m SaveGame -outDir Pages/Games -dc GameDbContext -udl --referenceScriptLibraries -f
dotnet aspnet-codegenerator razorpage -m SaveGameConfiguration -outDir Pages/Configurations -dc GameDbContext -udl --referenceScriptLibraries -f
~~~
