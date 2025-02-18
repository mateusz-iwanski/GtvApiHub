using Google.Apis.Auth.OAuth2;
using GtvApiHub.Exceptions;
using GtvApiHub.WebApi.DTOs;
using GtvApiHub.WebApi.EndPointInterfaces;
using GtvApiHub.WebApi.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GtvApiHub.WebApi.Services
{
    public class GtvApiConnectionValidator
    {
        private readonly IAlternativeItemEndPoint _services;
        private readonly IGtvToken _token;

        public GtvApiConnectionValidator(IGtvApiConfigurationServices services, IGtvToken token)
        {
            _token = token;
            _services = services.AlternativeItemService;

        }

        public async Task<bool> GtvApiTestconnectionAsync()
        {
            try
            {
               
                await _token.GetTokenAsync();

                var response = await _services.GetAsync();

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ApiConnectionException("GTV api return status 'Not Found' when try to get all alternative items in the connection validator");
                }
                if (!response.IsSuccessStatusCode)
                    throw new ApiConnectionException($"GTV api return status code {response.StatusCode} when try to get all alternative items in the connection validator");

                var listCategoryTreeDto = await response.Content.GetListObjectAsync<CategoryTreeDto>();

                if (listCategoryTreeDto == null)
                    throw new ApiConnectionException("GTV api return null when try to get all alternative items in the connection validator");

            }
            catch (Exception ex)
            {
                throw new ApiConnectionException("Can't connect to the GTV API", ex);
            }

            return true;
        }
    }
}
