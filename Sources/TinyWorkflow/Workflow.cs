using System;
using System.Collections.Generic;
using TinyWorkflow.Actions;

namespace TinyWorkflow
{
	/// <summary>
	/// Represent a workflow.
	/// Can be configured in fluent mode.
	/// </summary>
	/// <typeparam name="T">Type of the state</typeparam>
	public class Workflow<T> : IWorkflow<T>
	{
		#region Public

		/// <summary>
		/// State embeded in the actual Workflow
		/// </summary>
		public T Workload { get; set; }

		/// <summary>
		/// State of the workflow.
		/// </summary>
		public WorkflowState State
		{
			get
			{
				return _State;
			}
		    private set
		    {
		        if (value != _State)
		        {
		            _State = value;
                    RaiseWorkflowStateChangedEvent(value);
		        }
		    }
		}

        #endregion Public

        #region Private

        private readonly List<WorkflowAction<T>> _Actions;
        private WorkflowState _State;
        private int _WorkflowStep;
	    
	    #endregion Private

		public Workflow()
		{
			_Actions = new List<WorkflowAction<T>>();
			State = WorkflowState.NotRunning;
			_WorkflowStep = 0;
		}

		/// <summary>
		/// Start a workflow with the given workload
		/// </summary>
		/// <param name="workload"></param>
		public void Start(T workload)
		{
			Workload = workload;
			if (State == WorkflowState.NotRunning)
			{
				State = WorkflowState.Running;
				Run();
			}
			else if (State == WorkflowState.Running)
			{
				Run();
			}
		}

        /// <summary>
        /// Starts a workflow and resets it when it reaches <see cref="WorkflowState.End"/>.
        /// </summary>
        /// <param name="workload"></param>
	    public void StartAndReset(T workload)
        {
            //prevent multiple event subscriptions
            WorkflowStateChanged -= OnWorkflowEnded;
            WorkflowStateChanged += OnWorkflowEnded;

            Start(workload);
        }
	    void OnWorkflowEnded(object sender, WorkflowStateChangedEventArgs eventArgs)
	    {
	        //reset the workflow when it reaches End state
	        if (eventArgs.State == WorkflowState.End) Reset();
	    }

        public void End()
		{
			//Just in case new states later
			if (State == WorkflowState.NotRunning || State == WorkflowState.Running)
			{
				State = WorkflowState.End;
			}
		}

		/// <summary>
		/// Reset the workflow run state.
		/// </summary>
		public void Reset()
		{
			_WorkflowStep = 0;
			State = WorkflowState.NotRunning;
			foreach (var item in _Actions)
			{
				item.Reset();
			}
		}

		/// <summary>
		/// Unblock a blocked workflow.
		/// </summary>
		public void Unblock()
		{
			Unblock(1);
		}

		/// <summary>
		/// Unblock a blocked workflow.
		/// </summary>
		public void Unblock(int level)
		{
			UnblockInternal(level);
			Run();
		}

		/// <summary>
		/// Unblock a blocked workflow.
		/// </summary>
		internal void UnblockInternal(int level)
		{
			if (_Actions[_WorkflowStep] != null)
			{
				_Actions[_WorkflowStep].Unblock(level);
			}
			State = WorkflowState.Running;
		}

		/// <summary>
		/// Unblock a blocked workflow.
		/// </summary>
		public void UnblockAll()
		{
			Unblock(Int32.MaxValue);
		}

		/// <summary>
		/// Configure a 'block' step which block the execution of the workflow.
		/// </summary>
		public IWorkflow<T> Block()
		{
			Block(1);
			return this;
		}

		/// <summary>
		/// Configure a 'block' step which block the execution of the workflow.
		/// </summary>
		/// <param name="blockCount">Number of unblock that must be called before starting the workflow.</param>
		public IWorkflow<T> Block(int blockCount)
		{
			Block((obj) => { return blockCount; });
			return this;
		}

		/// <summary>
		/// Configure a 'block' step which block the execution of the workflow.
		/// </summary>
		/// <param name="blockCount">Function that retuen dynamic block count. Evaluated late after definition.</param>
		public IWorkflow<T> Block(Func<T, int> blockCount)
		{
			_Actions.Add(new BlockWorkflowAction<T>(blockCount));
			return this;
		}

		/// <summary>
		/// Configure a 'Go' step.
		/// </summary>
		/// <param name="action">Action that need to be run</param>
		/// <returns></returns>
		public IWorkflow<T> Do(Action<T> action)
		{
			_Actions.Add(new GoWorkflowAction<T>(action));
			return this;
		}

		/// <summary>
		/// Configure a 'Go' step.
		/// </summary>
		/// <param name="actions">Actions that need to be run</param>
		/// <returns></returns>
		public IWorkflow<T> DoAsynch(params Action<T>[] actions)
		{
			_Actions.Add(new MultipleGoWorkflowAction<T>(actions, false));
			return this;
		}

	    /// <summary>
	    /// Configure a 'Go' step.
	    /// </summary>
	    /// <param name="actions">Actions that need to be run</param>
	    /// <returns></returns>
	    public IWorkflow<T> DoParallel(params Action<T>[] actions)
	    {
	        _Actions.Add(new MultipleGoWorkflowAction<T>(actions, true));
	        return this;
	    }
		
        /// <summary>
        /// Configure a 'Foreach' step.
        /// </summary>
        /// <typeparam name="U">Type of parameter you want to loop on.</typeparam>
        /// <param name="itemExtractor">Function that return the list of items.</param>
        /// <param name="action">Action that must be done on each loop.</param>
        /// <returns></returns>
        public IWorkflow<T> Foreach<U>(Func<T, IEnumerable<U>> itemExtractor, Action<Tuple<U, T>> action)
		{
			return Foreach(itemExtractor, (new Workflow<Tuple<U, T>>()).Do(action));
		}

		/// <summary>
		/// Configure a 'Foreach' step.
		/// </summary>
		/// <typeparam name="U"></typeparam>
		/// <param name="itemExtractor">Function that will be dynamicaly resolved and list items.</param>
		/// <param name="workflow">Workflow that will be run for each item listed.</param>
		/// <returns></returns>
		public IWorkflow<T> Foreach<U>(Func<T, IEnumerable<U>> itemExtractor, IWorkflow<Tuple<U, T>> workflow)
		{
			_Actions.Add(new ForWorkflowAction<U, T>(itemExtractor, workflow));

			return this;
		}

		/// <summary>
		/// Run a step and block while the condition is true.
		/// </summary>
		/// <param name="action">Action that must be run</param>
		/// <param name="condition">Condition to run the action.</param>
		/// <returns></returns>
		public IWorkflow<T> While(Func<T, bool> condition, Action<T> action)
		{
			_Actions.Add(new WhileWorkflowAction<T>(condition, action));

			return this;
		}

		/// <summary>
		/// Run a step and block while the condition is true.
		/// </summary>
		/// <param name="workflow">Workflow that must be run</param>
		/// <param name="condition">Condition to run the action.</param>
		/// <returns></returns>
		public IWorkflow<T> While(Func<T, bool> condition, IWorkflow<T> workflow)
		{
			_Actions.Add(new WhileWorkflowAction<T>(condition, workflow));

			return this;
		}

		/// <summary>
		/// Run a step depending the condition.
		/// </summary>
		/// <param name="condition">Condition to run the action.</param>
		/// <param name="actionIfTrue">Action that must be run if contition is true</param>
		/// <param name="actionIfFalse">Action that must be run if condition is false</param>
		/// <returns></returns>
		public IWorkflow<T> If(Func<T, bool> condition, Action<T> actionIfTrue, Action<T> actionIfFalse)
		{
			_Actions.Add(new IfWorkflowAction<T>(condition, actionIfTrue, actionIfFalse));

			return this;
		}

		/// <summary>
		/// Run a step depending the condition.
		/// </summary>
		/// <param name="condition">Condition to run the action.</param>
		/// <param name="actionsIfTrue">Actions that must be run if condition is true</param>
		/// <param name="actionsIfFalse">Actions that must be run if condition is false</param>
		/// <returns></returns>
		public IWorkflow<T> If(Func<T, bool> condition, IWorkflow<T> actionsIfTrue, IWorkflow<T> actionsIfFalse)
		{
			_Actions.Add(new IfWorkflowAction<T>(condition, actionsIfTrue, actionsIfFalse));

			return this;
		}

		/// <summary>
		/// Run the workflow.
		/// </summary>
		private void Run()
		{
			while (_WorkflowStep < _Actions.Count)
			{
				if (State == WorkflowState.Running)
				{
					switch (_Actions[_WorkflowStep].State)
					{
						case WorkflowActionState.Blocked:
							State = WorkflowState.Blocked;
							return;

						case WorkflowActionState.Ended:
							_WorkflowStep++;
							break;

						case WorkflowActionState.Ready:
							_Actions[_WorkflowStep].Resolve(Workload);
							_Actions[_WorkflowStep].Run(Workload);
							break;
					}
				}
			}
            State = WorkflowState.End;
        }

	    private void RaiseWorkflowStateChangedEvent(WorkflowState state)
	    {
	        WorkflowStateChanged?.Invoke(this, new WorkflowStateChangedEventArgs(state));
	    }

        /// <summary>
        /// Occurs when workflow changes its state.
        /// </summary>
	    public event EventHandler<WorkflowStateChangedEventArgs> WorkflowStateChanged;
    }
}