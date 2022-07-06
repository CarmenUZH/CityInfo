var builder = WebApplication.CreateBuilder(args);

// Starting point of our application, Main Methods gets created behind the scenes

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Register required services - The reason things work

var app = builder.Build(); //build webaplication and returns it (app)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); //Generates a file, useswaggerui makes file pretty
    app.UseSwaggerUI(); //Middleware, lets swagger appear //Whenever a request comes in, documentation will be shown
}

app.UseHttpsRedirection();

app.UseAuthorization(); //Authentication should be added pretty early on 

app.MapControllers();
/*
 app.Run(async (context)=>{
await context.Response.WriteAsync("Hello World!");
}); 
//This would end up only writing "Hello World" when we run the application
 
 */


app.Run(); //starts it, at end
