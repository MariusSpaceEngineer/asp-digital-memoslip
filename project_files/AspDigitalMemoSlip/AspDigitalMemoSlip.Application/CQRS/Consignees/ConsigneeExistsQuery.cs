using AspDigitalMemoSlip.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Consignees
{
    public class ConsigneeExistsQuery : IRequest<bool>
    {
        public string ConsigneeId { get; set; }

        public ConsigneeExistsQuery(string consigneeId)
        {
            ConsigneeId = consigneeId;
        }
    }
    public class ConsigneeExistsQueryHandler : IRequestHandler<ConsigneeExistsQuery, bool>
    {
        private readonly IUnitOfWork uow;

        public ConsigneeExistsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(ConsigneeExistsQuery request, CancellationToken cancellationToken)
        {
            var consignee = await Task.Run(() => uow.ConsigneeRepository.GetConsigneeByUserId(request.ConsigneeId), cancellationToken);
            return consignee != null;
        }



    }
}
