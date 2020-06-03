﻿using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;

namespace API.Controllers
{
    // First new "class" created in this lecture series, : derives from ControllerBase 

    // This is called an "attribute"
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IGenericRepository<Product> _productsRepo;

        private readonly IGenericRepository<ProductBrand> _productBrandRepo;

        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        // constructor
        public ProductsController(IGenericRepository<Product> productsRepo,
        IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productsRepo = productsRepo;
        }

        // below are endpoints

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            //return "this will be a list of products, plural";

            //var products = await _context.Products.ToListAsync();
            var spec = new ProductsWithTypesAndBrandsSpecification();

            var products = await _productsRepo.ListAsync(spec);

            //return Ok(products);
            // code below converts an "Entity" to a "DTO"
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        // specify id as route parameter to differentiate in httpget ""{id}""
        // "{id}" essentially if there's a number in the url its understood to get this method
        // rather than the one above

        // "dotnet watch run" autosaves and updates as you go, seems messy and risky
        [HttpGet("{id}")]
        //public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            // this should return a single product? user selects a 
            // product from a list of products

            //return await _context.Products.FindAsync(id);
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            //return await _productsRepo.GetEntityWithSpec(spec);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            //// code below converts an "Entity" to a "DTO"
            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            // can't directly return an IReadOnlyList in asp.netcore
            // wrap in ok response
            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            // can't directly return an IReadOnlyList in asp.netcore
            // wrap in ok response
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}
