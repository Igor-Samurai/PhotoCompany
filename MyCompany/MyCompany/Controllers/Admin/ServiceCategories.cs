using Microsoft.AspNetCore.Mvc;
using MyCompany.Domain.Entities;

namespace MyCompany.Controllers.Admin
{
    public partial class AdminController
    {
        public async Task<IActionResult> ServiceCategoriesEdit(int id)
        {
            //В зависимости от наличия ID либо добавляем, либо изменяем запись
            ServiceCategory? entity = id == default ? new ServiceCategory() : await _dataManager.ServiceCategories.GetServiceCategoryByIdAsync(id);
            return View(entity);
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCategoriesEdit(ServiceCategory entity)
        {
            //В модели присутствуют ошибки, возвращаем на доработку
            if (!ModelState.IsValid)
                return View(entity);

            await _dataManager.ServiceCategories.SaveServiceCategoryAsync(entity);
            _logger.LogInformation($"Добавлена/обновлена категория услуги с ID: {entity.Id}");

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ServiceCategoriesDelete(int id)
        {
            //Т.к. в целях безопасности отключено каскадное удаление, то прежде чем удалить услугу убедитесь,
            //что на нее нет ссылки ни у одной из услуг
            await _dataManager.ServiceCategories.DeleteServiceCategoryAsync(id);
            _logger.LogInformation($"Удалена категория услуги с ID: {id}");

            return RedirectToAction("Index");
        }
    }
}