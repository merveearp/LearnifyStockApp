using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LearnifyStockApp.Models;
using LearnifyStockApp.Models.Repositories;
using LearnifyStockApp.ViewModels;

namespace LearnifyStockApp.Controllers;

public class HomeController : Controller
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ProductRepository _productRepository;
    private readonly CustomerRepository _customerRepository;

    public HomeController(CategoryRepository categoryRepository, ProductRepository productRepository, CustomerRepository customerRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public async Task<IActionResult> Index()
    {
        var categoryMax=await _categoryRepository.GetTopSellingCategoryAsync(false);
        var categoryMin=await _categoryRepository.GetTopSellingCategoryAsync(true);
       
        HomePageStatisticsViewModel model=new()
        {
        
        MaxCategoryName=categoryMax.CategoryName,
        MaxCategorySales=categoryMax.TotalSales,
        MinCategoryName=categoryMin.CategoryName,
        MinCategorySales=categoryMin.TotalSales,
        ProductsCount=await _productRepository.GetProductsCountAsync(),
        CustomersCount=await _customerRepository.GetCustomersCountAsync(),
        LastCustomers=await _customerRepository.GetLastCustomersAsync(5),
        LastProducts=await _productRepository.GetLastProductAsync(10)

        };

        return View(model);
    }

}
