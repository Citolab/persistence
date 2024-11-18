using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Citolab.Persistence
{
    /// <summary>
    ///     Base class for all read models.
    /// </summary>
    public abstract class Model
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected Model()
        {
            Id = Guid.NewGuid();
        }

        [BsonId]
        public Guid Id { get; set; }

        /// <summary>
        ///     User identifier of the author of this item.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        ///     Date and time this item was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        ///     Date and time this item was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        ///     User identifier of the person that last modified this item.
        /// </summary>
        public Guid LastModifiedByUserId { get; set; }

        /// <summary>
        ///     Flag indicating if the object is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}