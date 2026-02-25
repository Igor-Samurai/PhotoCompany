using MyCompany.Domain.Entities;
using MyCompany.Models;

namespace MyCompany.Infrastructure
{
    public static class HelperDTO
    {
        //Service => ServiceDTO
        public static ServiceDTO TransformService(Service entity)
        {
            ServiceDTO entityDTO = new ServiceDTO();
            entityDTO.Id = entity.Id;
            entityDTO.CategoryName = entity.ServiceCategory?.Title;
            entityDTO.Title = entity.Title;
            entityDTO.DescriptionShort = entity.DescriptionShort;
            entityDTO.Description = entity.Description;
            entityDTO.PhotoFileName = entity.Photo;
            entityDTO.Type = entity.Type.ToString();

            return entityDTO;
        }

        public static IEnumerable<ServiceDTO> TransformServices(IEnumerable<Service> entities)
        {
            List<ServiceDTO> entitiesDTO = new List<ServiceDTO>();

            //В цикле каждую доменную модель из коллекции превращаем в DTO
            foreach (Service entity in entities)
                entitiesDTO.Add(TransformService(entity));

            return entitiesDTO;
        }
    }
}