//-----------------------------------------------------------------------
// <copyright file="ItemNotFoundException.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an issue with a tridion item not found via core service.
    /// </summary>
    public class ItemNotFoundException : ExtensionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        public ItemNotFoundException() 
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="message">TCMURI for the item</param>
        public ItemNotFoundException(string message)
            : this("The item with TCMURI '{0}' was not found", message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception</param>
        /// <param name="innerException">Causing exception</param>
        public ItemNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="info">The parameter is not used.</param>
        /// <param name="context">The parameter is not used.</param>
        public ItemNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemNotFoundException"/> class.
        /// </summary>
        /// <param name="messageFormat">Format string</param>
        /// <param name="messageParameters">Parameters to inject into format string</param>
        public ItemNotFoundException(string messageFormat, params object[] messageParameters)
            : base(string.Format(messageFormat, messageParameters))
        {
        }
    }
}
