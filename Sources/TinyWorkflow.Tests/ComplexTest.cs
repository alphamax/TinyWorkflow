using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinyWorkflow.Tests.States;
using System.Collections.Generic;

namespace TinyWorkflow.Tests
{
    [TestClass]
    public class ComplexTest
    {
        [TestMethod]
        public void FindAllPrimeNumbersAfter()
        {
            Workflow<ComplexState> workflow = new Workflow<ComplexState>()
                //Define the 
                .Do(DefineInitialList)
                .Foreach(DynamicEnumerateItem,
                    new Workflow<Tuple<int, ComplexState>>()
                    .Do(InitWhile)
                    .While(TestNumber, InternalWhileContent)
                    .Do(RegisterResult));

            workflow.Start(new ComplexState());
            Assert.AreEqual(7, workflow.Workload.ListFinal[0]);
            Assert.AreEqual(7, workflow.Workload.ListFinal[1]);
            Assert.AreEqual(11, workflow.Workload.ListFinal[2]);
        }

        public void DefineInitialList(ComplexState state)
        {
            state.ListInitial = new List<int>() { 5, 6, 9 };
        }

        public IEnumerable<int> DynamicEnumerateItem(ComplexState state)
        {
            return state.ListInitial;
        }

        public IEnumerable<int> DynamicEnumerateItemResult(ComplexState state)
        {
            return state.ListFinal;
        }

        public void PrintNumber(Tuple<int, ComplexState> state)
        {
            Console.WriteLine(state.Item1 + " is a prime number");
        }

        public void InitWhile(Tuple<int, ComplexState> state)
        {
            state.Item2.CacheValue = state.Item1 + 1;
        }

        public bool TestNumber(Tuple<int, ComplexState> state)
        {
            return !IsPrimeNumber(state.Item2.CacheValue);
        }

        public void InternalWhileContent(Tuple<int, ComplexState> state)
        {
            state.Item2.CacheValue++;
        }

        public void RegisterResult(Tuple<int, ComplexState> state)
        {
            state.Item2.ListFinal.Add(state.Item2.CacheValue);
        }

        private bool IsPrimeNumber(int number)
        {
            for (int i = 2; i <= number/2; i++)
            {
                if ((number % i == 0))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
