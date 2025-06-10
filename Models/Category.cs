using System.ComponentModel.DataAnnotations;

namespace GameCatalog.Models
{
 
    public class Category
    {
        
        public int Id { get; set; }

       
        [Required(ErrorMessage = "O nome da categoria é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

   
        public virtual ICollection<Game> Games { get; set; } = new List<Game>();
    }
}