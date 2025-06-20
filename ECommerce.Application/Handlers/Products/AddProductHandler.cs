using AutoMapper;
using ECommerce.Application.Requests.Commands.Products;
using ECommerce.Domain.Abstractions;
using ECommerce.Domain.Models.Product;
using MediatR;

namespace ECommerce.Application.Handlers.Products
{
  public class AddProductHandler : IRequestHandler<AddProductCommand, Guid>
  {
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public AddProductHandler(IProductRepository productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<Guid> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
      var product = _mapper.Map<Product>(request);
      await _productRepository.AddProductAsync(product, cancellationToken);
      return product.Id;
    }
  }
}
