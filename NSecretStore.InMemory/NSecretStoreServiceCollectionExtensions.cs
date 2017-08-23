using NSecretStore.Abstractions;
using NSecretStore.InMemory;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class NSecretStoreServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemorySecretStore(this IServiceCollection services)
        {
            services.AddSingleton<ISecretReader>(new InMemorySecretStore());
            return services;
        }
    }
}