using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Polly;
using RestAPICore.Models;
using RestAPICore.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DI
    {
        public static void AddGooglePhotos(this IServiceCollection services)
            => services.AddGooglePhotos(_ => { });

        private static string sectionKey = $"{nameof(RestAPICore)}:{nameof(GooglePhotosOptions)}";

        public static void AddGooglePhotos(this IServiceCollection services, Action<GooglePhotosOptions> configure)
        {
            services.AddSingleton<IConfigureOptions<GooglePhotosOptions>>(s =>
            {
#nullable enable
                var configuration = s.GetService<IConfiguration?>();
                return new ConfigureOptions<GooglePhotosOptions>(options => configuration?.Bind(sectionKey, options));
            });
            services.AddHttpClient<GooglePhotosService>((s, client) =>
            {
                var configuration = s.GetRequiredService<IConfiguration>();
                var options = configuration.GetSection(sectionKey).Get<GooglePhotosOptions>();
                options = options ?? new GooglePhotosOptions();//we use default BaseAddress if no config object injected in
                client.BaseAddress = new Uri(options.BaseAddress);
                client.DefaultRequestHeaders.Add("User-Agent", $"{nameof(RestAPICore)}.{AppDomain.CurrentDomain.FriendlyName}.{Environment.MachineName}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                client.Timeout = Timeout.InfiniteTimeSpan;
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            })
            .AddTransientHttpErrorPolicy(policyBuilder =>
            {
                return policyBuilder.RetryAsync(retryCount: 3);
            })
            //https://github.com/aspnet/AspNetCore/issues/6804
            .SetHandlerLifetime(Timeout.InfiniteTimeSpan);
        }
    }
}