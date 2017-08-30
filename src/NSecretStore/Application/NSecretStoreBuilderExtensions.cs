using System;
using NSecretStore;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    public static class NSecretStoreBuilderExtensions
    {
        public static IApplicationBuilder UseNSecretStore(this IApplicationBuilder app,
        Action<NSecretStoreOptions> setupAction = null)
        {
            var options = new NSecretStoreOptions();
            setupAction?.Invoke(options);

            return app.UseMiddleware<NSecretStoreMiddleware>(options);
        }


    }
}
