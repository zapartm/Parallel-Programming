using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab2
{
    class Servant
    {
        MonitorForDrinking monitor;
        public Servant(MonitorForDrinking monitor)
        {
            this.monitor = monitor;
        }

        public void DoWork1()
        {
            var random = new Random(Thread.CurrentThread.ManagedThreadId);
            bool keepGoing = true;

            while(keepGoing)
            {
                Thread.Sleep((int)(random.NextDouble() % 5000) + 5000);
                keepGoing = monitor.RefillCucumbers();
                if (keepGoing)
                    Console.WriteLine(this.ToString() + " refills cucumbers");
                else
                    Console.WriteLine(this.ToString() + " terminates");
            }
        }

        public void DoWork2()
        {
            var random = new Random();
            bool keepGoing = true;

            while (keepGoing)
            {
                Thread.Sleep((int)(random.NextDouble() % 5000) + 5000);
                keepGoing = monitor.RefillWine();
                if(keepGoing)
                    Console.WriteLine(this.ToString() + " refills wine");
                else
                    Console.WriteLine(this.ToString() + " terminates");
            }
        }

        public override string ToString()
        {
            return "Servant";
        }
    }
}
