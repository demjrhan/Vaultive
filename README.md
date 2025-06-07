# Vaultive
Vaultive is a dynamic movie streaming website that features short films, trailers, and detailed movie information, supported by clear documentation and an engaging user experience.

## Screenshots from website

<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/home.png" alt="Home Page" width="1000"/>
</p>
<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/movies.png" alt="Movies Page" width="1000"/>
</p>

<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/streaming_services.png" alt="Streaming Services Page" width="1000"/>
</p>
<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/details.png" alt="Details Of Movie" width="1000"/>
</p>
<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/details_review.png" alt="Details Of Movie Review Section" width="1000"/>
</p>

## Analytical Diagram
<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/Vaultive%20-%20Analytical.png" alt="Analytical Diagram" width="1000"/>
</p>

## Design Diagram
<p align="center">
    <img src="https://github.com/demjrhan/Vaultive/blob/main/Documentation/Vaultive%20-%20Design.png" alt="Design Diagram" width="1000"/>
</p>


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

