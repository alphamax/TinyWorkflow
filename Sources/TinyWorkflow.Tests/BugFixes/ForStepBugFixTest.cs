using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;
using System.Collections.Generic;

namespace TinyWorkflow.Tests.BugFixes
{
    /// <summary>
    /// Do not run indefinitely  when no items returned (Bug #25775)
    /// </summary>
    [TestClass]
    public class ForStepBugFixTest
    {
        [TestMethod]
        public void RunStaticForeach()
        {
            Workflow<ListState> workflow = new Workflow<ListState>()
                .Foreach(StaticEnumerateItem,
                    new Workflow<Tuple<int, ListState>>()
                    .Do(ActionInForeach)
                );

            workflow.Start(new ListState());
            Assert.AreEqual(0, workflow.Workload.StateFullVariable);
        }

        [TestMethod]
        public void RunDynamicForeach()
        {
            Workflow<ListState> workflow = new Workflow<ListState>()
                .Do(DefineList)
                .Foreach(DynamicEnumerateItem,
                    new Workflow<Tuple<int, ListState>>()
                    .Do(ActionInForeach)
                );

            workflow.Start(new ListState());
            Assert.AreEqual(0, workflow.Workload.StateFullVariable);
        }

        public void DefineList(ListState state)
        {
            state.List = new List<int>() {};
        }

        public IEnumerable<int> DynamicEnumerateItem(ListState state)
        {
            return state.List;
        }

        public IEnumerable<int> StaticEnumerateItem(ListState state)
        {
            return new List<int>() {};
        }

        public void ActionInForeach(Tuple<int,ListState> state)
        {
            state.Item2.StateFullVariable += state.Item1;
        }
    }
}
