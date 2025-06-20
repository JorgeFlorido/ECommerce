using AutoMapper;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Product;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Unit>
  {
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public UpdateProductHandler(IProductRepository productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
      _ = await _productRepository.GetProductByIdAsync(command.ProductId, cancellationToken)
        ?? throw new KeyNotFoundException($"Product with ID {command.ProductId} not found.");

      var updatedProduct = _mapper.Map<Product>(command);
      await _productRepository.UpdateProductAsync(updatedProduct, cancellationToken);
      return Unit.Value;
    }
  }
}
