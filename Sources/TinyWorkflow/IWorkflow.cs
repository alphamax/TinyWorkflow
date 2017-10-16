using System;
using System.Collections.Generic;

namespace TinyWorkflow
{
	public interface IWorkflow<T>
	{
		T Workload { get; set; }

		WorkflowState State { get; }

		void Start(T workload);

	    void StartAndReset(T workload);

		void End();

		void Reset();

		void Unblock();

		void Unblock(int level);

		void UnblockAll();

		IWorkflow<T> Block();

		IWorkflow<T> Block(int blockCount);

		IWorkflow<T> Block(Func<T, int> blockCount);

		IWorkflow<T> Do(Action<T> action);

		IWorkflow<T> DoAsynch(params Action<T>[] actions);

	    IWorkflow<T> DoParallel(params Action<T>[] actions);

        IWorkflow<T> Foreach<U>(Func<T, IEnumerable<U>> itemExtractor, Action<Tuple<U, T>> action);

		IWorkflow<T> Foreach<U>(Func<T, IEnumerable<U>> itemExtractor, IWorkflow<Tuple<U, T>> workflow);

		IWorkflow<T> While(Func<T, bool> condition, Action<T> action);

		IWorkflow<T> While(Func<T, bool> condition, IWorkflow<T> workflow);

		IWorkflow<T> If(Func<T, bool> condition, Action<T> actionIfTrue, Action<T> actionIfFalse);

		IWorkflow<T> If(Func<T, bool> condition, IWorkflow<T> actionsIfTrue, IWorkflow<T> actionsIfFalse);
	}
}