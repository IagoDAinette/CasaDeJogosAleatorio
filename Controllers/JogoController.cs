using CasaDeJogosAleatorio.DTO;
using CasaDeJogosAleatorio.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace CasaDeJogosAleatorio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly IJogoService _service;

        public JogoController(IJogoService service) => _service = service;

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] JogoRequest dto)
        {
            var criado = await _service.CriarAsync(dto);
            // 201 Created + header Location apontando para o GET por id.
            return CreatedAtAction(nameof(BuscarPorId), new { id = criado.Id }, criado);
        }

        [HttpGet]
        public async Task<IActionResult> Listar() =>
            Ok(await _service.ListarAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { mensagem = "Id inválido. Informe um ObjectId com 24 caracteres hexadecimais." });

            var jogo = await _service.BuscarPorIdAsync(id);
            return jogo is null
                ? NotFound(new { mensagem = $"Jogo com id '{id}' não encontrado." })
                : Ok(jogo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] JogoRequest dto)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { mensagem = "Id inválido. Informe um ObjectId com 24 caracteres hexadecimais." });

            var atualizado = await _service.AtualizarAsync(id, dto);
            if (!atualizado)
                return NotFound(new { mensagem = $"Jogo com id '{id}' não encontrado." });

            return Ok(await _service.BuscarPorIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest(new { mensagem = "Id inválido. Informe um ObjectId com 24 caracteres hexadecimais." });

            var removido = await _service.RemoverAsync(id);
            return removido
                ? NoContent()
                : NotFound(new { mensagem = $"Jogo com id '{id}' não encontrado." });
        }

        // Rotas literais ("busca", "relatorio-estoque") têm prioridade sobre "{id}" no ASP.NET Core.
        [HttpGet("busca")]
        public async Task<IActionResult> Buscar([FromQuery] string plataforma, [FromQuery] decimal precoMaximo)
        {
            if (string.IsNullOrWhiteSpace(plataforma))
                return BadRequest(new { mensagem = "O parâmetro 'plataforma' é obrigatório." });
            if (precoMaximo < 0)
                return BadRequest(new { mensagem = "O parâmetro 'precoMaximo' não pode ser negativo." });

            return Ok(await _service.BuscarAsync(plataforma, precoMaximo));
        }

        [HttpGet("relatorio-estoque")]
        public async Task<IActionResult> RelatorioEstoque() =>
            Ok(await _service.RelatorioEstoqueAsync());
    }
}
