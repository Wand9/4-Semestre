using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo1.Domains
{
    public class Client
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("cpf")]
        public string? Cpf { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set;}

        [BsonElement("adress")]
        public string? Adress { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }
        public Client()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
