using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IConsignerRepository : IGenericRepository<Consigner, string>
    {
        Task<Consigner> GetByUserName(string name);
        Task<Consigner> GetConsingerByMemoId(int memoId);
        Task<List<Consignee>> GetPendingConsigneesByConsignerId(string consignerId);
        Task<Consigner> GetConsginerByConsigneeId(string consigneeId);

        Task<Consigner> FindById(string id);
    }

}
