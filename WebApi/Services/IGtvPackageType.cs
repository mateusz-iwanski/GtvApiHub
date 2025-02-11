using GtvApiHub.WebApi.DTOs;

namespace GtvApiHub.WebApi.Services
{
    public interface IGtvPackageType
    {
        Task<IEnumerable<PackageTypeDto>> GetAsync();
        Task<IEnumerable<PackageTypeDto>> GetAsync(string itemCode);
    }
}