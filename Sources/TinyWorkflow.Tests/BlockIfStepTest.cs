using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class BlockIfStepTest
    {
        [TestMethod]
        public void RunIfTrueAndBlock()
        {
            Workflow<SimpleState> workflow = new Workflow<SimpleState>()
                .If(TestToTrue, ActionIfTrue, ActionIfFalse)
                .Block()
                .Do(ActionAfter);
            workflow.Start(new SimpleState());
            Assert.AreEqual(2, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(3, workflow.Workload.StateFullVariable);
        }

        [TestMethod]
        public void RunIfFalseAndBlock()
        {
            Workflow<SimpleState> workflow = new Workflow<SimpleState>()
                .If(TestToFalse, ActionIfTrue, ActionIfFalse)
                .Block()
                .Do(ActionAfter);
            workflow.Start(new SimpleState());
            Assert.AreEqual(3, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(4, workflow.Workload.StateFullVariable);
        }

        public bool TestToTrue(SimpleState state)
        {
            return state.StateFullVariable == 0;
        }

        public bool TestToFalse(SimpleState state)
        {
            return state.StateFullVariable != 0;
        }

        public void ActionIfTrue(SimpleState state)
        {
            state.StateFullVariable = 2;
        }

        public void ActionIfFalse(SimpleState state)
        {
            state.StateFullVariable = 3;
        }

        public void ActionAfter(SimpleState state)
        {
            state.StateFullVariable++;
        }
    }
}
