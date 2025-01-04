using System;

namespace LearnifyStockApp.ViewModels;

public class HomePageStatisticsViewModel
{
    public string? MaxCategoryName { get; set; }
    public string? MinCategoryName { get; set; }
    public decimal MaxCategorySales { get; set; }
    public decimal MinCategorySales { get; set; }
    public int ProductsCount { get; set; }
    public int CustomersCount { get; set; }
    public IEnumerable<CustomerViewModel> LastCustomers{ get; set; }
    public IEnumerable<ProductViewModel> LastProducts{ get; set; }



}
