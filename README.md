
<!-- PROJECT LOGO -->
<br />
<p align="center">

  <h1 align="center">Photos Albums </h1>

  <p align="center">
    Social media platform with images sharing and the ability to add likes, comments and follow other users.
    <br />
    <br />
  </p>
</p>

<!-- TABLE OF CONTENTS -->
## Table of Contents

* [About the Project](#about-the-project)
* [Used Technologies](#used-technologies)
* [Current State of Project](#current-state-of-project)
* [Getting Started](#getting-started)
* [Usage](#usage)
* [Contributing](#contributing)
* [Contact](#contact)



<!-- ABOUT THE PROJECT -->
## About The Project

It is a web application project that is a social networking site with a images sharing. It contains:
* creating your profile,
* sharing own images,
* the ability to create albums and describe them,
* adding comments to photos and replies,
* liking photos and comments
* following other users

<!-- USED TECHNOLOGIES -->
### Used Technologies

The application has been clearly divided into the backend and frontend parts. The backend is implemented using .NET Core and the frontend is carried out using the Angular framework.

Backend
* [ASP.NET Core 5.0](https://docs.microsoft.com/pl-pl/aspnet/core/?view=aspnetcore-3.1)
* [MsSQL](https://docs.microsoft.com/pl-pl/sql/?view=sql-server-ver15)
* [Entity Framework Core](https://docs.microsoft.com/en-US/ef/core/)
* [ASP.NET Core Identity](https://docs.microsoft.com/en-US/aspnet/core/security/authentication/identity?view=aspnetcore-5.0&tabs=visual-studio)
* [AutoMapper](https://automapper.org/)
* JWTBearer Authentication
* [NUnit](https://nunit.org/)
* [Moq](https://github.com/Moq/moq4/wiki/Quickstart)


Frontend
* [Angular](https://angular.io/)
* [TypeScript](https://www.typescriptlang.org/)
* [HTML](https://developer.mozilla.org/en-US/docs/Web/HTML)
* [CSS](https://developer.mozilla.org/en-US/docs/Learn/Getting_started_with_the_web/CSS_basics)

<!-- CURRENT STATE OF THE PROJECT -->
## Current State of Project

Currently, an MVP (Minimal Value Project) is being created, where the backend already has part of the implementation.

<!-- Setup -->
## Setup
To open and run this project, follow this commands:
1. git clone https://github.com/Miatosz/Photo-Album-web-app-API.git
2. cd .\Photo-Album-web-app-API\
3. in Appsettings.json specify your database connection
4. dotnet ef database update
5. dotnet run

Then you can test app in e.g. Postman on port 5001(https://localhost:5001/)


