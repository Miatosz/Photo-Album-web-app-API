
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
* [CI/CD](#cicd)
* [Setup](#setup)


---

<!-- ABOUT THE PROJECT -->
## About The Project

It is a web application project that is a social networking site with a images sharing. It contains:
* creating your profile,
* sharing own images,
* the ability to create albums and describe them,
* adding comments to photos and replies,
* liking photos and comments
* following other users

---

<!-- USED TECHNOLOGIES -->
### Used Technologies

The application has been clearly divided into the backend and frontend parts. The backend is implemented using .NET Core and the frontend is carried out using the Angular framework.

Backend
* ASP.NET Core Web API
* MsSQL
* Entity Framework Core
* AutoMapper
* JWTBearer Authentication
* NUnit
* Moq

Frontend
* Angular
* TypeScript
* HTML
* CSS

---

## CI/CD
This project includes a GitHub Actions workflow that automatically builds the project and runs tests on each push or pull request.

---

<!-- Setup -->
## Setup
To open and run this project, follow this commands:
```bash
1. git clone https://github.com/Miatosz/Photo-Album-web-app-API.git
2. cd .\Photo-Album-web-app-API\
3. in Appsettings.json specify your database connection
4. dotnet ef database update
5. dotnet run
```

Then you can test app in e.g. Postman on port 5001(https://localhost:5001/)


