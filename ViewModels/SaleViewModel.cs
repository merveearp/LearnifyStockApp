using System;

namespace LearnifyStockApp.ViewModels;

public class SaleViewModel
{
    public int Id {get;set;}
	public bool IsDeleted {get;set;}
	public DateTime UpdatedAt {get;set;}
    public string? ProductName {get;set;}
	public string? CustomerName {get;set;}
	public int Quantity {get;set;}
	public decimal UnitPrice {get;set;}
	public DateTime SaleDate {get;set;}
}


