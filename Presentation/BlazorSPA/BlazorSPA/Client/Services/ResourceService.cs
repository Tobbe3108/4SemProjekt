using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorSPA.Client.Data;
using Microsoft.Extensions.Configuration;
using NodaTime;

namespace BlazorSPA.Client.Services
{
    public class ResourceService
    {
        private readonly AuthCredentialsKeeper _authCredentialsKeeper;
        private readonly HttpClient _client;
        private readonly string _mobileBffUrl;

        public ResourceService(AuthCredentialsKeeper authCredentialsKeeper, HttpClient client, IConfiguration configuration)
        {
            _authCredentialsKeeper = authCredentialsKeeper;
            _client = client;
            _mobileBffUrl = configuration.GetValue<string>("WebBff:BaseUrl");
        }

        public async Task<(Guid, string)> Add(ResourceViewModel resource)
        {
            if (resource.Available.Count > 0)
            {
                if (resource.Available.Any(r => r.From == r.To))
                {
                    return (Guid.Empty, "Start and end time cannot be the same time");
                }
                if (resource.Available.Any(dayAndTime => resource.HasOverlapping(dayAndTime)))
                {
                    return (Guid.Empty, "Overlap in available time");
                }
            }
            
            HttpResponseMessage result;
            try
            {
                result = await _client.PostAsJsonAsync($"{_mobileBffUrl}/Resource", resource);
            }
            catch (Exception e)
            {
                return (Guid.Empty, e.Message);
            }

            return result.IsSuccessStatusCode ? (await result.Content.ReadFromJsonAsync<Guid>(), null) : (Guid.Empty, "Failed to create resource");
        }

        public async Task<List<ResourceViewModel>> GetAll()
        {
            HttpResponseMessage result;
            try
            {
                result = await _client.GetAsync($"{_mobileBffUrl}/Resource");
            }
            catch (Exception)
            {
                return null;
            }
            
        
            if (!result.IsSuccessStatusCode) return null;

            var resources = await result.Content.ReadFromJsonAsync<List<ResourceViewModel>>();
            return resources ?? null;
        }

        public async Task<ResourceViewModel> GetById(Guid id)
        {
            HttpResponseMessage result;
            try
            {
                result = await _client.GetAsync($"{_mobileBffUrl}/Resource/{id}");
            }
            catch (Exception)
            {
                return null;
            }
            
        
            if (!result.IsSuccessStatusCode) return null;

            ResourceViewModel resource = null;
            try
            {
                resource = await result.Content.ReadFromJsonAsync<ResourceViewModel>();
            }
            catch (Exception e)
            {
                Console.WriteLine("Begin Error");
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.WriteLine("End error");
            }
            return resource ?? null;
        }

        public async Task<(bool, string)> Update(ResourceViewModel resource)
        {
            HttpResponseMessage result;
            try
            {
                result = await _client.PutAsJsonAsync($"{_mobileBffUrl}/Resource/{resource.Id}", resource);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            
            await Task.Delay(500);
            return result.IsSuccessStatusCode ? (true, null) : (false, "Failed to update resource");
        }

        public async Task<(bool, string)> Delete(Guid id)
        {
            HttpResponseMessage result;
            try
            {
                result = await _client.DeleteAsync($"{_mobileBffUrl}/Resource/{id}");
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            
            return result.IsSuccessStatusCode ? (true, null) : (false, "Failed to delete resource");
        }
    }
}