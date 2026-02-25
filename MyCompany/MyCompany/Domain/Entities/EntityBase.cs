using System.ComponentModel.DataAnnotations;

namespace MyCompany.Domain.Entities
{
    //Базовый класс для всех сущностей в приложении, содержит общие данные для всех сущностей
    public abstract class EntityBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заполните название")]
        [Display(Name = "Название")]
        [MaxLength(200)]
        public string? Title { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
