using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class SimpleStepTest
    {
        [TestMethod]
        public void RunOneStep()
        {
            Workflow<SimpleState> workflow = new Workflow<SimpleState>()
                .Do(EasyAction);
            workflow.Start(new SimpleState());
            Assert.AreEqual(1, workflow.Workload.StateFullVariable);
        }

        [TestMethod]
        public void RunMultipleStepSequence()
        {
            Workflow<SimpleState> workflow = new Workflow<SimpleState>()
                .Do(EasyAction)
                .Do(EasyAction2)
                .Do(EasyAction3);
            workflow.Start(new SimpleState());
            Assert.AreEqual(3, workflow.Workload.StateFullVariable);
        }

        [TestMethod]
        public void RunHugeMultipleStep()
        {
            Workflow<SimpleState> workflow = new Workflow<SimpleState>();
            for (int i = 0; i < 500; i++)
			{
			    workflow.Do(EasyActionAdd);
			}
            workflow.Start(new SimpleState());
            Assert.AreEqual(500, workflow.Workload.StateFullVariable);
        }


        public void EasyAction(SimpleState state)
        {
            state.StateFullVariable = 1;
        }

        public void EasyAction2(SimpleState state)
        {
            state.StateFullVariable = 2;
        }

        public void EasyAction3(SimpleState state)
        {
            state.StateFullVariable = 3;
        }

        public void EasyActionAdd(SimpleState state)
        {
            state.StateFullVariable++;
        }
    }
}
