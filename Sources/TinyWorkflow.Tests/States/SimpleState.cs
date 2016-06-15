using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Tests.States
{
    public class SimpleState
    {
        public int StateFullVariable { get; set; }
        public int StateFullVariableAsynch1 { get; set; }
        public int StateFullVariableAsynch2 { get; set; }
        public int StateFullVariableAsynch3 { get; set; }

        public SimpleState()
        {
            StateFullVariable = 0;
            StateFullVariableAsynch1 = 0;
            StateFullVariableAsynch2 = 0;
            StateFullVariableAsynch3 = 0;
        }
    }
}
