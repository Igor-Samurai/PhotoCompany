using MyCompany.Domain.Repositories.Abstract;

namespace MyCompany.Domain
{
    //Удобный класс-менеджер для работы с бизнес-сущностями
    public class DataManager
    {
        public IServiceCategoriesRepository ServiceCategories { get; set; }
        public IServicesRepository Services { get; set; }

        public DataManager(IServiceCategoriesRepository serviceCategoriesRepository, IServicesRepository servicesRepository)
        {
            ServiceCategories = serviceCategoriesRepository;
            Services = servicesRepository;
        }
    }
}
