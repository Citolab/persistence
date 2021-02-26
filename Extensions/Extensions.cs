using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Citolab.Persistence.Helpers;
using Citolab.Persistence.MongoDb;
using Citolab.Persistence.NoAction;
using Microsoft.Extensions.DependencyInjection;
using ObjectCloner.Extensions;

namespace Citolab.Persistence.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddInMemoryPersistence(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton<ICollectionOptions>(new InMemoryDatabaseOptions());
            services.AddScoped<IUnitOfWork, NoActionUnitOfWork>();
            return services;
        }

        public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services, string databaseName, string connectionString, List<Type> typesToCache = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton<ICollectionOptions>(new MongoDbDatabaseOptions(databaseName, connectionString, typesToCache));
            services.AddScoped<IUnitOfWork, MongoDbUnitOfWork>();
            return services;
        }

        /// <summary>
        ///     Deep clone using JSON serialization.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toClone"></param>
        /// <returns></returns>
        public static T Clone<T>(this T source)
        {
            return source.DeepClone();
        }
    }
}
