using AspDigitalMemoSlip.Application.Exceptions;
using AspDigitalMemoSlip.Application.Interfaces;
using AspDigitalMemoSlip.Domain;
using AutoMapper;
using DTOClassLibrary.DTO.Product;
using MediatR;

public class UpdateProductCommand : IRequest<List<ProductDTO>>
{
    public ProductStateChangeDTO ProductStateChangeDTO { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, List<ProductDTO>>
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;

    public UpdateProductCommandHandler(IUnitOfWork uow, IMapper mapper)
    {
        this.uow = uow;
        this.mapper = mapper;
    }

    public async Task<List<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var modifiedProducts = new List<Product>();

        foreach (var stateChange in request.ProductStateChangeDTO.ProductStateUpdates)
        {
            var product = await uow.ProductRepository.GetProductById(stateChange.ProductId);
            if (product != null)
            {
                product.State = stateChange.ProductState;
                product.Remarks = stateChange.Remark;
                modifiedProducts.Add(product);
            }
            else
            {
                throw new EntityNotFoundException("product with " + product.Id);
            }
        }

        List<ProductDTO> updatedProductDTOs = new List<ProductDTO>();
        if (modifiedProducts.Any())
        {
            await uow.ProductRepository.UpdateMultipleProducts(modifiedProducts);
            await uow.Commit();

            updatedProductDTOs = modifiedProducts.Select(product => mapper.Map<ProductDTO>(product)).ToList();
        }

        return updatedProductDTOs;
    }


}
