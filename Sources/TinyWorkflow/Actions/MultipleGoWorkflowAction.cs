using System;
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

		#endregion Public properties

		#region Private Variables

		private WorkflowActionState state;

		#endregion Private Variables

		#region Ctor

		public MultipleGoWorkflowAction(Action<T>[] actions)
		{
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
			foreach (var item in Actions)
			{
				Task.Factory.StartNew(() => item(obj));
			}
			state = WorkflowActionState.Ended;
		}

		#endregion Public methods
	}
}