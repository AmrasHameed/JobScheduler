## JobScheduler (.NET + Quartz)

This is a simple job scheduling system built with **ASP.NET Core Web API** and **Quartz.NET**.  
It allows scheduling recurring jobs like printing `"Hello World"` at specified intervals — hourly, daily, or weekly.

##  Features

- Schedule recurring jobs via REST API
- Supports:
  - Hourly (at a specific minute)
  - Daily (at a specific hour and minute)
  - Weekly (on a specific day, hour, and minute)
- Uses Quartz.NET for background scheduling
- Swagger UI for easy testing

## Technologies Used

- ASP.NET Core Web API
- Quartz.NET
- Swagger / Swashbuckle

## 🔧 Setup Instructions

### 1. Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (7.0+ recommended)

### 2. Clone the repo

- git clone https://github.com/AmrasHameed/JobScheduler.git
- cd JobScheduler

### 3. Run the Project

- dotnet run
The app should start on:http://localhost:5139/

### 4. Use Swagger UI (Optional)

- Open your browser and go to http://localhost:5139/swagger
- Expand the JobController endpoint:
   - Choose POST /api/job/{type}
   - Enter the type (hourly, daily, or weekly)
   - Provide JSON body like 
       - POST /api/job/hourly JSON:{"minute": 15}
       - POST /api/job/daily JSON:{ "hour": 14, "minute": 30}
       - POST /api/job/weekly JSON:{"hour": 9, "minute": 0, "dayOfWeek": "MON"}
   - Execute the request



