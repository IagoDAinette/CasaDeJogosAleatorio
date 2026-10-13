using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CasaDeJogosAleatorio.Model
{
    public class Jogo
    {
        // [BsonId] marca a chave primária (_id) do Mongo.
        // [BsonRepresentation(ObjectId)] guarda como ObjectId no banco, mas expõe como string no C#.
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("titulo")]
        public string Titulo { get; set; } = string.Empty;

        [BsonElement("plataforma")]
        public string Plataforma { get; set; } = string.Empty;

        [BsonElement("genero")]
        public string Genero { get; set; } = string.Empty;

        // Decimal128 evita perda de precisão (o padrão do driver para decimal é string).
        [BsonElement("preco")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Preco { get; set; }

        [BsonElement("anoLancamento")]
        public int AnoLancamento { get; set; }

        [BsonElement("estoque")]
        public int Estoque { get; set; }
    }
}
