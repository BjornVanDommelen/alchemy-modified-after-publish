//-----------------------------------------------------------------------
// <copyright file="ExtensionException.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception picked up by the extension.
    /// </summary>
    public abstract class ExtensionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException"/> class.
        /// </summary>
        public ExtensionException() 
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception</param>
        public ExtensionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException"/> class.
        /// </summary>
        /// <param name="message">Message for the exception</param>
        /// <param name="innerException">Causing exception</param>
        public ExtensionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException"/> class.
        /// </summary>
        /// <param name="info">The parameter is not used.</param>
        /// <param name="context">The parameter is not used.</param>
        public ExtensionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionException"/> class.
        /// </summary>
        /// <param name="messageFormat">Format string</param>
        /// <param name="messageParameters">Parameters to inject into format string</param>
        public ExtensionException(string messageFormat, params object[] messageParameters)
            : base(string.Format(messageFormat, messageParameters))
        {
        }
    }
}
