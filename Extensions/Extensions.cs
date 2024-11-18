using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Citolab.Persistence.MongoDb;
using Citolab.Persistence.NoAction;
using Microsoft.Extensions.DependencyInjection;
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

        public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services, MongoDbDatabaseOptions settings)
        {

            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddMemoryCache();
            services.AddLogging();
            services.AddSingleton<ICollectionOptions>(settings);
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
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source object cannot be null.");
            }

            var options = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(source, options);
            return JsonSerializer.Deserialize<T>(json, options);
        }
    }
}
