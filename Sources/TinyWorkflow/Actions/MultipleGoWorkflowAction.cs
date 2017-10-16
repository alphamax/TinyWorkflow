using System;
using System.Threading.Tasks;

namespace TinyWorkflow.Actions
{
	internal class MultipleGoWorkflowAction<T> : WorkflowAction<T>
	{
	    private readonly bool m_waitActionsFinish;

	    #region Public properties

		/// <summary>
		/// Action embeded in the step
		/// </summary>
		public Action<T>[] Actions { get; private set; }

		public override WorkflowActionState State
		{
			get { return state; }
		}

		#endregion Public properties

		#region Private Variables

		private WorkflowActionState state;

		#endregion Private Variables

		#region Ctor

		public MultipleGoWorkflowAction(Action<T>[] actions, bool waitActionsFinish)
		{
		    m_waitActionsFinish = waitActionsFinish;
		    Actions = actions;
			state = WorkflowActionState.Ready;
		}

		#endregion Ctor

		#region Public methods

		public override void Reset()
		{
			state = WorkflowActionState.Ready;
		}

		public override void Run(T obj)
		{
		    Task task = Task.Run(() => Parallel.ForEach(Actions, (action) => action(obj)));
		    if (m_waitActionsFinish)
		        task.Wait();
			state = WorkflowActionState.Ended;
		}

		#endregion Public methods
	}
}