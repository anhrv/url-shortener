# URL Shortener

A fullstack URL shortener application, made with ASP.NET Core Web API, Entity Framework, SQL Server, Redis and Angular/Bootstrap.

# How it works

### Client

The client side application is made with Angular and Bootstrap. The user can input a URL and recieve a short URL, which redirects him to the original.
The user can also limit the amount of clicks the short URL can recieve, as well as set an expiration time of the short URL.
The frontend is very simple, and it is also responsive.

### Server

The server side application is made with ASP.NET Core Web Api, Entity Framework, Sql Server and Redis. I also made an attempt to follow Clean Architecture, Repository pattern and best practices. Once the server recieves the request, it first checks if it is a valid URL. If not, an error is returned which is displayed to the client. The server checks if the provided URL has already been shortened, in which case it will return the existing short URL, but only if the limiting options were not set (clicks and expiration). If it was not already shortened, the server will generate a random 6-character unique key and return the short URL to the client. When a short URL is clicked, the server looks for the corresponding long URL and permanently redirects to it. It will also check the clicks and expiration time and delete the short URL if needed. Any error will be displayed to the client. Every CRUD operation is done on the cache (lazy loading and write-through) and database, meant to boost performance and scalability.

## Installation

If you wish to run the code on your machine, make sure you update the connection strings, in the client app (my-config.ts) and in the server app (appsettings.json) and run the migrations.
