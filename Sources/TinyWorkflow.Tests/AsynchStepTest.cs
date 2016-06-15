using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;
using System.Threading;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class AsynchStepTest
    {
        Workflow<SimpleState> temporaryWorkflow;
        [TestMethod]
        public void RunOneAsynchStep()
        {
            temporaryWorkflow = new Workflow<SimpleState>()
                .DoAsynch(EasyAction)
                .Block()
                .Do(EasyAsynchronousTestValidation);
            temporaryWorkflow.Start(new SimpleState());
            Thread.Sleep(3000);
        }

        [TestMethod]
        public void RunMultipleAsynchStep()
        {
            temporaryWorkflow = new Workflow<SimpleState>()
                .DoAsynch(EasyActionAsynch1,EasyActionAsynch2,EasyActionAsynch3)
                .Block(3)
                .Do(AsynchronousTestValidation);
            temporaryWorkflow.Start(new SimpleState());
            Thread.Sleep(3000);
        }

        public void EasyAsynchronousTestValidation(SimpleState state)
        {
            Assert.AreEqual(1, state.StateFullVariable);
        }

        public void AsynchronousTestValidation(SimpleState state)
        {
            Assert.AreEqual(10, state.StateFullVariableAsynch1);
            Assert.AreEqual(20, state.StateFullVariableAsynch2);
            Assert.AreEqual(30, state.StateFullVariableAsynch3);
        }

        public void EasyAction(SimpleState state)
        {
            Thread.Sleep(1000);
            state.StateFullVariable = 1;
            temporaryWorkflow.Unblock();
        }

        public void EasyActionAsynch1(SimpleState state)
        {
            Thread.Sleep(1200);
            state.StateFullVariableAsynch1 = 10;
            temporaryWorkflow.Unblock();
        }

        public void EasyActionAsynch2(SimpleState state)
        {
            Thread.Sleep(700);
            state.StateFullVariableAsynch2 = 20;
            temporaryWorkflow.Unblock();
        }

        public void EasyActionAsynch3(SimpleState state)
        {
            Thread.Sleep(1000);
            state.StateFullVariableAsynch3 = 30;
            temporaryWorkflow.Unblock();
        }
    }
}
