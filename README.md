````markdown
# WeatherApp

A weather tracking application built with ASP.NET Core MVC and styled with custom CSS. Users can select cities, view current weather data, and save or review weather history.

---

## Features

- **City-based Weather Search**
- **Real-time Temperature, Humidity & Forecast Data**
- **Save and View Weather History**
- **Glassmorphism UI with CSS blur effects**
- **Filter history by city**
- **Responsive design for all devices**

---

## Technologies Used

- ASP.NET Core MVC (.NET 6+)
- C#
- Entity Framework Core
- Razor Pages
- Custom CSS (Glassmorphism + Animations)
- HTML5
- Bootstrap 5

---

## Setup Instructions

1. **Clone the repository**

```bash
git clone https://github.com/romey101/WeatherApp.git
cd WeatherApp
````

2. **Open the solution**

* For **Visual Studio**: Open the `WeatherApp.sln` file
* For **VS Code**: Open the entire project folder

3. **Set up the database**

> This app uses **Entity Framework Core**. Run the following command to apply migrations and create the database:

```bash
dotnet ef database update
```

4. **Run the application**

```bash
dotnet run
```

> Then open your browser and visit:
> `http://localhost:5249/Weather`

---

## Screenshots


### Weather Home Page

![Weather Home](screenshots/weather_home.png)

### Weather Login Page

![Weather Login](screenshots/weather_login.png)

### Weather Search Page

![Weather Search](screenshots/weather_search.png)

### Weather History Page

![Weather History](screenshots/weather_history.png)

---

## Project Structure

```
WeatherApp/
├── Controllers/
├── Models/
├── Views/
│   ├── Weather/
│   └── Shared/
├── wwwroot/
│   ├── css/
│   └── images/
├── Data/
├── appsettings.json
└── Program.cs
```

---



## Author

**Maram Alshammary**
[1romey101@gmail.com](mailto:1romey101@gmail.com)
[GitHub Profile](https://github.com/romey101)

```

---

```
