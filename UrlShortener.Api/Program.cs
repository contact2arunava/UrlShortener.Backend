using UrlShortener.Service;
using UrlShortener.Data;

//using Microsoft.Data.SqlClient;
//using Swashbuckle.AspNetCore.SwaggerUI; // required for UseSwaggerUI extension
//using Swashbuckle.AspNetCore.SwaggerGen;
//using UrlShortener.Data.Interfaces; // optional; package must be installed

var builder = WebApplication.CreateBuilder(args);

/*Add Data Layer to Container.
The reference to the Data layer is coming through the Service Layer, which is referenced by the API layer.
This is a common pattern in layered architecture*/
builder.Services.AddDataLayer(builder.Configuration);


builder.Services.AddServiceLayer();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // requires Swashbuckle.AspNetCore package

var app = builder.Build();

app.UseSwagger();       // enable swagger JSON endpoint
app.UseSwaggerUI();     // enable swagger UI

app.MapControllers();

app.Run();
