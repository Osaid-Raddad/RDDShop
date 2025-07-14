using RDDShop.Models;

namespace RDDShop.DTO.Request

{
    public class CategoryRequestDTO
    {
        public Status Status { get; set; } = Status.Active;
        public List<CategoryTranslationRequest> CategoryTranslations { get; set; } 

    }
}
