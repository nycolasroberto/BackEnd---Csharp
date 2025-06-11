using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCatalog.Models
{
    // Representa um jogo no catálogo
    public class Game
    {

        // Identificador único do jogo
        public int Id { get; set; }

        // Nome do jogo 
        [Required(ErrorMessage = "O nome do jogo é obrigatório")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres")]
        public string Nome { get; set; } = string.Empty;

        // Descrição detalhada do jogo
        [StringLength(1000, ErrorMessage = "A descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        // Data de lançamento do jogo 
        [Required(ErrorMessage = "A data de lançamento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataLancamento { get; set; }

        // Preço do jogo 
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0, 999.99, ErrorMessage = "O preço deve estar entre 0 e 999,99")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        // Nome do desenvolvedor do jogo 
        [Required(ErrorMessage = "O desenvolvedor é obrigatório")]
        [StringLength(150, ErrorMessage = "O nome do desenvolvedor deve ter no máximo 150 caracteres")]
        public string Desenvolvedor { get; set; } = string.Empty;

        // ID da categoria ao qual o jogo pertence 
        // Chave estrangeira para Category
        [Required(ErrorMessage = "A categoria é obrigatória")]
        public int CategoriaId { get; set; }

        // Categoria à qual o jogo pertence
        // Relacionamento N:1 
        public virtual Category? Categoria { get; set; }
    }
}
