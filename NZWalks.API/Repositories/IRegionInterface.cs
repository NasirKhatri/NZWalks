using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IRegionInterface
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetById(Guid id);
        Task<Region> Create(Region region);
        Task<Region?> Update(Guid id, Region region);
        Task<Region?> Delete(Guid id);
        
    }
}
