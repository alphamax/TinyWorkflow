using System;
using System.Collections.Generic;

namespace TinyWorkflow.Actions
{
	internal class IfWorkflowAction<T> : WorkflowAction<T>
	{
		#region Public properties

		public override WorkflowActionState State
		{
			get
			{
				var tempWF = EvaluatedConditionOnResolve == true ? WorkflowIfTrue : null;
				tempWF = EvaluatedConditionOnResolve == false ? WorkflowIfFalse : tempWF;

				if (tempWF != null)
				{
					if (tempWF.State == WorkflowState.Blocked)
					{
						return WorkflowActionState.Blocked;
					}

					if (tempWF.State == WorkflowState.End)
					{
						return WorkflowActionState.Ended;
					}
				}

				return WorkflowActionState.Ready;
			}
		}

		/// <summary>
		/// Action embeded in the step if True
		/// </summary>
		public IWorkflow<T> WorkflowIfTrue { get; private set; }

		/// <summary>
		/// Action embeded in the step if False
		/// </summary>
		public IWorkflow<T> WorkflowIfFalse { get; private set; }

		/// <summary>
		/// Condition for steps
		/// </summary>
		public Func<T, bool> Condition { get; private set; }

		/// <summary>
		/// Dynamic query that is used in cas of 'for'. Must be evaluated just before being run.
		/// </summary>
		public Func<IEnumerable<WorkflowAction<T>>> DynamicQuery { get; private set; }

		#endregion Public properties

		#region Private Variables

		private bool? EvaluatedConditionOnResolve { get; set; }

		#endregion Private Variables

		#region Ctor

		public IfWorkflowAction(Func<T, bool> condition, Action<T> actionIfTrue, Action<T> actionIfFalse)
			: this(condition, new Workflow<T>().Do(actionIfTrue), new Workflow<T>().Do(actionIfFalse))
		{
		}

		public IfWorkflowAction(Func<T, bool> condition, IWorkflow<T> actionIfTrue, IWorkflow<T> actionIfFalse)
		{
			Condition = condition;
			WorkflowIfTrue = actionIfTrue;
			WorkflowIfFalse = actionIfFalse;
		}

		#endregion Ctor

		#region Public methods

		public override void Reset()
		{
			if (WorkflowIfTrue != null)
			{
				WorkflowIfTrue.Reset();
			}
			if (WorkflowIfFalse != null)
			{
				WorkflowIfFalse.Reset();
			}
		}

		public override void Resolve(T obj)
		{
			EvaluatedConditionOnResolve = Condition(obj);
		}

		public override void Run(T obj)
		{
			if (EvaluatedConditionOnResolve == true)
			{
				if (WorkflowIfTrue != null)
				{
					WorkflowIfTrue.Start(obj);
				}
			}
			else if (EvaluatedConditionOnResolve == false)
			{
				if (WorkflowIfFalse != null)
				{
					WorkflowIfFalse.Start(obj);
				}
			}
			else
			{
				throw new Exception("If part is not resolved");
			}
		}

		public override void Unblock(int unblockLevel)
		{
			if (EvaluatedConditionOnResolve == true)
			{
				var wfTrue = WorkflowIfTrue as Workflow<T>;
				if (wfTrue != null)
				{
					wfTrue.UnblockInternal(unblockLevel);
				}
			}
			else if (EvaluatedConditionOnResolve == false)
			{
				var wfFalse = WorkflowIfFalse as Workflow<T>;
				if (wfFalse != null)
				{
					wfFalse.UnblockInternal(unblockLevel);
				}
			}
		}

		#endregion Public methods
	}
}