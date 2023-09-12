namespace TaskList2.Controllers;

public sealed record Person(string Name, string Email, string Password, int Id = 0)
{
    public int Id { get; init; } = Id == default ? Email.GetHashOfString() : Id;
}