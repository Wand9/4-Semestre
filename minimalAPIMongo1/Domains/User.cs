﻿using MongoDB.Bson.Serialization.Attributes;

namespace minimalAPIMongo1.Domains
{
    public class User
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("name")]
        public string? Name { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        public Dictionary<string, string> AdditionalAttributes { get; set; }

        public User()
        {
            AdditionalAttributes = new Dictionary<string, string>();
        }
    }
}
