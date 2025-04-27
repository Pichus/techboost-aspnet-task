namespace techboost_aspnet.Exceptions;

public class EntityAlreadyExistsException : Exception
{
    public EntityAlreadyExistsException()
    {
    }

    public EntityAlreadyExistsException(string name)
        : base($"Entity with name {name} already exists")
    {
    }
}