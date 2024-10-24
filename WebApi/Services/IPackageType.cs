using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IPackageType
    {
        Task<IEnumerable<PackageTypeDto>> GetAsync();
        Task<IEnumerable<PackageTypeDto>> GetAsync(string itemCode);
    }
}