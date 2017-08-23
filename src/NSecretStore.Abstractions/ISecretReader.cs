namespace NSecretStore.Abstractions
{
    public interface ISecretReader
    {
        string GetSecret(string key);
        void SetSecret(string id, string value);
        void DeleteSecret(string id);
    }
}