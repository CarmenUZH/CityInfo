var builder = WebApplication.CreateBuilder(args);

// Starting point of our application

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); //Register required services - The reason things work

var app = builder.Build(); //build webaplication and returns it (app)

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); //Middleware, lets swagger appear
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
/*
 app.Run(async (context)=>{
await context.Response.WriteAsync("Hello World!");
}); 
//This would end up only writing "Hello World" when we run the application
 
 */


app.Run(); //starts it, at end
