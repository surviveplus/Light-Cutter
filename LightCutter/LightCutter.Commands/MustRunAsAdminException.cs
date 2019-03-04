using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands
{
    /// <summary>
    /// The exception that is thrown when app must run as admin.
    /// </summary>
    /// <remarks>
    /// <para>change log</para>
    /// </remarks>
    [Serializable]
    public class MustRunAsAdminException : Exception
    {
        // Override or Implement Interface

        #region Exception Members

        /// <summary>
        /// Sets the SerializationInfo object with the parameter name and additional exception information.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info object is a null reference (Nothing in Visual Basic).</exception>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);

            // TODO: Save private members in info object. For example to save Value property, write the code as follows.
            // info.AddValue("Value", this.Value);
        } // end sub

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MustRunAsAdminException class.
        /// </summary>
        public MustRunAsAdminException() { }

        /// <summary>
        /// Initializes a new instance of the MustRunAsAdminException class with a specified error message. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MustRunAsAdminException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the MustRunAsAdminException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. Set a null reference (Nothing in Visual Basic) if the inner exception value was not supplied.</param>
        public MustRunAsAdminException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the MustRunAsAdminException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected MustRunAsAdminException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {

            // TODO: Initialize private members by info object. For example to initialize Value property, write the code as follows.
            // this.Value = info.GetInt32("Value");

        } // end constructor

        #endregion

    } // end class
}
