using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IConsigneeRepository : IGenericRepository<Consignee, string>
    {
        Task<Consignee> GetConsigneeByMemoId(int id);
        Task<Consignee> GetConsigneeByUserId(string userId);
        Task<bool> UpdateConsigneeAcceptedByConsignerToTrue(string userId);
        Task<bool> DeleteConsignee(string userId);


    }
}
