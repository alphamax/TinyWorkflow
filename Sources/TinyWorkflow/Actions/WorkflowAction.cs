using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Actions
{
    /// <summary>
    /// Workflow action that is representing a workflow step.
    /// </summary>
    /// <typeparam name="T">Type of the state linked to the workflow.</typeparam>
    internal abstract class WorkflowAction<T>
    {
        #region Public properties

        /// <summary>
        /// State of the workflow.
        /// </summary>
        public abstract WorkflowActionState State { get; } 
        #endregion

        #region Public methods

        /// <summary>
        /// Resolve the dynamic part of the step.
        /// </summary>
        /// <returns></returns>
        public virtual void Resolve(T obj)
        {

        }

        /// <summary>
        /// Reset the action to it's initial state.
        /// </summary>
        /// <returns></returns>
        public virtual void Reset()
        {

        }

        /// <summary>
        /// Run the step
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Run(T obj)
        {

        }

        /// <summary>
        /// Unblock the step.
        /// </summary>
        /// <param name="unblockLevel"></param>
        public virtual void Unblock(int unblockLevel)
        {

        }

        #endregion
    }
}
