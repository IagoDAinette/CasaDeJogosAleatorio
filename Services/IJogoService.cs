using CasaDeJogosAleatorio.DTO;

namespace CasaDeJogosAleatorio.Services
{
    public interface IJogoService 
    {
        Task<List<JogoResponse>> ListarAsync();
        Task<JogoResponse?> BuscarPorIdAsync(string id);
        Task<JogoResponse> CriarAsync(JogoRequest dto);
        Task<bool> AtualizarAsync(string id, JogoRequest dto);
        Task<bool> RemoverAsync(string id);
        Task<List<JogoResponse>> BuscarAsync(string plataforma, decimal precoMaximo);
        Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync();
    }
}
