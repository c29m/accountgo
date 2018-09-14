# AccountGo
Accounting System built in ASP.NET MVC is in early stage and lots of work to do but happy to share it to anyone. This will be very useful if you have future project to develop accounting system. We do the hard work for you!
It is initially designed for a small size businesses and the idea is to help them running efficient business by using Accounting System fit to them.

# Features
On a high level, this solution will provide modules including

1. Accounts Receivable
2. Accounts Payable
3. Inventory Control
4. Financial/Accounting

#FRONT-END
The screenshot below will be the future front-end. It is heavily under-development and you could be part of it. The project is "AccountGoWeb" and consuming the "Api" project.

Technology Stack:
- ASP.NET Core
- ReactJS
- MobX, React-MobX
- Axios
- Bootstrap
- D3
- React-router (on some pages)

Demo site (new UI) : http://accountgo.net

![accountgoweb](https://cloud.githubusercontent.com/assets/17961526/17953180/d2e7aac2-6aa3-11e6-8150-fe1b8274cf91.png)
![accountgoweb](https://cloud.githubusercontent.com/assets/17961526/17430653/0cf89cca-5b28-11e6-81dd-5f14695c8cfc.png)

# Publish Database
1. Open solution in VS. The SQL Database project is under Database\SQL.Accountgo
2. Right click the project Database\SQL.Accountgo and select Rebuild.
3. Right click the project Database\SQL.Accountgo and select Publish.
4. In "Target database connection", click "Edit" button.
5. Select existing database connection or create a new one.

# Setup Develoment Environment (Visual Studio Code)
In “Presentation” folder, there are three web projects. Go to demo site is http://accountgo.net
-	“Web” – an old web front-end.
-	“AccountGoWeb” – this is the future. Functionalities from the old "Web" front-end are soon transported to this project. Development under ASP.NET Core 2.1, ReactJS and MobX.
-	“Api” – ASP.NET REST API project to be consumed by “AccountGoWeb”.

AccountGoWeb/Api projects using ASP.NET Core 2.1.

“AccountGoWeb” use webpack so you also need to install webpack to the project.

1. Install Visual Studio Code.
1. Clone/Fork the latest repo here https://github.com/AccountGo/accountgo
1. Build the projects in this order. Core->Services->Dto->Api. To build the project, CD to project folder and execute "dotnet restore" then "dotnet build".
1. CD to AccountGoWeb project folder. 
1. Install webpack-cli. Execute "npm install -g webpack"
1. Install webpack. Execute "npm install -g webpack-cli"
1. Install gulp. Execute "npm install -g gulp"
1. In cmd, type and enter "gulp"
1. In cmd, type and enter "webpack"
1. In cmd, type and enter "dotnet build"
1. Run the front-end by typing "dotnet run"

# Run "Api" project
1. Run the api by typing "dotnet run". Make sure you CD to api project.
2. In api/appsettings.json, update properly your "LocalConnection" connection string.
3. In api/Startup.cs, then ConfigureServices method, set connectionString = Configuration["Data:LocalConnection:ConnectionString"];

# Initialized Data
On your first run, if you successfully Publish Database\SQL.AccountGo you will get an empty database. Let's get your DB Initialized with sample master data.'
1. Create your first user. registration page is hidden but you can go directly from your browser. (e.g. http://localhost/account/register)
2. Set password like 'P@ssword1'. More than 6 chars, 1 Caps, 1 special, 1 number. Note: Currently, even successfully register a new user, it will not inform you. Just try to login using your newly created user. This registratio page is temporary.
3. After successful user registration, the system automatically runs data initialization of the following.
- Company
- Chart of accounts/account classes
- Financial year
- Payment terms
- GL setting
- Tax
- Vendor
- Customer
- Items
- Banks
             

# Help Wanted
Wether you are a Developer, Consultant, Accountant, QA, Marketing expert, Project Manager we can all be part of this great and promising project.

If you are a developer and wanted to take part as contributor/collaborator we are happy to welcome you! To start with, you can visit the issues page and pick an issue that you would like to work on.

So go ahead, add your code. Looking forward to your first pull request.

# Contact Support
Feel free to email mvpsolution@gmail.com of any questions.
