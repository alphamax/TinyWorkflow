using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyWorkflow.Actions
{
	internal class ForWorkflowAction<U, T> : WorkflowAction<T>
	{
		#region Public properties

		public override WorkflowActionState State
		{
			get
			{
				if (Workflow.State == WorkflowState.End)
				{
					return WorkflowActionState.Ended;
				}
				if (Workflow.State == WorkflowState.Blocked)
				{
					return WorkflowActionState.Blocked;
				}
				return WorkflowActionState.Ready;
			}
		}

		public Func<T, IEnumerable<U>> Enumarator { get; private set; }

		public IWorkflow<Tuple<U, T>> Workflow { get; private set; }

		#endregion Public properties

		#region Private Variables

		private List<U> ResolvedEnumerator;

		private int indexOfRun;

		private T LastState;

		#endregion Private Variables

		#region Ctor

		public ForWorkflowAction(Func<T, IEnumerable<U>> enumerator, IWorkflow<Tuple<U, T>> workflow)
		{
			Workflow = workflow;
			Enumarator = enumerator;
			indexOfRun = 0;
		}

		#endregion Ctor

		#region Public methods

		public override void Reset()
		{
			indexOfRun = 0;
			if (Workflow != null)
			{
				Workflow.Reset();
			}
		}

		/// <summary>
		/// Resolve the dynamic part of the step.
		/// </summary>
		/// <returns></returns>
		public override void Resolve(T obj)
		{
			LastState = obj;
			ResolvedEnumerator = Enumarator(obj).ToList();
		}

		/// <summary>
		/// Run the step
		/// </summary>
		/// <param name="obj"></param>
		public override void Run(T obj)
		{
			LastState = obj;

			if (indexOfRun >= ResolvedEnumerator.Count)
			{
				//Workflow part is done before starting.
				//No items to iterate.
				Workflow.End();
			}
			else
			{
				do
				{
					Workflow.Start(new Tuple<U, T>(ResolvedEnumerator[indexOfRun], obj));
					switch (Workflow.State)
					{
						case WorkflowState.End:
							indexOfRun++;
							if (indexOfRun < ResolvedEnumerator.Count)
							{
								Workflow.Reset();
							}
							break;

						case WorkflowState.Blocked:
							return;
					}
				}
				while (indexOfRun < ResolvedEnumerator.Count);
			}
		}

		public override void Unblock(int unblockLevel)
		{
			var wf = Workflow as Workflow<T>;
			wf?.UnblockInternal(unblockLevel);
			//Run(LastState);
			//if (indexOfRun >= ResolvedEnumerator.Count)
			//{
			//    Workflow.Reset();
			//}
		}

		#endregion Public methods
	}
}