using BooksAPI;

var builder = WebApplication.CreateBuilder(args);
builder.AddDependencyInjectionServices();

var app = builder.Build();
app.AddApplicationMiddelweares();

