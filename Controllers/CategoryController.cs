using AspNetCoreHero.ToastNotification.Abstractions;
using LearnifyStockApp.Models.Repositories;
using LearnifyStockApp.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LearnifyStockApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly INotyfService _notyfService;

        public CategoryController(CategoryRepository categoryRepository, INotyfService notyfService)
        {
            _categoryRepository = categoryRepository;
            _notyfService = notyfService;
        }
        //true=silinmiş kayıtlar false=silinmemişkayıtlar
        public async Task<ActionResult> Index(bool id)
        {
            bool isDeleted=id;
            ViewBag.IsDeleted=isDeleted;
            var categories =await _categoryRepository.GetAllAsync(isDeleted);
            return View(categories);
        }
        
        public ActionResult Create()
        {
          return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(CategoryCreateViewModel categoryCreateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _categoryRepository.CreateAsync(categoryCreateViewModel);
                _notyfService.Success("Kategori başarıyla eklendi");
                return RedirectToAction("Index");
            }
            return View(categoryCreateViewModel);
        }
        public async Task<ActionResult> Update(int id)
        {
            var category =await _categoryRepository.GetByIdAsync(id);
            var categoryUpdateViewModel=new CategoryUpdateViewModel
            {
                Id=category.Id,
                Name=category.Name,
                Description=category.Description,
                UpdatedAt=category.UpdatedAt.Year == 1 ? null : category.UpdatedAt
            };
            return View(categoryUpdateViewModel);

        }
        [HttpPost]
        public async Task<ActionResult> Update(CategoryUpdateViewModel categoryUpdateViewModel)
        {
            if(ModelState.IsValid)
            {
                await _categoryRepository.UpdateAsync(categoryUpdateViewModel);
                _notyfService.Success("Kategori başarıyla güncellendi");
                return RedirectToAction("Index");
                
            }
            return View(categoryUpdateViewModel);
        }
        public async Task<ActionResult> SoftDelete(int id)
        {
            var isDeleted=await _categoryRepository.SoftDeleteAsync(id);
            var message=isDeleted ? "Kategori başarıyla geri alındı!":  "Kategori çöp kutusuna gönderildi.";
            _notyfService.Warning(message);
            return RedirectToAction("Index",new{id=isDeleted});
        }
        public async Task<ActionResult> HardDelete(int id)
        {
            var isDeleted = await _categoryRepository.HardDeleteAsync(id);
            _notyfService.Warning("Kategori kalıcı olarak silindi");
            return RedirectToAction("Index");
        }



    }
}
