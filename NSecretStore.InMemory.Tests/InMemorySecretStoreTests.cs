using System.Collections.Generic;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace NSecretStore.InMemory.Tests
{
    public class InMemorySecretStoreTests
    {
        private InMemorySecretStore Sut { get; } = new InMemorySecretStore();
        private Fixture Fixture { get; } = new Fixture();

        [Test]
        public void GetSecret_Should_Throw_KeyNotFoundException_When_Key_Not_Set()
        {
            var key = Fixture.Create<string>();
            Assert.Throws<KeyNotFoundException>(() => Sut.GetSecret(key));
        }

        [Test]
        public void GetSecret_Should_Return_Value_When_Set_By_SetSecret()
        {
            var key = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            Sut.SetSecret(key, value);
            Assert.AreEqual(value, Sut.GetSecret(key));
        }

        [Test]
        public void GetSecret_Should_Throw_KeyNotFoundException_When_Key_Deleted()
        {
            var key = Fixture.Create<string>();
            var value = Fixture.Create<string>();
            Sut.SetSecret(key, value);
            Sut.DeleteSecret(key);
            Assert.Throws<KeyNotFoundException>(() => Sut.GetSecret(key));
        }

        [Test]
        public void DeleteSecret_Should_Throw_KeyNotFoundException_When_Key_Not_Set()
        {
            var key = Fixture.Create<string>();
            Assert.Throws<KeyNotFoundException>(() => Sut.DeleteSecret(key));
        }
    }
}
