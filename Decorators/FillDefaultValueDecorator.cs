﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Citolab.Persistence.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace Citolab.Persistence.Decorators
{
    /// <summary>
    ///     Used to fill default values like, created, createdby, modified, modifiedby, id etc..
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FillDefaultValueDecorator<T> : CollectionDecoratorBase<T> where T : Model, new()
    {
        private readonly Guid? _actorId;

        /// <inheritdoc />
        public FillDefaultValueDecorator(IMemoryCache memoryCache, ICollection<T> decoree, Guid? actorId)
            : base(memoryCache, decoree)
        {
            _actorId = actorId;
        }

        /// <inheritdoc />
        public override async Task<T> AddAsync(T document)
        {
            var userId = document.CreatedByUserId == Guid.Empty
                ? _actorId
                : document.CreatedByUserId;
            if (document.Id == default(Guid)) document.Id = Guid.NewGuid();
            if (userId.HasValue)
            {
                document.CreatedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.CreatedByUserId != default(Guid)
                        ? document.CreatedByUserId
                        : userId.Value;
                document.LastModifiedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.LastModifiedByUserId != default(Guid)
                        ? document.LastModifiedByUserId
                        : userId.Value;
            }
            var dateNow = !OverrideDefaultValues.FillDefaulValues && document.Created != default(DateTime)
                ? document.Created
                : DateTime.UtcNow;
            document.Created = dateNow;
            document.LastModified = dateNow;
            return await base.AddAsync(document);
        }

        public override async Task AddManyAsync(List<T> documents)
        {
            foreach (var document in documents)
            {
                var userId = document.CreatedByUserId == Guid.Empty
                ? _actorId
                : document.CreatedByUserId;
                if (document.Id == default(Guid)) document.Id = Guid.NewGuid();
                if (userId.HasValue)
                {
                    document.CreatedByUserId =
                        !OverrideDefaultValues.FillDefaulValues && document.CreatedByUserId != default(Guid)
                            ? document.CreatedByUserId
                            : userId.Value;
                    document.LastModifiedByUserId =
                        !OverrideDefaultValues.FillDefaulValues && document.LastModifiedByUserId != default(Guid)
                            ? document.LastModifiedByUserId
                            : userId.Value;
                }
                var dateNow = !OverrideDefaultValues.FillDefaulValues && document.Created != default(DateTime)
                    ? document.Created
                    : DateTime.UtcNow;
                document.Created = dateNow;
                document.LastModified = dateNow;
            }
            await base.AddManyAsync(documents);
        }


        /// <inheritdoc />
        public override async Task<bool> UpdateAsync(T document)
        {
            var userId = _actorId;

            if (userId.HasValue)
                document.LastModifiedByUserId =
                    !OverrideDefaultValues.FillDefaulValues && document.LastModifiedByUserId != default(Guid)
                        ? document.LastModifiedByUserId
                        : userId.Value;

            var dateNow = !OverrideDefaultValues.FillDefaulValues && document.LastModified != default(DateTime)
                ? document.LastModified
                : DateTime.UtcNow;
            document.LastModified = dateNow;
            return await base.UpdateAsync(document);
        }
    }
}