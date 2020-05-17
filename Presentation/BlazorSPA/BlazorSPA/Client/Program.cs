using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Blazored.LocalStorage;
using BlazorSPA.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;

namespace BlazorSPA.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU2MjgxQDMxMzgyZTMxMmUzMFVFU0pIVWVHcm1OQ1dGRjNaS2IwZmlTeWMwak1IbUsyNTFteFdMKzlDelU9;MjU2MjgyQDMxMzgyZTMxMmUzMGlqVHJIY2pFQ2FUVDNxNmdOMXVaMWRhNnRoMjF5cCtDdkMybDBhb3ZLbkE9;MjU2MjgzQDMxMzgyZTMxMmUzMGhsMDRGQlZac3R3UVRBcFJ0MVRSTjd2bGgzdlRlSmdoVmY3UktUMndRaUE9;MjU2Mjg0QDMxMzgyZTMxMmUzMFBaaXQwT2MzT0w3UVRwampyZm1KYzllZ3lRb2tRV0VMVDh1R0JPYk40ZU09");
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            //Auth
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<AuthCredentialsKeeper>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore(options =>
            {
            });


            builder.Services.AddScoped<ResourceService>();
            builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSyncfusionBlazor();

            await builder.Build().RunAsync();
        }
    }
}
