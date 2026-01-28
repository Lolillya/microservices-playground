using ECommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.DTOs.Conversions;
using ProductApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IProduct productInterface) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProduct()
        {
            // Get all products from repo
            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return NotFound("No Product found");

            // convert data from entity to dto
            var (_, list) = ProductConversion.FromEntity(null!, products);
            return list!.Any() ? Ok(list) : NotFound("No Product found");
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            // get single product from repo
            var product = await productInterface.FindByIdAsync(id);
            if (product is null)
                return NotFound($"No Product found with Id: {id}");

            // conver from entity to dto
            var (_product, _) = ProductConversion.FromEntity(product, null!);
            return _product is not null ? Ok(_product) : NotFound($"No Product found with Id: {id}");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> CreateProduct(ProductDTO product)
        {
            // check model state is all data annotations are valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.CreateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> UpdateProduct(ProductDTO product)
        {
            // check model state is all data annotations are valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.UpdateAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Response>> DeleteProduct(ProductDTO product)
        {
            // convert to entity
            var getEntity = ProductConversion.ToEntity(product);
            var response = await productInterface.DeleteAsync(getEntity);
            return response.Flag is true ? Ok(response) : BadRequest(response);
        }
    }

}