using System;
using System.Threading.Tasks;
using MediatR;
using AspDigitalMemoSlip.Application.Interfaces;
using AutoMapper;
using AspDigitalMemoSlip.Application.CQRS.MemoSlips;

namespace AspDigitalMemoSlip.Application.CQRS.Products
{
    public class DeleteProductCommand : IRequest
    {
        public int ProductId { get; }

        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public DeleteProductCommandHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }


        async Task IRequestHandler<DeleteProductCommand>.Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await uow.MemoRepository.GetById(request.ProductId);
            uow.MemoRepository.Delete(product);
            await uow.Commit();
        }


    }
}
