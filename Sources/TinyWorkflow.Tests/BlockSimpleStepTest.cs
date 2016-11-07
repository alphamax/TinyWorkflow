using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;

namespace TinyWorkflow.Tests
{
	[TestClass]
	public class BlockSimpleStepTest
	{
		[TestMethod]
		public void SimpleStepBlock()
		{
			var workflow = new Workflow<SimpleState>()
				.Do(ActionBeforeBlock)
				.Block()
				.Do(ActionAfterBlock);

			workflow.Start(new SimpleState());
			Assert.AreEqual(5, workflow.Workload.StateFullVariable);
			workflow.Unblock();
			Assert.AreEqual(10, workflow.Workload.StateFullVariable);
		}

		public void ActionBeforeBlock(SimpleState state)
		{
			state.StateFullVariable = 5;
		}

		public void ActionAfterBlock(SimpleState state)
		{
			state.StateFullVariable = 10;
		}
	}
}