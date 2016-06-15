using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Actions
{
    /// <summary>
    /// State of an action.
    /// </summary>
    internal enum WorkflowActionState
    {
        /// <summary>
        /// Ready to be run.
        /// </summary>
        Ready,
        /// <summary>
        /// Blocked. Waiting for an unblock command.
        /// </summary>
        Blocked,
        /// <summary>
        /// Has been run and is finised.
        /// </summary>
        Ended,
    }
}
