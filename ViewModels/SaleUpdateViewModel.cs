using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearnifyStockApp.ViewModels;

public class SaleUpdateViewModel
{
    public int Id {get;set;}
    public int ProductId {get;set;}

    [Display(Name ="Ürün")]
    public List<SelectListItem>? ProductList { get; set; }
	public int CustomerId {get;set;}
   
    [Display(Name ="Müşteri")]
    public List<SelectListItem>? CustomerList { get; set; }

    [Display(Name ="Satış Miktarı")]
    [Required(ErrorMessage ="Satış miktarı hakkında bilgilendirme yapınız.")]
	public int Quantity {get;set;}

    [Display(Name ="Birim Fiyatı")]
    [Required(ErrorMessage ="Birim fiyat hakkında bilgilendirme yapınız.")]
	public decimal UnitPrice {get;set;}

    [Display(Name ="Güncelleme Tarihi")]
	public DateTime? UpdatedAt {get;set;}

    [Display(Name ="Satış Tarihi")]
	public DateTime SaleDate {get;set;}
}
