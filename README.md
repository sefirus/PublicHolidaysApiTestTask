# Public Holidays API

This ASP.NET Core Web API provides information about public holidays for various countries. It integrates with an external service to fetch holiday data, caches the results, and exposes endpoints to:

- Retrieve available Countries.
- Retrieve holidays grouped by month for a given country and year.
- Check the status of a specific day (workday, free day, or holiday).
- Calculate the maximum number of consecutive free days (holidays plus weekends) in a year.

The solution follows an Onion Architecture with clearly separated domain, application, infrastructure, and API layers.

### Running Locally

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/sefirus/PublicHolidaysApiTestTask.git
   cd PublicHolidaysApiTestTask
   ```

2. **Configure the Application:**  
   Update `appsettings.json` with your database connection string.

3. **Build and Run:**

   ```bash
   dotnet build --configuration Release
   dotnet run --configuration Release
   ```

4. **API Documentation:**  
   Navigate to `https://localhost:<port>/swagger` to explore the endpoints.

## Deployment

This project is configured for deployment to Azure Web Apps using GitHub Actions. For more details, see the [https://public-holidays-test-task.azurewebsites.net/swagger/index.html](url)
