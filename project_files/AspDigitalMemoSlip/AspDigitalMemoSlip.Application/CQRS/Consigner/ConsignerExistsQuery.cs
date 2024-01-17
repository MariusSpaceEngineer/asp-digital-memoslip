using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspDigitalMemoSlip.Application.Interfaces;
using MediatR;

namespace AspDigitalMemoSlip.Application.CQRS.Consigner
{
    public class ConsignerExistsQuery : IRequest<bool>
    {
        public string ConsignerId { get; set; }

        public ConsignerExistsQuery(string consignerId)
        {
            ConsignerId = consignerId;
        }
    }
    public class ConsignerExistsQueryHandler : IRequestHandler<ConsignerExistsQuery, bool>
    {
        private readonly IUnitOfWork uow;

        public ConsignerExistsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(ConsignerExistsQuery request, CancellationToken cancellationToken)
        {
            var consigner = await Task.Run(() => uow.ConsignerRepository.FindById(request.ConsignerId), cancellationToken);
            return consigner != null;
        }



    }

}
