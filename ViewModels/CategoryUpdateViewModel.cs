using System;
using System.ComponentModel.DataAnnotations;

namespace LearnifyStockApp.ViewModels;

public class CategoryUpdateViewModel
{
    public int Id {get;set;}

	[Display(Name ="Kategori")]
	[Required(ErrorMessage ="Bu alan zorunlu alandır")]
	public string? Name {get;set;}	
	
	[Display(Name="Kategori Açıklaması")]
	[Required(ErrorMessage ="Açıklama boş bırakılamaz")]
	public string? Description {get;set;}
	public DateTime? UpdatedAt {get;set;}

}
