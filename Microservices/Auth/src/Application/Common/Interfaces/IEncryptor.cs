namespace Auth.Application.Common.Interfaces
{
    public interface IEncryptor
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}