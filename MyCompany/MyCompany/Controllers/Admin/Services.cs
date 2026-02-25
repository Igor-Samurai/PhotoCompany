using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain.Entities;

namespace MyCompany.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> ServicesEdit(int id)
        {
            //В зависимости от наличия ID либо добавляем, либо изменяем запись
            Service? entity = id == default ? new Service() : await _dataManager.Services.GetServiceByIdAsync(id);
            ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> ServicesEdit(Service entity, IFormFile? titleImageFile)
        {
            //В модели присутствуют ошибки, возвращаем на доработку
            if (!ModelState.IsValid)
            {
                ViewBag.ServiceCategories = await _dataManager.ServiceCategories.GetServiceCategoriesAsync();
                return View(entity);
            }

            if (titleImageFile != null)
            {
                entity.Photo = titleImageFile.FileName;
                await SaveImg(titleImageFile);
            }

            await _dataManager.Services.SaveServiceAsync(entity);
            _logger.LogInformation($"Добавлена/обновлена услуга с ID: {entity.Id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ServicesDelete(int id)
        {
            await _dataManager.Services.DeleteServiceAsync(id);
            _logger.LogInformation($"Удалена услуга с ID: {id}");

            return RedirectToAction("Index");
        }
    }
}