using CasaDeJogosAleatorio.DTO;
using CasaDeJogosAleatorio.Model;

namespace CasaDeJogosAleatorio.Repositories
{
    public interface IJogoRepository
    {
        Task<List<Jogo>> ListarAsync();
        Task<Jogo?> BuscarPorIdAsync(string id);
        Task<Jogo> CriarAsync(Jogo jogo);
        Task<bool> AtualizarAsync(string id, Jogo jogo);
        Task<bool> RemoverAsync(string id);
        Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo);
        Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync();
    }
}
