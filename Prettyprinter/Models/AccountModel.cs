﻿using System;
using System.Xml;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using Formatting = Newtonsoft.Json.Formatting;

namespace DBLayer
{    
    [BsonIgnoreExtraElements]
    public class AccountModel
    {
        /*
         * { "_id" : ObjectId("5a67ddb3e2d82c35250dbe50"),
         * "Name" : "Test",
         * "Title" : "Tester",
         * "Email" : "test@email.com",
         * "Password" : "12345",
         * "Birthday" : ISODate("2017-12-31T16:00:00Z"),
         * "AdminRole" : true,
         * "DisplayPicURL" : "",
         * "Bio" : "BIODATA" }
         */
        [BsonId]
        [BsonElement(elementName:"_id")]
        public ObjectId ID { get; set; }
        [BsonElement(elementName:"Name")]
        public string Name { get; set; }
        [BsonElement(elementName:"Title")]
        public string Title { get; set; }
        [BsonElement(elementName:"Email")]
        public string Email { get; set; }
        [BsonElement(elementName:"Password")]
        public string Password { get; set; }
        [BsonElement(elementName:"Birthday")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Birthday { get; set; }
        [BsonElement(elementName:"AdminRole")]
        [BsonRepresentation(BsonType.Boolean)]
        public Boolean AdminRole { get; set; }
        [BsonElement(elementName:"DisplayPicURL")]
        public string DisplayPicURL { get; set; }
        [BsonElement(elementName:"Bio")]
        public string Bio { get; set; }
        
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
        
        internal AccountModel(ObjectId id, string name, string title, string email, string password, DateTime birthday, bool adminRole, string displayPicUrl, string bio)
        {
            ID = id;
            Name = name;
            Title = title;
            Email = email;
            Password = password;
            Birthday = birthday;
            AdminRole = adminRole;
            DisplayPicURL = displayPicUrl;
            Bio = bio;
        }
    }
}