using AspNetCoreHero.ToastNotification.Abstractions;
using LearnifyStockApp.Models.Entities;
using LearnifyStockApp.Models.Repositories;
using LearnifyStockApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearnifyStockApp.Controllers
{
    public class SaleController : Controller
    {
        private readonly SaleRepository _saleRepository;
        private readonly ProductRepository _productRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly INotyfService _notyfService;

        public SaleController(SaleRepository saleRepository, ProductRepository productRepository, CustomerRepository customerRepository, INotyfService notyfService)
        {
            _saleRepository = saleRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _notyfService = notyfService;
        }

        public async Task<ActionResult> Index(bool id)
        {
            bool isDeleted=id;
            ViewBag.IsDeleted=isDeleted;
            var sales=await _saleRepository.GetAllAsync(isDeleted);
            return View(sales);
        }
        [NonAction]
        private async Task<List<SelectListItem>> GenerateProductListAsync()
        {
            var products=await _productRepository.GetAllAsync(false);
            List<SelectListItem> productsSelectList=[];
            foreach (var product in products)
            {
                productsSelectList.Add(new SelectListItem
                {
                    Text=product.Name,
                    Value=product.Id.ToString()
                });
            }
            return productsSelectList;
        }
        [NonAction]
        private async Task<List<SelectListItem>> GenerateCustomerListAsync()
        {
            var customers=await _customerRepository.GetAllAsync(false);
            List<SelectListItem> customersSelectList=[];
            foreach (var customer in customers)
            {
                customersSelectList.Add(new SelectListItem
                {
                    Text=customer.Name,
                    Value=customer.Id.ToString()
                });
            }
            return customersSelectList;
        }
        
        public async Task<ActionResult> Create()
        {
            SaleCreateViewModel model=new()
            {
                ProductList=await GenerateProductListAsync(),
                CustomerList=await GenerateCustomerListAsync()

            };
            return View(model);

        }
        [HttpPost]
        public async Task<ActionResult> Create(SaleCreateViewModel saleCreateViewModel)
        {
            if(ModelState.IsValid)
            {
                var result=await _saleRepository.CreateAsync(saleCreateViewModel);
                var message=result==true ? "Yeni satış kaydı eklendi":"Stok miktarı yetersiz olduğu için satış yapılamadı.";
                _notyfService.Success(message);
                return RedirectToAction("Index");
            }
            saleCreateViewModel.ProductList=await GenerateProductListAsync();
            saleCreateViewModel.CustomerList=await GenerateCustomerListAsync();
        return View(saleCreateViewModel);
        }
        public async Task<ActionResult> Update(int id)
        {
            var sale =await _saleRepository.GetByIdAsync(id);
            SaleUpdateViewModel saleUpdateViewModel =new (){

            Id=sale.Id,
            Quantity=sale.Quantity,
            UnitPrice=sale.UnitPrice,
            SaleDate=sale.SaleDate,
            UpdatedAt=sale.UpdatedAt.Year==1 ? null :sale.UpdatedAt,
            ProductList=await GenerateProductListAsync(),
            CustomerList=await GenerateCustomerListAsync()

            };
             return View(saleUpdateViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(SaleUpdateViewModel saleUpdateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _saleRepository.UpdateAsync(saleUpdateViewModel);
                _notyfService.Success("Seçmiş olduğunuz Satış Kaydı güncellendi");
                return RedirectToAction("Index");
                
            }
            saleUpdateViewModel.ProductList=await GenerateProductListAsync();
            saleUpdateViewModel.CustomerList=await GenerateCustomerListAsync();
            return View(saleUpdateViewModel);
        }
        public async Task<ActionResult> SoftDelete(int id)
        {
            bool isDeleted=await _saleRepository.SoftDeleteAsync(id);
            var message=isDeleted ? "Satış kaydı başarıyla geri alındı!":  "Satış kaydı çöp kutusuna gönderildi.";
            _notyfService.Warning(message);
            return RedirectToAction("Index",new{id=isDeleted});
        }
        public async Task<ActionResult> HardDelete(int id)
        {
            bool isDeleted = await _saleRepository.HardDeleteAsync(id);
            _notyfService.Warning("Satış Kaydı kalıcı olarak silindi");
            return RedirectToAction("Index",new{id=isDeleted});
        }




    }
}
