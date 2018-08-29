using System;

namespace Ghost.Extensions.Attributes
{
    /// <summary>
    /// Defines storage type for enum translations
    /// </summary>
    public enum EnumStorageType
    {
        /// <summary>
        /// Indicates that translations for enums are stored in resc file
        /// </summary>
        Resources,
        /// <summary>
        /// Indicates that translations for enums are stored in redis cache
        /// </summary>
        Redis
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumResourceAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnumResourceAttribute"/> class.
        /// Default storage type for enum translations is Redis cache,
        /// use other constructors to define other storage types
        /// </summary>
        public EnumResourceAttribute()
        {
            this.StorageType = EnumStorageType.Redis;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumResourceAttribute"/> class.
        /// Use this constructor to define that translations for enum with this attribute are contained within resx file.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        public EnumResourceAttribute(Type resourceType)
        {
            ResourceType = resourceType;
            this.Prefix = string.Empty;
            this.StorageType = EnumStorageType.Resources;
        }

        /// <summary>
        /// Gets or sets the type of the resx file contains translations for enums.
        /// </summary>
        /// <value>
        /// The type of the resx file contains translations for enums.
        /// </value>
        public Type ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets the type of the storage for enum translations.
        /// </summary>
        /// <value>
        /// The type of the storage for enum translations.
        /// </value>
        public EnumStorageType StorageType { get; set; }
    }
}
