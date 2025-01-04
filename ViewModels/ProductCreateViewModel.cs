using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearnifyStockApp.ViewModels;

public class ProductCreateViewModel
{
    [Display(Name ="Ürün ")]
    [Required(ErrorMessage ="Ürün adı boş bırakılamaz")]
	public string? Name {get;set;}

    [Display(Name ="Ürün Açıklaması ")]
    [Required(ErrorMessage ="Açıklama boş bırakılamaz")]
	public string? Description {get;set;}
	public int CategoryId {get;set;}

    [Display(Name ="Kategori ")]
    public List<SelectListItem>? CategoryList { get; set; }
	public int SupplierId {get;set;}

    [Display(Name ="Tedarikçi ")]
    public List<SelectListItem>? SupplierList { get; set; }

    [Display(Name ="Stok Miktarı")]
    [Required(ErrorMessage ="Stok Miktarı boş bırakılamaz")]
	public int? StockQuantity {get;set;}

    [Display(Name ="Minimum Stok Seviyesi")]
    [Required(ErrorMessage ="Minimum Stok Seviyesi boş bırakılamaz")]
	public int? MinimumStockLevel {get;set;}

    [Display(Name ="Fiyat")]
    [Required(ErrorMessage ="Fiyat bilgisi boş bırakılamaz")]
	public decimal? Price {get;set;}

}
