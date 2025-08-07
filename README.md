# Vaultive

![HTML5](https://img.shields.io/badge/html5-%23E34F26.svg?style=for-the-badge&logo=html5&logoColor=white) ![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) ![CSS3](https://img.shields.io/badge/css3-%231572B6.svg?style=for-the-badge&logo=css3&logoColor=white) ![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white) ![SQLite](https://img.shields.io/badge/sqlite-%2307405e.svg?style=for-the-badge&logo=sqlite&logoColor=white) ![JavaScript](https://img.shields.io/badge/javascript-%23323330.svg?style=for-the-badge&logo=javascript&logoColor=%23F7DF1E) ![Swagger](https://img.shields.io/badge/swagger-%2385EA2D.svg?style=for-the-badge&logo=swagger&logoColor=black)

Vaultive is a dynamic movie streaming website that features short films, trailers, and detailed movie information, supported by clear documentation and an engaging user experience.

## Screenshots from website

<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/home.gif" alt="Home Page" width="1000"/>
</a>
<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/movies.png" alt="Movies Page" width="1000"/>
</a>

<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/streaming_services.png" alt="Streaming Services Page" width="1000"/>
</a>
<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/details.gif" alt="Details Of Movie" width="1000"/>
</a>
<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/details_review.gif" alt="Details Of Movie Review Section" width="1000"/>
</a>

## Analytical Diagram
<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/Vaultive%20-%20Analytical.png" alt="Analytical Diagram" width="1000"/>
</a>

## Design Diagram
<a href="https://github.com/demjrhan/Vaultive" target="_blank">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/Vaultive%20-%20Design.png" alt="Design Diagram" width="1000"/>
</a>


## 📂 Database Details
Sample Data: `Backend/Project/Context/SampleData.cs`  

Database Script: `MasterContext.cs`  

Models: `Models/...`

Configurations: `Configurations/...`


## Build Instructions

### Navigate to the backend directory  
- cd Vaultive/Project/Backend
- mkdir Migrations
- dotnet restore  
- dotnet build  
- dotnet ef migrations add InitialCreate  
- dotnet ef database update  
- dotnet run  

