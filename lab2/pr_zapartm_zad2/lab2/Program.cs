using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using CodeExMachina;

namespace lab2
{
    class Program
    {
        static int N = 4; // ilość rycerzy + król
        static int w = 2; // pojemność butelki (w kielichach)
        static int c = 2; // maksymalna ilosć ogórków na talerzyku 

        static void Main(string[] args)
        {
            MonitorForSpeaking monitorForSpeaking = new MonitorForSpeaking(N);
            MonitorForDrinking monitorForDrinking = new MonitorForDrinking(N, w, c);

            for (int i = 0; i < N; i++)
            {
                Thread thread;
                if (i == 0)
                {
                    King king = new King(i, monitorForSpeaking, monitorForDrinking);
                    thread = new Thread(new ThreadStart(king.DoWork));
                }
                else
                {
                    Knight knight = new Knight(i, monitorForSpeaking, monitorForDrinking);
                    thread = new Thread(new ThreadStart(knight.DoWork));
                }
                thread.Start();
            }

            Servant servant1 = new Servant(monitorForDrinking);
            Thread thread1 = new Thread(new ThreadStart(servant1.DoWork1));
            thread1.Start();

            Servant servant2 = new Servant(monitorForDrinking);
            Thread thread2 = new Thread(new ThreadStart(servant2.DoWork2));
            thread2.Start();
        }

    }

}
