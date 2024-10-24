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
    public class CategoryTree : ICategoryTree
    {
        private readonly ICategoryTreeEndPoint _services;

        public CategoryTree(IApiConfigurationServices services)
        {
            _services = services.CategoryTreeService;
        }

        public async Task<IEnumerable<CategoryTreeDto>> GetAsync()
        {
            var response = await _services.GetAsync();

            var listCategoryTreeDto = await response.Content.GetListObjectAsync<CategoryTreeDto>();

            return listCategoryTreeDto;
        }


        public async Task<IEnumerable<CategoryTreeDto>> GetAsync(string itemCode)
        {
            var response = await _services.GetByItemCodeAsync(itemCode);

            var listCategoryTreeDto = await response.Content.GetListObjectAsync<CategoryTreeDto>();

            return listCategoryTreeDto;
        }

    }
}
