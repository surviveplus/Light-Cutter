using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Surviveplus.LightCutter.Commands.Targeting
{

    [Serializable]
    public class TargetNotSelectedException : Exception
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
        /// Initializes a new instance of the TargetNotSelectedException class.
        /// </summary>
        public TargetNotSelectedException() : base("Target not selected. Insert a command to select a screen capture or image before other commands.") { }

        /// <summary>
        /// Initializes a new instance of the TargetNotSelectedException class with a specified error message. 
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public TargetNotSelectedException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the TargetNotSelectedException class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception. Set a null reference (Nothing in Visual Basic) if the inner exception value was not supplied.</param>
        public TargetNotSelectedException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the TargetNotSelectedException class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected TargetNotSelectedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) {

            // TODO: Initialize private members by info object. For example to initialize Value property, write the code as follows.
            // this.Value = info.GetInt32("Value");
        }
        #endregion

    } // end class
} // end namespace
