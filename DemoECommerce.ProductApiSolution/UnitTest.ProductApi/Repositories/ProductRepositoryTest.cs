using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System.Linq.Expressions;

namespace UnitTest.ProductApi.Repositories
{
    public class ProductRepositoryTest
    {
        private readonly ProductDbContext productDbContext;
        private readonly ProductRepository productRepository;

        public ProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            productDbContext = new ProductDbContext(options);
            productRepository = new ProductRepository(productDbContext);
        }

        // Create Product
        [Fact]
        public async Task CreateAsync_WhenProductAlreadyExist_ReturnErrorResponse()
        {
            // Arrange
            var existingProduct = new Product { Name = "Existing Product" };
            productDbContext.Products.Add(existingProduct);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.CreateAsync(existingProduct);

            // Assert 
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be($"{existingProduct.Name} already added.");
        }

        [Fact]
        public async Task CreateAsync_WhenProductDoesNotExist_AddProductAndReturnsSuccessResponse()
        {
            // Arrange
            var existingProduct = new Product { Name = "Existing Product" };

            // Act
            var result = await productRepository.CreateAsync(existingProduct);

            // Assert
            result.Should().NotBeNull();
            result!.Flag.Should().BeTrue();
            result!.Message.Should().Be($"{existingProduct.Name} added successfully.");
        }

        // DELETE PRODUCT
        [Fact]
        public async Task DeleteAsync_WhenProductIsFound_ReturnsSuccessResponse()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Non-existing Product", Price = 78.67m, Quantity = 5 };
            productDbContext.Products.Add(product);

            // Act
            var result = await productRepository.DeleteAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be($"{product.Name} deleted successfully.");
        }

        [Fact]
        public async Task DeleteAsync_WhenProductIsNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Non-existing Product", Price = 78.67m, Quantity = 5 };

            // Act
            var result = await productRepository.DeleteAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product not found.");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductExists_ReturnsProduct()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Test Product", Price = 50.00m, Quantity = 10 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.FindByIdAsync(product.Id);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task FindByIdAsync_WhenProductDoesNotExist_ReturnsNull()
        {
            // Act
            var result = await productRepository.FindByIdAsync(999);
            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_WhenProductsExist_ReturnsAllProducts()
        {
            // Arrange
            var products = new List<Product>()
            {
                new(){ Id = 1, Name = "Product 1", Quantity = 10, Price = 100.70m },
                new(){ Id = 2, Name = "Product 2", Quantity = 110, Price = 1000.70m }
            };
            productDbContext.Products.AddRange(products);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
            result.Should().Contain(p => p.Name == "Product 1");
            result.Should().Contain(p => p.Name == "Product 2");
        }

        [Fact]
        public async Task GetAllAsync_WhenNoProductsExist_ReturnsNull()
        {
            // Act
            var result = await productRepository.GetAllAsync();
            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsFound_ReturnProduct()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Test Product", Price = 50.00m, Quantity = 10 };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();
            Expression<Func<Product, bool>> predicate = p => p.Name == "Test Product";

            // Act
            var result = await productRepository.GetByAsync(predicate);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("Test Product");
        }

        [Fact]
        public async Task GetByAsync_WhenProductIsNotFound_ReturnNull()
        {
            // Arrange
            Expression<Func<Product, bool>> predicate = p => p.Name == "Test Product";

            // Act
            var result = await productRepository.GetByAsync(predicate);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_WhenProductIsUpdatedSuccessfully_ReturnSuccessResponse()
        {
            // Arrange
            var product = new Product() { Id = 1, Name = "Product 1" };
            productDbContext.Products.Add(product);
            await productDbContext.SaveChangesAsync();

            // Act
            var result = await productRepository.UpdateAsync(product);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeTrue();
            result.Message.Should().Be("Product updated successfully.");
        }

        [Fact]
        public async Task UpdateAsync_WhenProductIsNotFound_ReturnErrorResponse()
        {
            // Arrange
            var updatedProduct = new Product() { Id = 1, Name = "Product 22" };

            // Act
            var result = await productRepository.UpdateAsync(updatedProduct);

            // Assert
            result.Should().NotBeNull();
            result.Flag.Should().BeFalse();
            result.Message.Should().Be("Product not found.");
        }
    }
}
