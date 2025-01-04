using System;
using System.ComponentModel.DataAnnotations;

namespace LearnifyStockApp.ViewModels;

public class CustomerUpdateViewModel
{
    public int Id { get; set; }

    [Display(Name = "Müşteri Adı")]
    [Required(ErrorMessage = "Müşteri adı boş bırakılamaz!")]
    public string? Name { get; set; }

    [Display(Name = "İletişim Kişisi")]
    [Required(ErrorMessage = "İletişim kişisi bilgisi boş bırakılamaz!")]
    public string? ContactName { get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email boş bırakılamaz")]
    public string? Email { get; set; }

    [Display(Name = "Telefon Numarası")]
    [Required(ErrorMessage = "Telefon Numarası boş bırakılamaz")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Adres")]
    [Required(ErrorMessage = "Adres boş bırakılamaz")]
    public string? Address { get; set; }

    [Display(Name = "Şehir")]
    [Required(ErrorMessage = "Şehir boş bırakılamaz")]
    public string? City { get; set; }

    [Display(Name = "Ülke")]
    [Required(ErrorMessage = "Ülke boş bırakılamaz")]
    public string? Country { get; set; }

    public DateTime? UpdatedAt { get; set; }

}
