using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyWorkflow.Tests.States
{
    public class ComplexState
    {
        public List<int> ListInitial { get; set; }
        public List<int> ListFinal { get; set; }

        public int CacheValue { get; set; }

        public ComplexState()
        {
            ListInitial = new List<int>();
            ListFinal = new List<int>();
        }
    }
}
