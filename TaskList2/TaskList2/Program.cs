using Npgsql;

var conn = new NpgsqlConnection("Host=localhost;Port=5432;Database=testDb;Username=postgres;Password=qwerty");

conn.Open();

// Passing PostGre SQL Function Name
var command = new NpgsqlCommand("EXE GetEmployeePrintInfo", conn);

// Execute the query and obtain a result set
var reader = command.ExecuteReader();

// Reading from the database rows
var listOfManager = new List<string>();

while (reader.Read())
{
    var wsManager =
        reader["WSManager"].ToString() ??
        throw new InvalidOperationException(); // Remember Type Casting is required here it has to be according to database column data type
    listOfManager.Add(wsManager);
}

reader.Close();

command.Dispose();
conn.Close();

return;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();