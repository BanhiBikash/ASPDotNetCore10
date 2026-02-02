var builder = WebApplication.CreateBuilder(args);

//adds all controllers as services
builder.Services.AddControllers();

//creates an instance of app with all its dependencies
var app = builder.Build();

//allows the actions in the controller
app.MapControllers();

app.Run();
