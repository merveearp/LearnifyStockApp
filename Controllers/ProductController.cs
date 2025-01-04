using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using LearnifyStockApp.Models.Repositories;
using LearnifyStockApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace LearnifyStockApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly SupplierRepository _supplierRepository;
        private readonly INotyfService _notyfService;


        public ProductController(ProductRepository productRepository, CategoryRepository categoryRepository, SupplierRepository supplierRepository, INotyfService notyfService)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
            _notyfService = notyfService;
        }

        public async Task<ActionResult> Index(bool id)
        {
            bool isDeleted=id;
            ViewBag.IsDeleted=isDeleted;
            var products=await _productRepository.GetAllAsync(isDeleted);
            return View(products);
        }
        [NonAction]
        private async Task<List<SelectListItem>> GenerateCategoryListAsync()
        {
            var categories=await _categoryRepository.GetAllAsync(false);
            List<SelectListItem> categoriesSelectList=[];
            foreach (var category in categories)
            {
                categoriesSelectList.Add(new SelectListItem
                {
                    Text=category.Name,
                    Value=category.Id.ToString()
                });
            }
            return categoriesSelectList;
        }
        [NonAction]
        private async Task<List<SelectListItem>> GenerateSupplierListAsync()
        {
            var suppliers=await _supplierRepository.GetAllAsync(false);
            List<SelectListItem> suppliersSelectList=[];
            foreach (var supplier in suppliers)
            {
                suppliersSelectList.Add(new SelectListItem
                {
                    Text=supplier.Name,
                    Value=supplier.Id.ToString()
                });
            }
            return suppliersSelectList;
        }

        public async Task<ActionResult> Create()
        {
            ProductCreateViewModel model=new()
            {
                CategoryList=await GenerateCategoryListAsync(),
                SupplierList=await GenerateSupplierListAsync()

            };
            return View(model);

        }
        [HttpPost]
        public async Task<ActionResult> Create(ProductCreateViewModel productCreateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _productRepository.CreateAsync(productCreateViewModel);
                _notyfService.Success("Yeni ürün eklendi");
                return RedirectToAction("Index");
            }
            productCreateViewModel.CategoryList=await GenerateCategoryListAsync();
            productCreateViewModel.SupplierList=await GenerateSupplierListAsync();
        return View(productCreateViewModel);
        }
        public async Task<ActionResult> Update(int id)
        {
            var product =await _productRepository.GetByIdAsync(id);
            ProductUpdateViewModel productUpdateViewModel =new (){

            Id=product.Id,
            Name=product.Name,
            Description=product.Description,
            StockQuantity=product.StockQuantity,
            MinimumStockLevel=product.MinimumStockLevel,
            Price=product.Price,
            UpdatedAt=product.UpdatedAt.Year==1 ? null :product.UpdatedAt,
            CategoryList=await GenerateCategoryListAsync(),
            SupplierList=await GenerateSupplierListAsync()

            };
             return View(productUpdateViewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Update(ProductUpdateViewModel productUpdateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _productRepository.UpdateAsync(productUpdateViewModel);
                _notyfService.Success("Seçmiş olduğunuz ürün güncellendi");
                return RedirectToAction("Index");
                
            }
            productUpdateViewModel.CategoryList=await GenerateCategoryListAsync();
            productUpdateViewModel.SupplierList=await GenerateSupplierListAsync();
            return View(productUpdateViewModel);
        }
        public async Task<ActionResult> SoftDelete(int id)
        {
            bool isDeleted=await _productRepository.SoftDeleteAsync(id);
            var message=isDeleted ? "Ürün başarıyla geri alındı!":  "Ürün çöp kutusuna gönderildi.";
            _notyfService.Warning(message);
            return RedirectToAction("Index",new{id=isDeleted});
        }
        public async Task<ActionResult> HardDelete(int id)
        {
            bool isDeleted = await _productRepository.HardDeleteAsync(id);
            _notyfService.Warning("Ürün kalıcı olarak silindi");
            return RedirectToAction("Index",new{id=isDeleted});
        }

    }
}
