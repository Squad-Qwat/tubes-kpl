using PaperNest_API.View;

// Cek argumen untuk menentukan mode yang dijalankan
bool runCliMode = Environment.GetCommandLineArgs().Length > 1 && 
                 Environment.GetCommandLineArgs()[1].ToLower() == "--cli";

if (runCliMode)
{
 
    Console.WriteLine("Menjalankan PaperNest dalam mode CLI...");
    var cliView = new CLIView();
    cliView.Start();
    return;
}

var builder = WebApplication.CreateBuilder();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
