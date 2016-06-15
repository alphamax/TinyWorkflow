using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;
using System.Collections.Generic;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class BlockWhileStepTest
    {
        [TestMethod]
        public void RunWhileWithBlock()
        {
            Workflow<ConditionState> workflow = new Workflow<ConditionState>()
                .While(WhileTest,
                    new Workflow<ConditionState>()
                    .Do(WhileContent)
                    .Block()
                );

            workflow.Start(new ConditionState() { MaxRun = 5});
            Assert.AreEqual(0, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(1, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(3, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(6, workflow.Workload.StateFullVariable);
            workflow.Unblock();
            Assert.AreEqual(10, workflow.Workload.StateFullVariable);
        }

        public void WhileContent(ConditionState state)
        {
            state.StateFullVariable += state.ActualRun;
            state.ActualRun++;
        }

        public bool WhileTest(ConditionState state)
        {
            return state.ActualRun < state.MaxRun;
        }
    }
}
