using AspDigitalMemoSlip.Domain;
using DTOClassLibrary.DTO;

namespace AspDigitalMemoSlip.Application.Interfaces
{
    public interface ISalesConfirmationRepository
    {
        Task<SalesConfirmationDTO> Create(string ConsignerId,string ConsigneeId, SalesConfirmationDTO SalesConfirmation);
        Task<IEnumerable<SalesConfirmation>> GetAllSalesConfirmationsForUser(string ConsigneeId);
        Task<SalesConfirmation> GetSalesConfirmationById(int salesConfirmationId);
        Task<SalesConfirmation> Update(SalesConfirmation Modified);
    }
}