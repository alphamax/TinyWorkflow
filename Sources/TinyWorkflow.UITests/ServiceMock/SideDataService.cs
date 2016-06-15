using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyWorkflow.UITests.ServiceMock.Enums;

namespace TinyWorkflow.UITests.ServiceMock
{
    public class SideDataService : BaseService
    {
        List<string> users = new List<string>() { "Jean", "Pierre", "Paul", "Jacques", "Yves", "Choubidoumaster", "Fred", "Marcel" };
        List<string> sections = new List<string>() { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

        public void GetUsers(Action<IEnumerable<string>> callback)
        {
            Task.Factory.StartNew(() =>
            {
                this.GenerateSleep(500, 1500);
                callback(users);
            });
        }

        public void GetSections(Action<IEnumerable<string>> callback)
        {
            Task.Factory.StartNew(() =>
            {
                this.GenerateSleep(500, 1500);
                callback(sections);
            });
        }

        public void GetUserSections(string user, Action<IEnumerable<string>> callback)
        {
            Task.Factory.StartNew(() =>
            {
                this.GenerateSleep(500, 1500);
                List<string> tempSection = sections.ToList();
                int loop = r.Next(5);
                for (int i = 0; i < loop; i++)
                {
                    tempSection.RemoveAt(r.Next(tempSection.Count));
                }
                callback(tempSection);
                
            });
        }

    }
}
