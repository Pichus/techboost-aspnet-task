namespace techboost_aspnet.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string name)
        : base($"Entity with name {name} not found")
    {
    }
}