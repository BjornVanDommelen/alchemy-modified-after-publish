//-----------------------------------------------------------------------
// <copyright file="ImpersonationException.cs" company="Tahzoo">
//     Copyright (c) Tahzoo. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Tahzoo.Tridion.Extensions.CheckIfModifiedAfterPublish
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an issue with core service impersonation.
    /// </summary>
    public class ImpersonationException : ExtensionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationException"/> class.
        /// </summary>
        public ImpersonationException() 
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationException"/> class.
        /// </summary>
        /// <param name="message">Username that the system attempted to impersonate</param>
        public ImpersonationException(string message)
            : this("Failed to impersonate user '{0}'", message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationException"/> class.
        /// </summary>
        /// <param name="message">Username that the system attempted to impersonate</param>
        /// <param name="innerException">Causing exception</param>
        public ImpersonationException(string message, Exception innerException)
            : base(string.Format("Failed to impersonate user '{0}'", message), innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationException"/> class.
        /// </summary>
        /// <param name="info">The parameter is not used.</param>
        /// <param name="context">The parameter is not used.</param>
        public ImpersonationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImpersonationException"/> class.
        /// </summary>
        /// <param name="messageFormat">Format string</param>
        /// <param name="messageParameters">Parameters to inject into format string</param>
        public ImpersonationException(string messageFormat, params object[] messageParameters)
            : base(string.Format(messageFormat, messageParameters))
        {
        }
    }
}
