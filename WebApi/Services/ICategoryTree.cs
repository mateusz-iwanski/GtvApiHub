﻿using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface ICategoryTree
    {
        Task<IEnumerable<CategoryTreeDto>> GetAsync();
        Task<IEnumerable<CategoryTreeDto>> GetAsync(string itemCode);
    }
}