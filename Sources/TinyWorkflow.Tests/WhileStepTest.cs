using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;
using System.Collections.Generic;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class WhileStepTest
    {
        [TestMethod]
        public void RunWhile()
        {
            Workflow<ConditionState> workflow = new Workflow<ConditionState>()
                .While(WhileTest, WhileContent);

            workflow.Start(new ConditionState() { MaxRun = 5});
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
