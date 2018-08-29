using System;

namespace Ghost.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumStringAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The text.
        /// </summary>
        private readonly string text;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumStringAttribute"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public EnumStringAttribute(string value)
        {
            this.text = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumStringAttribute"/> class.
        /// </summary>
        /// <param name="resourceType">
        /// Type of the resource.
        /// </param>
        /// <param name="resourceName">
        /// Name of the resource.
        /// </param>
        /// <remarks>
        /// </remarks>
        public EnumStringAttribute(Type resourceType, string resourceName)
        {
            this.text = ResourceHelper.GetResourceLookup(resourceType, resourceName);
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <remarks></remarks>
        public override string ToString()
        {
            return this.text;
        }

        #endregion
    }
}
