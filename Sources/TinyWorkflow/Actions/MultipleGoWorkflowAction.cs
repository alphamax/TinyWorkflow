using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Actions
{
    internal class MultipleGoWorkflowAction<T> : WorkflowAction<T>
    {
        #region Public properties
        /// <summary>
        /// Action embeded in the step
        /// </summary>
        public Action<T>[] Actions { get; private set; }

        public override WorkflowActionState State
        {
            get { return state; }
        }

        #endregion

        #region Private Variables

        private WorkflowActionState state;

        #endregion

        #region Ctor

        public MultipleGoWorkflowAction(Action<T>[] actions)
        {
            Actions = actions;
            state = WorkflowActionState.Ready;
        }

        #endregion

        #region Public methods

        public override void Reset()
        {
            state = WorkflowActionState.Ready;
        }

        public override void Run(T obj)
        {
            foreach (var item in Actions)
            {
                Task.Factory.StartNew(() => item(obj));
            }
            state = WorkflowActionState.Ended;
        }

        #endregion
    }
}
