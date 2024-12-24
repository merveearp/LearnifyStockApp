using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnifyStockApp.Models;
using LearnifyStockApp.Models.Repositories;

namespace LearnifyStockApp.Controllers;

public class HomeController : Controller
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ProductRepository _productRepository;

    public HomeController(CategoryRepository categoryRepository, ProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }

    public async Task<IActionResult> Index()
    {
        var categoryMax=await _categoryRepository.GetTopSellingCategoryAsync(false);
        var categoryMin=await _categoryRepository.GetTopSellingCategoryAsync(true);
        ViewBag.CategoryMaxName=categoryMax.CategoryName;
        ViewBag.CategoryMaxSales=categoryMax.TotalSales;
        
        ViewBag.CategoryMinName=categoryMin.CategoryName;
        ViewBag.CategoryMinSales=categoryMin.TotalSales;
        ViewBag.ProductsCount=await _productRepository.GetProductsCountAsync();

        return View();
    }

}
