using System;

namespace LearnifyStockApp.ViewModels;

public class CustomerViewModel
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? Name { get; set; }
    public string? ContactName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

}
