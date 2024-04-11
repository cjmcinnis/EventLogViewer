using System;
using System.Security;

namespace EventLogParser;

public class ComputerConnection
{
    private string? _username;
    private SecureString? _password;
    private string? _hostname;
    private ConnectionID _ID;


    public string? Username
    {
        get => _username;
        set =>  _username = value;
    }
    public SecureString? Password
    {
        get => _password;
        set => _password = value;
    }
    public string? Hostname
    {
        get => _hostname;
        set => _hostname = value;
    }
    public ConnectionID ID
    {
        get => _ID;
    }

    public ComputerConnection(string username, string password, string hostname)
    {
        // convert the password to a securestring.
        SecureString securePassword = ConvertStringToSecureString(password);

        Username = username;
        Password = securePassword;
        Hostname = hostname;
        _ID = new ConnectionID();
    }

    // Converts a string to a secure string
    // @param string : the string to convert
    private SecureString ConvertStringToSecureString(string password)
    {
        SecureString securePassword = new SecureString();
        foreach (char c in password)
        {
            securePassword.AppendChar(c);
        }
        return securePassword;

    }

}

public readonly struct ConnectionID
{
    public static ConnectionID New() => new ConnectionID(Guid.NewGuid());

    public Guid Value { get; }

    public ConnectionID(Guid value)
    {
        Value = value;
    }

    public override bool Equals(object obj)
    {
        return obj is ConnectionID && Equals(obj);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public bool Equals(ConnectionID identifier)
    {
        return Value == identifier.Value;
    }

    public static bool operator ==(ConnectionID first, ConnectionID second)
    {
        return first.Equals(second);
    }

    public static bool operator !=(ConnectionID first, ConnectionID second)
    {
        return first.Equals(second) == false;
    }
}
