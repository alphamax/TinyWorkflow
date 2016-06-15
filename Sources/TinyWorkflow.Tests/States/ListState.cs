using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Tests.States
{
    public class ListState
    {
        public List<int> List { get; set; }
        public int StateFullVariable { get; set; }

        public ListState()
        {
            List = new List<int>();
            StateFullVariable = 0;
        }
    }
}
