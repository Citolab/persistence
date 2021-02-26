using System;
using System.Collections.Generic;

namespace Citolab.Persistence.MongoDb
{
    public interface IMongoDbDatabaseOptions: ICollectionOptions
    {
        string ConnectionString { get; }
        string DatabaseName { get; }
        List<Type> TypesToCache { get; }
        bool EnvironmentSuffix { get; set; }
    }
}