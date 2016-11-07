using System;

namespace TinyWorkflow.Actions
{
	internal class WhileWorkflowAction<T> : WorkflowAction<T>
	{
		#region Public properties

		/// <summary>
		/// Action embeded in the step
		/// </summary>
		public IWorkflow<T> Workflow { get; private set; }

		/// <summary>
		/// Condition for running the step
		/// </summary>
		public Func<T, bool> ConditionToRun { get; private set; }

		/// <summary>
		/// Condition result, once evaluated.
		/// </summary>
		public bool? CachedConditionToRun { get; private set; }

		public override WorkflowActionState State
		{
			get
			{
				if (!CachedConditionToRun.HasValue)
				{
					return WorkflowActionState.Ready;
				}
				else if (CachedConditionToRun == true)
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
				else
				{
					return WorkflowActionState.Ended;
				}
			}
		}

		#endregion Public properties

		#region Private Variables

		private T _State;

		#endregion Private Variables

		#region Ctor

		public WhileWorkflowAction(Func<T, bool> conditionToRun, Action<T> action)
			: this(conditionToRun, new Workflow<T>().Do(action))
		{
		}

		public WhileWorkflowAction(Func<T, bool> conditionToRun, IWorkflow<T> workflow)
		{
			Workflow = workflow;
			ConditionToRun = conditionToRun;
		}

		#endregion Ctor

		#region Public methods

		private bool EvaluateCondition(T obj)
		{
			CachedConditionToRun = ConditionToRun(obj);
			return CachedConditionToRun.Value;
		}

		public override void Reset()
		{
			if (Workflow != null)
			{
				Workflow.Reset();
			}
			CachedConditionToRun = null;
		}

		public override void Run(T obj)
		{
			_State = obj;
			if (Workflow != null)
			{
				if (EvaluateCondition(obj))
				{
					Workflow.Start(obj);

					if (Workflow.State == WorkflowState.End && EvaluateCondition(obj))
					{
						Workflow.Reset();
					}
				}
			}
		}

		public override void Unblock(int unblockLevel)
		{
			var wf = Workflow as Workflow<T>;

			if (wf != null)
			{
				wf.UnblockInternal(unblockLevel);

				//if (Workflow.State == WorkflowState.End && EvaluateCondition(_State))
				//{
				//    Workflow.Reset();
				//}
			}
		}

		#endregion Public methods
	}
}