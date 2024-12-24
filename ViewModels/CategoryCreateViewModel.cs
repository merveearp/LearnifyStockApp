using System;
using System.ComponentModel.DataAnnotations;

namespace LearnifyStockApp.ViewModels;

public class CategoryCreateViewModel
{
	[Display(Name ="Kategori")]
	[Required(ErrorMessage ="Bu alan zorunlu alandır")]
	public string? Name {get;set;}

	[Display(Name="Kategori Açıklaması")]
	[Required(ErrorMessage ="Açıklama boş bırakılamaz")]
	public string? Description {get;set;}
    


}
