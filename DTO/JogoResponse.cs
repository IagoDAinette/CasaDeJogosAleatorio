namespace CasaDeJogosAleatorio.DTO
{
    public class JogoResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Plataforma { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public int AnoLancamento { get; set; }
        public int Estoque { get; set; }
    }
}
