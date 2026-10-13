using CasaDeJogosAleatorio.DTO;
using CasaDeJogosAleatorio.Model;
using CasaDeJogosAleatorio.Repositories;

namespace CasaDeJogosAleatorio.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _repository;

        // Injeção de Dependência: recebe a INTERFACE, não a classe concreta.
        public JogoService(IJogoRepository repository) => _repository = repository;

        public async Task<List<JogoResponse>> ListarAsync() =>
            (await _repository.ListarAsync()).Select(ParaResponse).ToList();

        public async Task<JogoResponse?> BuscarPorIdAsync(string id)
        {
            var jogo = await _repository.BuscarPorIdAsync(id);
            return jogo is null ? null : ParaResponse(jogo);
        }

        public async Task<JogoResponse> CriarAsync(JogoRequest dto)
        {
            var criado = await _repository.CriarAsync(ParaEntidade(dto));
            return ParaResponse(criado);
        }

        public async Task<bool> AtualizarAsync(string id, JogoRequest dto) =>
            await _repository.AtualizarAsync(id, ParaEntidade(dto));

        public async Task<bool> RemoverAsync(string id) =>
            await _repository.RemoverAsync(id);

        public async Task<List<JogoResponse>> BuscarAsync(string plataforma, decimal precoMaximo) =>
            (await _repository.BuscarPorPlataformaEPrecoAsync(plataforma, precoMaximo)).Select(ParaResponse).ToList();

        public async Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync() =>
            await _repository.RelatorioEstoqueAsync();

        private static Jogo ParaEntidade(JogoRequest dto) => new()
        {
            Titulo = dto.Titulo.Trim(),
            Plataforma = dto.Plataforma.Trim(),
            Genero = dto.Genero.Trim(),
            Preco = dto.Preco,
            AnoLancamento = dto.AnoLancamento,
            Estoque = dto.Estoque
        };

        private static JogoResponse ParaResponse(Jogo j) => new()
        {
            Id = j.Id ?? string.Empty,
            Titulo = j.Titulo,
            Plataforma = j.Plataforma,
            Genero = j.Genero,
            Preco = j.Preco,
            AnoLancamento = j.AnoLancamento,
            Estoque = j.Estoque
        };
    }
}
