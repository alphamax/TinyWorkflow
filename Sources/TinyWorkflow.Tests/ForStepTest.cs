using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;

namespace TinyWorkflow.Tests
{
	[TestClass]
	public class ForStepTest
	{
		[TestMethod]
		public void RunStaticForeach()
		{
			var workflow = new Workflow<ListState>()
				.Foreach(StaticEnumerateItem,
					new Workflow<Tuple<int, ListState>>()
					.Do(ActionInForeach)
				);

			//Workflow<ListState> workflow = new Workflow<ListState>()
			//    .Foreach(StaticEnumerateItem,ActionInForeach);

			workflow.Start(new ListState());
			Assert.AreEqual(10, workflow.Workload.StateFullVariable);
		}

		[TestMethod]
		public void RunDynamicForeach()
		{
			var workflow = new Workflow<ListState>()
				.Do(DefineList)
				.Foreach(DynamicEnumerateItem,
					new Workflow<Tuple<int, ListState>>()
					.Do(ActionInForeach)
				);

			workflow.Start(new ListState());
			Assert.AreEqual(20, workflow.Workload.StateFullVariable);
		}

		public void DefineList(ListState state)
		{
			state.List = new List<int>() { 5, 6, 9 };
		}

		public IEnumerable<int> DynamicEnumerateItem(ListState state)
		{
			return state.List;
		}

		public IEnumerable<int> StaticEnumerateItem(ListState state)
		{
			return new List<int>() { 3, 5, 2 };
		}

		public void ActionInForeach(Tuple<int, ListState> state)
		{
			state.Item2.StateFullVariable += state.Item1;
		}
	}
}