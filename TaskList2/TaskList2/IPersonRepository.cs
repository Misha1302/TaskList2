namespace TaskList2;

using TaskList2.Controllers;

public interface IPersonRepository
{
    void Create(Person person);
    void Delete(int id);
    Person? Get(int id);
    List<Person> GetTestTable();
    void Update(Person person);
}