using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class ParallelStepTest
    {
        private IWorkflow<SimpleState> temporaryWorkflow;

        [TestMethod]
        public void RunOneParallelStep()
        {
            temporaryWorkflow = new Workflow<SimpleState>()
                .DoParallel(EasyAction)
                .Do(EasyParallelTestValidation);
            temporaryWorkflow.Start(new SimpleState());
        }

        [TestMethod]
        public void RunMultipleParallelStep()
        {
            temporaryWorkflow = new Workflow<SimpleState>()
                .DoParallel(EasyActionParallel1, EasyActionParallel2, EasyActionParallel3)
                .Do(ParallelTestValidation);
            temporaryWorkflow.Start(new SimpleState());
        }

        public void EasyParallelTestValidation(SimpleState state)
        {
            Assert.AreEqual(1, state.StateFullVariable);
        }

        public void ParallelTestValidation(SimpleState state)
        {
            Assert.AreEqual(10, state.StateFullVariableAsynch1);
            Assert.AreEqual(20, state.StateFullVariableAsynch2);
            Assert.AreEqual(30, state.StateFullVariableAsynch3);
        }

        public void EasyAction(SimpleState state)
        {
            Thread.Sleep(1000);
            state.StateFullVariable = 1;
        }

        public void EasyActionParallel1(SimpleState state)
        {
            Thread.Sleep(1200);
            state.StateFullVariableAsynch1 = 10;
        }

        public void EasyActionParallel2(SimpleState state)
        {
            Thread.Sleep(700);
            state.StateFullVariableAsynch2 = 20;
        }

        public void EasyActionParallel3(SimpleState state)
        {
            Thread.Sleep(1000);
            state.StateFullVariableAsynch3 = 30;
        }
    }
}