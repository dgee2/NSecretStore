using System.Collections.Generic;
using Xunit;
using Ploeh.AutoFixture.Xunit2;

namespace NSecretStore.InMemory.Tests
{
    public class InMemorySecretStoreTests
    {
        public InMemorySecretStore Sut { get; } = new InMemorySecretStore();

        [Theory, AutoData]
        public void GetSecret_Should_Throw_KeyNotFoundException_When_Key_Not_Set(string key)
        {
            Assert.Throws<KeyNotFoundException>(() => Sut.GetSecret(key));
        }

        [Theory, AutoData]
        public void GetSecret_Should_Return_Value_When_Set_By_SetSecret(string key, string value)
        {
            Sut.SetSecret(key, value);
            Assert.Equal(value, Sut.GetSecret(key));
        }

        [Theory, AutoData]
        public void GetSecret_Should_Throw_KeyNotFoundException_When_Key_Deleted(string key,string value)
        {
            Sut.SetSecret(key, value);
            Sut.DeleteSecret(key);
            Assert.Throws<KeyNotFoundException>(() => Sut.GetSecret(key));
        }

        [Theory,AutoData]
        public void DeleteSecret_Should_Throw_KeyNotFoundException_When_Key_Not_Set(string key)
        {
            Assert.Throws<KeyNotFoundException>(() => Sut.DeleteSecret(key));
        }
    }
}
