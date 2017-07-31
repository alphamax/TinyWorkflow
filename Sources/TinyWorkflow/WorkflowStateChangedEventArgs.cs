using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow
{
    public class WorkflowStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// New workflow state.
        /// </summary>
        public WorkflowState State { get; private set; }

        public WorkflowStateChangedEventArgs(WorkflowState state)
        {
            State = state;
        }
    }
}
