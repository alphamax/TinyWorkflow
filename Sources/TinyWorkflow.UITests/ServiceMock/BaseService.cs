using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TinyWorkflow.UITests.ServiceMock
{
    public class BaseService
    {
        protected Random r = new Random(DateTime.Now.Millisecond);
        protected void GenerateSleep(int min, int max)
        {
            int delay = min + r.Next(max - min);
            Thread.Sleep(delay);
        }
    }
}
