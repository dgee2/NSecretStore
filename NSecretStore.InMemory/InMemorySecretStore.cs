using System.Collections.Generic;
using NSecretStore.Abstractions;

namespace NSecretStore.InMemory
{
    public class InMemorySecretStore : ISecretReader
    {
        private readonly IDictionary<string, string> _backingData = new Dictionary<string, string>();

        public string GetSecret(string key)
        {
            if (!_backingData.ContainsKey(key))
            {
                throw new KeyNotFoundException();
            }
            return _backingData[key];
        }

        public void SetSecret(string key, string value)
        {
            _backingData[key] = value;
        }

        public void DeleteSecret(string key)
        {
            if (!_backingData.Remove(key))
            {
                throw new KeyNotFoundException();
            }
        }
    }
}
