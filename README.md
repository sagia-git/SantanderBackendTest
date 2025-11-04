# 📡 Santander Backend Test

RESTful API to retrieve the details of the best n stories from the Hacker News API, as determined by their score, where n is specified by the caller to the API. 

## 🚀 Features

- RESTful endpoints
- JSON-based request and response format  
- Rate limiting and retries for consuming HackerNews API
- RateLimit for Web API requests

## 🛠️ Technologies Used

- **Language:** C# 12
- **Framework:** .NET 8
- **Documentation:** Swagger / OpenAPI
- **Libraries** Newtonsoft, Polly

## 📦 Getting Started with Visual Studio

1. **Clone the repository**
```python
   git clone https://github.com/sagia-git/SantanderBackendTest.git
```

2. **Open the solution file**
- Launch Visual Studio.
- Open the .sln file from the cloned directory.

3. **Configure environment variables**
- Open appsettings.json and update settings as needed.

4. **Build and run the project**
- Press F5 to run the API with debugging.
- Or press Ctrl + F5 to run without debugging.

5. **Access the API**

- The API will be available at: https://localhost:7232/api/v0/

- Swagger UI will be available at: https://localhost:7232/swagger/index.html

## 📚 API Endpoints

| Method | Endpoint | Description |
|-----------------|-----------------|-----------------|
| GET     | /stories?storiesCount=n     |  Get the n best stories

