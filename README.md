# Shooping Food


How to Run an Shooping Food Application

Introduction

This README provides step-by-step instructions for setting up and running an MVC Shooping Food application. It outlines the necessary prerequisites, configuration steps, and commands to help you successfully launch and test the application.



Prerequisites

Before you begin, ensure that your development environment meets the following requirements:

.NET SDK: Version 6.0 or higher. 
Visual Studio 2022 (or higher) with ASP.NET and web development workloads installed.
MY SQL Server




Cloning the Repository

Clone the repository to your local development environment using the following command:

git clone https://github.com/cnunes4/shoopingFood.git
cd KAApi




Setting Up the Environment

Restore Dependencies:

Run the following command to restore the required NuGet packages:

dotnet restore

Database Configuration:

Update the appsettings.json file with your database connection string.
Apply database migrations to set up the schema:
dotnet ef database update


Running the Application

To run the application, use the following command:

dotnet run

Alternatively, you can run the application using Visual Studio:
Open the .sln solution file in Visual Studio.
Press F5 to run in Debug mode or Ctrl + F5 to run without debugging.



Accessing the Application

Once the application is running, you can access it by navigating to:
http://localhost:5147/




Testing the Application

Use Postman or Swagger UI (if configured) to test API endpoints.
Swagger, if set up, can be accessed at:
http://localhost:5019/



Troubleshooting Tips

Ensure your .NET SDK version is up to date.
Verify that all dependencies have been restored with dotnet restore.
Confirm that the database connection string is correctly set in appsettings.json.
