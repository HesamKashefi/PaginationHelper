# PaginationHelper

PaginationHelper makes the pagination of your data easy and awesome in .net APIs!
But how?

## Build Status

[![build](https://github.com/HesamKashefi/PaginationHelper/actions/workflows/build.yml/badge.svg)](https://github.com/HesamKashefi/PaginationHelper/actions/workflows/build.yml)

## Installation

With dotnet cli

    dotnet add package PaginationHelper
----
Or with nuget package manager console
    
    Install-Package PaginationHelper

## Setup

PaginationHlper relies on `AutoMapper` to map your data into DTOs (Data Transfer Objects) so you need to install and configure AutoMapper with PaginationHelper.

In yuour `Startup.cs`'s ConfigureServices method add the following

    public void ConfigureServices(IServiceCollection services) 
    {
        services.AddAutoMapper(cfg => 
        {
            //Register type mappings
            //cfg.CreateMap<WeatherForecast, WeatherForecastDto>(); 
        },
        // assemblies containing AutoMapper Profiles
        typeof(Startup).Assembly);

        // Registering PaginationHlper
        services.AddPaginationHelper();
        ...
    }

## How to use

Inject `IPageHelper ` interface into your Controller or class.

    using Microsoft.AspNetCore.Mvc;
    using PaginationHelper;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IPageHelper _pageHelper;
        private readonly AppDbContext _context;

        public WeatherForecastController(IPageHelper pageHelper, AppDbContext context)
        {
            _pageHelper = pageHelper;
            _context = context;
        }
    }
----
Use `GetPageAsync` to get a page of data by passing an `ordered queriable` to it and pagination dto.
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] PaginationDto paginationDto)
    {
        var query = _context.WeatherForecasts.OrderBy(x => x.Date);
        var pagination = await _pageHelper.GetPageAsync(query, paginationDto);
        return Ok(pagination);
    }

----

If you want to map your data into DTOs :

Use `GetProjectedPageAsync<TSource, TTarget>` where TSource is your data type in the queriable and TTarget is your DTO type to get a page of data by passing an `ordered queriable` to it and pagination dto.

    [HttpGet]
    public async Task<IActionResult> GetProjected([FromQuery] PaginationDto paginationDto)
    {
        var query = _context.WeatherForecasts.OrderBy(x => x.Date);

        var pagination = await _pageHelper
        .GetProjectedPageAsync<WeatherForecast, WeatherForecastDto>(query, paginationDto);

        return Ok(pagination);
    }

## What do i get ?

You will get an envelope containing your requested page of data, count of the data received and a series of links to current, next, previous, first and last pages.

You will also get data about how many pages and records are availabe totally!

How cool is that?!

This is a sample data you get :

    {
        "data": [
            {
                "id": 11,
                "date": "2019-09-21T13:55:00.5810091+04:30",
                "temperatureC": -16,
                "temperatureF": 4,
                "summary": "Chilly"
            },
            {
                "id": 12,
                "date": "2019-09-22T13:55:00.5810097+03:30",
                "temperatureC": 32,
                "temperatureF": 89,
                "summary": "Scorching"
            },
            {
                "id": 13,
                "date": "2019-09-23T13:55:00.5810103+03:30",
                "temperatureC": 51,
                "temperatureF": 123,
                "summary": "Freezing"
            },
            {
                "id": 14,
                "date": "2019-09-24T13:55:00.5810109+03:30",
                "temperatureC": 30,
                "temperatureF": 85,
                "summary": "Sweltering"
            },
            {
                "id": 15,
                "date": "2019-09-25T13:55:00.5810115+03:30",
                "temperatureC": -15,
                "temperatureF": 6,
                "summary": "Freezing"
            }
        ],
        "error": null,
        "meta": {
            "status": 0,
            "count": 5,
            "links": {
                "pager": {
                    "numberOfPages": 4,
                    "currentPage": 3,
                    "totalRecords": 20,
                    "pageSize": 5
                },
                "self": "http://localhost:5000/WeatherForecast?pageSize=5&page=3",
                "nextPage": "http://localhost:5000/WeatherForecast?pageSize=5&page=4",
                "prevPage": "http://localhost:5000/WeatherForecast?pageSize=5&page=2",
                "firstPage": "http://localhost:5000/WeatherForecast?pageSize=5&page=1",
                "lastPage": "http://localhost:5000/WeatherForecast?pageSize=5&page=4"
            }
        }
    }
