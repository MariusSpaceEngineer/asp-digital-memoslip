using AspDigitalMemoSlip.Application.CQRS.MemoSlips;
using AspDigitalMemoSlip.Domain;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface IMemoRepository : IGenericRepository<Memo, int>
    {
        Task<Memo> Create(Memo memo);
        Task<List<Memo>> GetMemosByConsigneeId(string consigneeId);
        Task<IEnumerable<Memo>> GetMemosWithProductsByConsginee(string consgineeId);
    }
}
