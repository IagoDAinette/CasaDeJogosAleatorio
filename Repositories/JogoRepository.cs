using CasaDeJogosAleatorio.Model;
using CasaDeJogosAleatorio.Settings;
using CasaDeJogosAleatorio.DTO;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Text.RegularExpressions;

namespace CasaDeJogosAleatorio.Repositorie
{
    public class JogoRepository
    {
        private readonly IMongoCollection<Jogo> _jogos;

        // Injeção de Dependência: o IMongoClient (singleton) e as configurações (IOptions)
        // são entregues pelo container, definidos no Program.cs.
        public JogoRepository(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            var settings = options.Value;
            var database = client.GetDatabase(settings.DatabaseName);
            // Se a collection não existir, o Mongo a cria no primeiro insert.
            _jogos = database.GetCollection<Jogo>(settings.JogosCollectionName);
        }

        public async Task<List<Jogo>> ListarAsync() =>
            await _jogos.Find(_ => true).ToListAsync();

        public async Task<Jogo?> BuscarPorIdAsync(string id) =>
            await _jogos.Find(j => j.Id == id).FirstOrDefaultAsync();

        public async Task<Jogo> CriarAsync(Jogo jogo)
        {
            // O driver preenche jogo.Id com o ObjectId gerado após o insert.
            await _jogos.InsertOneAsync(jogo);
            return jogo;
        }

        public async Task<bool> AtualizarAsync(string id, Jogo jogo)
        {
            jogo.Id = id;
            var resultado = await _jogos.ReplaceOneAsync(j => j.Id == id, jogo);
            // MatchedCount > 0 indica que o documento existia (mesmo que nada tenha mudado).
            return resultado.MatchedCount > 0;
        }

        public async Task<bool> RemoverAsync(string id)
        {
            var resultado = await _jogos.DeleteOneAsync(j => j.Id == id);
            return resultado.DeletedCount > 0;
        }

        public async Task<List<Jogo>> BuscarPorPlataformaEPrecoAsync(string plataforma, decimal precoMaximo)
        {
            // Filtro composto com Builders: plataforma igual (sem diferenciar maiúsculas) E preço <= máximo.
            var builder = Builders<Jogo>.Filter;
            var filtro = builder.And(
                builder.Regex(j => j.Plataforma, new BsonRegularExpression($"^{Regex.Escape(plataforma)}$", "i")),
                builder.Lte(j => j.Preco, precoMaximo)
            );
            return await _jogos.Find(filtro).ToListAsync();
        }

        public async Task<List<RelatorioEstoqueDto>> RelatorioEstoqueAsync()
        {
            // Pipeline de agregação via LINQ: $group por plataforma.
            // Count() = quantidade de TÍTULOS (documentos), Sum(Preco * Estoque) = valor do inventário.
            return await _jogos.AsQueryable()
                .GroupBy(j => j.Plataforma)
                .Select(g => new RelatorioEstoqueDto
                {
                    Plataforma = g.Key,
                    QuantidadeTitulos = g.Count(),
                    ValorTotalInventario = g.Sum(j => j.Preco * j.Estoque)
                })
                .ToListAsync();
        }
    }
}
