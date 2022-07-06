var builder = WebApplication.CreateBuilder(args);

// Starting point of our application, Main Methods gets created behind the scenes

builder.Services.AddControllers();//Registers necessary services to implement controllers (Remember we're in MVC)
//Addcontrollesrwithviews would also regiester services for views, but this application is just for api so we dont really need views - Maybe better for ASPtest
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
app.UseRouting(); //Marks the position in the middleware pipeline where a routing decision is made
app.UseAuthorization(); //Authentication should be added pretty early on 
app.UseEndpoints(endpoints => //Marks the position in the middleware pipeline where the selected endpoint is executed
{
    endpoints.MapControllers();//add endpoints without specifying routes (specify routes with attributes)
});

app.MapControllers();
/*
 app.Run(async (context)=>{
await context.Response.WriteAsync("Hello World!");
}); 
//This would end up only writing "Hello World" when we run the application
 
 */


app.Run(); //starts it, at end
