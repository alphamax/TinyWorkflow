using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Tests.States
{
    public class ConditionState
    {
        public int MaxRun { get; set; }
        public int ActualRun { get; set; }
        public int StateFullVariable { get; set; }

        public ConditionState()
        {
            ActualRun = 0;
            MaxRun = 0;
            StateFullVariable = 0;
        }
    }
}
