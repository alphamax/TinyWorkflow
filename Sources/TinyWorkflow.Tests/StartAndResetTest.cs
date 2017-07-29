using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class StartAndResetTest
    {
        [TestMethod]
        public void StartAndResetSingleStepWorkflow()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Do(c => c.Data = c.Data.ToUpper());

            var contextData = "sample";
            
            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetMultipleTimes()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Do(c => c.Data = c.Data.ToUpper());

            var contextData = "sample";

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetWithMultipleSteps()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Do(c => c.Data = c.Data.ToUpper())
                .Do(c => c.Data = c.Data += c.Data)
                .Do(c => c.Data = c.Data.ToLower());

            var contextData = "sample";

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData + contextData, workflow.Workload.Data);

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData + contextData, workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetWithBlockingAction()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Do(c => c.Data = c.Data.ToUpper())
                .Block(1)
                .Do(c => c.Data += c.Data);

            var contextData = "sample";
            
            //start and check state at which workflow should be blocked
            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.Blocked, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);

            //continue workflow and if it has been reset
            workflow.Unblock();
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual((contextData + contextData).ToUpper(), workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetWithForeachWorkflow()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Do(c => c.Data += c.Data)
                .Foreach(context => context.Data.ToCharArray(),
                    context => context.Item2.Data += context.Item1.ToString().ToUpper());

            var contextData = "sample";

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData + contextData + (contextData + contextData).ToUpper(), workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetWithWhileLoop()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .While(context => context.Data != context.Data.ToUpper(), context =>
                {
                    var charArray = context.Data.ToCharArray();
                    for (int i = 0; i < charArray.Length; i++)
                    {
                        if (char.IsLower(charArray[i]))
                        {
                            charArray[i] = char.ToUpper(charArray[i]);
                            context.Data = new string(charArray);
                            break;
                        }
                    }
                });

            var contextData = "sample";

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(contextData.ToUpper(), workflow.Workload.Data);
        }

        [TestMethod]
        public void StartAndResetWithMultipleWorkflows()
        {
            var workflow = new Workflow<SimpleWorkflowContext>()
                .Foreach(context => context.Data.ToCharArray(), new Workflow<Tuple<char, SimpleWorkflowContext>>()
                    .Do(context => context.Item2.Data += context.Item1)
                    .Foreach(context => context.Item2.Data.ToCharArray(), new Workflow<Tuple<char, Tuple<char, SimpleWorkflowContext>>>()
                        .Do(context => context.Item2.Item2.Data = context.Item2.Item2.Data.Substring(0,context.Item2.Item2.Data.Length - 1))
                    )
                );

            var contextData = "sample";

            workflow.StartAndReset(new SimpleWorkflowContext(contextData));
            Assert.AreEqual(WorkflowState.NotRunning, workflow.State);
            Assert.AreEqual(string.Empty, workflow.Workload.Data);
        }

        private class SimpleWorkflowContext
        {
            public string Data { get; set; }

            public SimpleWorkflowContext(string data)
            {
                Data = data;
            }
        }
    }
}
