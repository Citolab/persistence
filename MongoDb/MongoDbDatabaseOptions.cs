using Citolab.Persistence.MongoDb;
using System;
using System.Collections.Generic;

namespace Citolab.Persistence.MongoDb
{
    public class MongoDbDatabaseOptions : IMongoDbDatabaseOptions
    {
        public string DatabaseName { get; }
        public string ConnectionString { get; }
        public List<Type> TypesToCache { get; }

        public MongoDbDatabaseOptions(string databaseName, string connectionString, List<Type> typesToCache)
        {
            DatabaseName = databaseName;
            ConnectionString = connectionString;
            TypesToCache = typesToCache != null ? typesToCache : new List<Type>();
        }
        public bool EnvironmentSuffix { get; set; } = true;
        public bool FlagDelete { get; set; }
        public bool TimeLoggingEnabled { get; set; }
    }
}
