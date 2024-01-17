using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using DTOClassLibrary.DTO.Consignee;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspDigitalMemoSlip.Application.CQRS.Consignees
{
    public class GetConsigneeByUserIdQuery : IRequest<ConsigneeDTO>
    {
        public GetConsigneeByUserIdQuery(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; set; }
    }

    public class GetConsigneeByUserIdQueryHandler : IRequestHandler<GetConsigneeByUserIdQuery, ConsigneeDTO>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetConsigneeByUserIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<ConsigneeDTO> Handle(GetConsigneeByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var consignee = mapper.Map<ConsigneeDTO>(await uow.ConsigneeRepository.GetConsigneeByUserId(request.UserId));

                return consignee;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }

}
