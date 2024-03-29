﻿using Catalog.Service.EventHandlers.Commands;
using Catalog.Service.Queries;
using Catalog.Service.Queries.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Common.Collection;

namespace Catalog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        //private readonly ILogger _logger;
        private readonly IProductQueryService _productQueryService;
        private readonly IMediator _mediator;

        public ProductController(
            IProductQueryService productQueryService,
            IMediator mediator)
        {
            _productQueryService = productQueryService;
            _mediator = mediator;
        }

        //products
        [HttpGet]
        public async Task<DataCollection<ProductDto>> GetAll(int page = 1, int take =10, string ids = null)
        {
            IEnumerable<int> products = null;

            if (!string.IsNullOrEmpty(ids))
            { 
                products = ids.Split(',').Select(x => Convert.ToInt32(x));
            }


            return await _productQueryService.GetAllAsync(page, take, products);
        }

        //products/1
        [HttpGet("{id}")]
        public async Task<ProductDto> Get(int id)
        { 
            return await _productQueryService.GetAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            await _mediator.Publish(command);
            return Ok();
        }
    }
}
