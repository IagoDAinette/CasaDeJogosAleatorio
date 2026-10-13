using System.ComponentModel.DataAnnotations;

namespace CasaDeJogosAleatorio.DTO
{
    public class JogoRequest
    {
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "O título deve ter entre 1 e 150 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "A plataforma é obrigatória.")]
        public string Plataforma { get; set; } = string.Empty;

        [Required(ErrorMessage = "O gênero é obrigatório.")]
        public string Genero { get; set; } = string.Empty;

        [Range(0, 100000, ErrorMessage = "O preço deve ser maior ou igual a zero.")]
        public decimal Preco { get; set; }

        [Range(1970, 2100, ErrorMessage = "Ano de lançamento inválido.")]
        public int AnoLancamento { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
        public int Estoque { get; set; }
    }
}
