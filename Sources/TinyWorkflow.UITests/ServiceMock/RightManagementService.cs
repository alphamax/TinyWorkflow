using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyWorkflow.UITests.ServiceMock.Enums;

namespace TinyWorkflow.UITests.ServiceMock
{
    public class RightManagementService : BaseService
    {
        public void HasPrivilege(Rights privilege, Action<bool> callback)
        {
            Task.Factory.StartNew(() =>
                {
                    this.GenerateSleep(500, 1500);
                    if (privilege == Rights.ReadOnly)
                    {
                        callback(true);
                    }
                    else if (privilege == Rights.ReadWrite)
                    {
                        callback(true);
                    }
                    else
                    {
                        callback(false);
                    }
                });
        }
    }
}
