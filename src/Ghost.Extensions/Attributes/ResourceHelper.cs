using System;
using System.Reflection;

namespace Ghost.Extensions.Attributes
{
    /// <summary>
    /// The resource helper.
    /// </summary>
    public class ResourceHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the resource lookup.
        /// </summary>
        /// <param name="resourceType">
        /// Type of the resource.
        /// </param>
        /// <param name="resourceName">
        /// Name of the resource.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <remarks>
        /// </remarks>
        public static string GetResourceLookup(Type resourceType, string resourceName)
        {
            if ((resourceType == null) || (resourceName == null))
            {
                return null;
            }

            var property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
            if (property == null)
            {
                throw new InvalidOperationException(string.Format("Type Does Not Have Property"));
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(string.Format("Resource Property is Not String Type"));
            }

            return (string)property.GetValue(null, null);
        }

        #endregion
    }
}