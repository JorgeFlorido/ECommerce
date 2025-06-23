using ECommerce.Application.Interfaces;
using ECommerce.Application.Requests.Queries.Orders;
using ECommerce.Application.Services;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Models;
using ECommerce.Domain.Models.Order;
using FluentAssertions;
using NSubstitute;

namespace ECommerce.Application.Tests.Services
{
  [TestFixture]
  public class CheckoutProcessorTests
  {
    private ITaxService _taxService;
    private IShippingService _shippingService;
    private IDiscountService _discountService;
    private IInventoryService _inventoryService;
    private CheckoutProcessor _processor;

    [SetUp]
    public void SetUp()
    {
      _taxService = Substitute.For<ITaxService>();
      _shippingService = Substitute.For<IShippingService>();
      _discountService = Substitute.For<IDiscountService>();
      _inventoryService = Substitute.For<IInventoryService>();
      _processor = new CheckoutProcessor(_taxService, _discountService, _shippingService, _inventoryService);
    }

    [Test]
    public async Task GivenValidOrderWithDiscount_WhenCalculatingOrderCost_ThenShouldCalculateAllComponents()
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 50m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT10"
      };

      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m); // 10% tax
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("DISCOUNT10", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT10", Amount = 10m });

      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);

      // Assert
      result.GrossAmount.Should().Be(100m);
      result.TaxAmount.Should().Be(10m);   
      result.ShippingCost.Should().Be(15m);
      result.DiscountCode.Should().NotBeNull();
      result.DiscountCode.Amount.Should().Be(10m);
    }

    [Test]
    public async Task GivenOrderWithoutDiscountCode_WhenCalculatingOrderCost_ThenShouldNotApplyDiscount()
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = string.Empty
      };

      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m);
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync(string.Empty, Arg.Any<CancellationToken>()).Returns((DiscountCode)null);

      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);

      // Assert
      result.GrossAmount.Should().Be(100m);
      result.TaxAmount.Should().Be(10m);
      result.ShippingCost.Should().Be(15m);
      result.DiscountCode.Should().BeNull();
    }

    [Test]
    public async Task GivenInvalidDiscountCode_WhenCalculatingOrderCost_ThenShouldNotApplyDiscount()
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "INVALID_CODE"
      };
      
      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m);
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("INVALID_CODE", Arg.Any<CancellationToken>()).Returns((DiscountCode)null);
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(100m);
      result.TaxAmount.Should().Be(10m);
      result.ShippingCost.Should().Be(15m);
      result.DiscountCode.Should().BeNull();
    }

    [Test]
    public async Task GivenEmptyOrderItems_WhenCalculatingOrderCost_ThenShouldReturnZeroAmounts()
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items = [],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT10"
      };

      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m);
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("DISCOUNT10", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT10", Amount = 10m });
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(0m);
      result.TaxAmount.Should().Be(0m);
      result.ShippingCost.Should().Be(15m);
    }

    [Test]
    public async Task GivenMultipleItems_WhenCalculatingOrderCost_ThenShouldSumAmountsCorrectly() 
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 50m },
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 30m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT10"
      };

      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m); // 10% tax
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("DISCOUNT10", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT10", Amount = 10m });
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(110m); // (50 + 60) - 10 (discount)
      result.TaxAmount.Should().Be(11m); // 10% of 110
      result.ShippingCost.Should().Be(15m);
    }

    [Test]
    public async Task GivenZeroTaxRate_WhenCalculatingOrderCost_ThenShouldNotApplyTax() 
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT10"
      };
      
      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0m); // Zero tax rate
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("DISCOUNT10", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT10", Amount = 10m });
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(100m);
      result.TaxAmount.Should().Be(0m);
      result.ShippingCost.Should().Be(15m);
    }

    [Test]
    public async Task GivenZeroShippingCost_WhenCalculatingOrderCost_ThenShouldNotIncludeShipping() 
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 100m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT10"
      };
      
      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m);
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(0m); // Zero shipping cost
      _discountService.GetDiscountCodeAsync("DISCOUNT10", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT10", Amount = 10m });
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(100m); // Gross should not include shipping
      result.TaxAmount.Should().Be(10m); // 10% of 100
      result.ShippingCost.Should().Be(0m);
    }

    [Test]
    public async Task GivenDiscountExceedingGrossAmount_WhenCalculatingOrderCost_ThenShouldNotAllowNegativeTotal() 
    {
      // Arrange
      var request = new OrderCostCalculationQuery
      {
        CustomerId = Guid.NewGuid(),
        Items =
                [
                    new OrderItem { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 50m }
                ],
        ShippingAddress = new OrderShippingAddress(),
        BillingAddress = new OrderBillingAddress
        {
          CustomerAddress = new CustomerAddress { Country = Country.ES }
        },
        DiscountCode = "DISCOUNT100" // Discount greater than gross amount
      };
      
      _taxService.GetTaxRateAsync(Arg.Any<Country>(), Arg.Any<CancellationToken>()).Returns(0.1m);
      _shippingService.CalculateShippingCostAsync(Arg.Any<OrderShippingAddress>(), Arg.Any<CancellationToken>()).Returns(15m);
      _discountService.GetDiscountCodeAsync("DISCOUNT100", Arg.Any<CancellationToken>()).Returns(new DiscountCode { Code = "DISCOUNT100", Amount = 100m });
      
      // Act
      var result = await _processor.CalculateOrderCostAsync(request, CancellationToken.None);
      
      // Assert
      result.GrossAmount.Should().Be(50m); // Gross should not be negative
      result.TaxAmount.Should().Be(5m); // 10% of 50
      result.ShippingCost.Should().Be(15m);
    }

    [Test]
    public void GivenTaxServiceError_WhenCalculatingOrderCost_ThenShouldThrowException() 
    { 

    }
  }
}