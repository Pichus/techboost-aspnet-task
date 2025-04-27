namespace techboost_aspnet.Exceptions;

public class UserCollectionEmpty : Exception
{
    public UserCollectionEmpty()
    {
    }

    public UserCollectionEmpty(string name)
        : base($"No collections for user {name} are found")
    {
    }
}