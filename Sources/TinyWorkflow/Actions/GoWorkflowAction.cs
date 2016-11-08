using System;

namespace TinyWorkflow.Actions
{
	internal class GoWorkflowAction<T> : WorkflowAction<T>
	{
		#region Public properties

		/// <summary>
		/// Action embeded in the step
		/// </summary>
		public Action<T> Action { get; private set; }

		public override WorkflowActionState State
		{
			get { return state; }
		}

		#endregion Public properties

		#region Private Variables

		private WorkflowActionState state;

		#endregion Private Variables

		#region Ctor

		public GoWorkflowAction(Action<T> action)
		{
			Action = action;
			state = WorkflowActionState.Ready;
		}

		#endregion Ctor

		#region Public methods

		public override void Reset()
		{
			state = WorkflowActionState.Ready;
		}

		/// <summary>
		/// Run the step
		/// </summary>
		/// <param name="obj"></param>
		public override void Run(T obj)
		{
			Action(obj);
			state = WorkflowActionState.Ended;
		}

		#endregion Public methods
	}
}