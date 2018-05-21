using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace lab2
{
    class Knight
    {
        protected readonly int myId;
        protected readonly int sleepTime, storyTime;
        protected readonly int drinkingTime = 3000;
        protected MonitorForSpeaking monitorForSpeaking;
        protected MonitorForDrinking monitorForDrinking;

        public Knight(int myId, MonitorForSpeaking monitor1, MonitorForDrinking monitor2)
        {
            this.myId = myId;
            this.monitorForSpeaking = monitor1;
            this.monitorForDrinking = monitor2;
            var random = new Random((int)myId);
            sleepTime = (int)(random.NextDouble() % 5000) + 3000;
            storyTime = (int)(random.NextDouble() % 5000) + 3000;
        }

        public void DoWork()
        {
            for (int i = 0; i < 3; i++)
            {
                // spanie
                Thread.Sleep(sleepTime);

                // opowiadanie
                monitorForSpeaking.StartSpeaking(myId);
                Console.WriteLine(this.ToString() + " starts speaking");
                Thread.Sleep(storyTime);
                monitorForSpeaking.SpeakingDone(myId);
                Console.WriteLine(this.ToString() + " stops speaking");

                // picie
                monitorForDrinking.StartDrinking(myId);
                Console.WriteLine(this.ToString() + " starts drinking");
                Thread.Sleep(drinkingTime);
                monitorForDrinking.DrinkingDone(myId);
                Console.WriteLine(this.ToString() + " stops drinking");
            }

            monitorForDrinking.Die();
            Console.WriteLine(this.ToString() + " terminates");
        }

        public override string ToString()
        {
            return ("Knight " + myId.ToString());
        }
    }


    class King : Knight
    {
        public King(int myId, MonitorForSpeaking monitor1, MonitorForDrinking monitor2) : base(myId, monitor1, monitor2)
        {
        }

        public override string ToString()
        {
            return "King";
        }
    }
}
