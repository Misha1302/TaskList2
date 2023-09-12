namespace TaskList2;

using System.Data;
using Dapper;
using Npgsql;
using TaskList2.Controllers;

public class PersonRepository : IPersonRepository
{
    private readonly string _connectionString;

    public PersonRepository(string conn)
    {
        _connectionString = conn;
    }

    public void Delete(int id)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);

        const string sqlQuery = "DELETE FROM \"TestTable\" WHERE \"Id\" = @id";
        db.Execute(sqlQuery, id);
    }

    public List<Person> GetTestTable()
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);
        return db.Query<Person>("SELECT * FROM \"TestTable\"").ToList();
    }

    public Person? Get(int id)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);
        return db.Query<Person>("SELECT * FROM \"TestTable\" WHERE \"Id\" = @id", new { id }).FirstOrDefault();
    }

    public void Create(Person person)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);

        const string sqlQuery =
            "INSERT INTO \"TestTable\" (\"Name\", \"Email\", \"Password\", \"Id\") VALUES(@Name, @Email, @Password, @Id)";
        db.Execute(sqlQuery, person);

        // если мы хотим получить id добавленного пользователя
        //var sqlQuery = "INSERT INTO \"TestTable\" (Name, Age) VALUES(@Name, @Age); SELECT CAST(SCOPE_IDENTITY() as int)";
        //int? PersonId = db.Query<int>(sqlQuery, Person).FirstOrDefault();
        //Person.Id = PersonId.Value;
    }

    public void Update(Person person)
    {
        using IDbConnection db = new NpgsqlConnection(_connectionString);

        const string sqlQuery =
            "UPDATE \"TestTable\" SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id";
        db.Execute(sqlQuery, person);
    }
}