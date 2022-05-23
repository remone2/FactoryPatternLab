public interface Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }
    public bool HasAccess { get; set; }

    public void BuildAuthString();
}

public class User : Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { get; set; } = false;

    public void BuildAuthString()
    {
        UserAuthString = UserName;
    }
}

public class Manager : Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { get; set; } = true;

    public void BuildAuthString()
    {
        UserAuthString = UserName + "MAN";
    }
}

public class Admin : Client
{
    public string UserName { get; set; }
    public string UserAuthString { get; set; }

    public bool HasAccess { get; set; } = true;

    public void BuildAuthString()
    {
        UserAuthString = UserName + "ADMIN";
    }
}

public interface AccessBehavior
{
    public Client Client { get; set; }

    public bool HandleAccess(Client client);
}

public class CheckString : AccessBehavior
{
    public Client Client { get; set; }

    public bool HandleAccess(Client client)
    {
        if (client.UserAuthString.EndsWith("ADMIN"))
        {
            return true;
        }
        else
            return false;
    }
}

public class SwitchAuth : AccessBehavior
{
    public Client Client { get; set; }

    public bool HandleAccess(Client client)
    {
        if (client.HasAccess == true)
        {
            return client.HasAccess = false;
        }
        else
        {
            return client.HasAccess = true;
        }
    }
}

public class ClientFactory
{
    Client client;

    public Client createClient(string clientType, string userName)
    {
        if (clientType == "User")
        {
            client = new User();
            client.UserName = userName;
            client.BuildAuthString();
            return client;
        }
        else if (clientType == "Manager")
        {
            client = new Manager();
            client.UserName = userName;
            client.BuildAuthString();
            return client;
        }
        else if (clientType == "Admin")
        {
            client = new Admin();
            client.UserName = userName;
            client.BuildAuthString();
            return client;
        }
        else
            throw new Exception();
    }
}

public abstract class ClientHandler
{
    ClientFactory factory { get; set; }

    public abstract void CreateClient(string clientType, string userName);
}

public class RetailClientHandler : ClientHandler
{
    ClientFactory factory;

    public override void CreateClient(string clientType, string userName)
    {
        factory = new ClientFactory();
        Client client = factory.createClient(clientType, userName);

        AccessBehavior accessBehavior;

        if (clientType == "User" || clientType == "Manager")
        {
            accessBehavior = new SwitchAuth();
            accessBehavior.HandleAccess(client);
        }
        else if (clientType == "Admin")
        {
            accessBehavior = new CheckString();
            accessBehavior.HandleAccess(client);
        }
        else
        {
            throw new Exception();
        }
    }
}

public class EnterpriseClientHandler : ClientHandler
{
    ClientFactory factory;

    public override void CreateClient(string clientType, string userName)
    {
        factory = new ClientFactory();
        Client client = factory.createClient(clientType, userName);

        AccessBehavior accessBehavior;

        if (clientType == "User")
        {
            accessBehavior = new SwitchAuth();
            accessBehavior.HandleAccess(client);
        }
        else if (clientType == "Admin" || clientType == "Manager")
        {
            accessBehavior = new CheckString();
            accessBehavior.HandleAccess(client);
        }
        else
        {
            throw new Exception();
        }
    }
}