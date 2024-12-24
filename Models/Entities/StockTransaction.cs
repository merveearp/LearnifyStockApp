using System;

namespace LearnifyStockApp.Models.Entities;

public class StockTransaction
{
    public int Id {get;set;}
	public bool IsDeleted {get;set;}
	public DateTime CreatedAt {get;set;}
	public DateTime UpdatedAt {get;set;}
	public int ProductId {get;set;}
	public int TransactionTypeId {get;set;}
	public DateTime TransactionDate {get;set;}
	public int Quantity {get;set;}
	public string? Note {get;set;}

}
