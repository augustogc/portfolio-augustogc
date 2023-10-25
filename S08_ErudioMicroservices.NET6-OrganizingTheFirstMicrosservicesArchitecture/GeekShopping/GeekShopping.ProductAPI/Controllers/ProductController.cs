﻿using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVO>>> FindAll() 
        {
            var products = await _repository.FindAll();
            return Ok(products);
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductVO>> FindById(long id)
        {
            var product = await _repository.FindById(id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpPost("id")]
        public async Task<ActionResult<ProductVO>> Create(ProductVO vo)
        {
            

            if (vo == null)
            {
                return BadRequest();
            }
            else
            {
                var product = await _repository.Create(vo);

                return Ok(product);
            }
        }

        [HttpPut("id")]
        public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
        {


            if (vo == null)
            {
                return BadRequest();
            }
            else
            {
                var product = await _repository.Update(vo);

                return Ok(product);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            var status = await _repository.Delete(id);

            if (status == false)
            {
                return BadRequest();
            }
            else 
            {
                return Ok(status);
            }    
        }
    }
}