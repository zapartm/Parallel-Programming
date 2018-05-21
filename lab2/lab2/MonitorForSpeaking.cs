using CodeExMachina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class MonitorForSpeaking
    {
        int N;
        Object obj = new object();

        bool kingSpeaking = false;
        ConditionVariable kingStopsSpeaking = new ConditionVariable();
        bool[] isWaiting;
        bool[] isBusy;
        ConditionVariable[] hisTurnCondition;
        bool waitPrev;

        public MonitorForSpeaking(int N)
        {
            this.N = N;

            isWaiting = new bool[N]; 
            isBusy = new bool[N]; 
            hisTurnCondition = new ConditionVariable[N];

            for (int i = 0; i < N; i++)
            {
                isBusy[i] = false;
                hisTurnCondition[i] = new ConditionVariable();
            }
        }

        public void StartSpeaking(int i)
        {
            lock (obj)
            {
                if(i != 0) // jeśli król mówi to i tak musimy czekać
                    if (kingSpeaking) kingStopsSpeaking.Wait(obj);

                if (isBusy[i] == true || isBusy[(i + 1) % N] == true)
                {
                    isWaiting[i] = true;
                    hisTurnCondition[i].Wait(obj);
                    if (i != 0 && kingSpeaking) // w czasie czekania na sąsiadów król mógł zacząć mówić
                        kingStopsSpeaking.Wait(obj);
                    isWaiting[i] = false;
                }
                if (i == 0) // jeśli jest królem
                    kingSpeaking = true;

                isBusy[i] = true;
                isBusy[(i + 1) % N] = true;
                if (waitPrev)
                {
                    waitPrev = false;
                    hisTurnCondition[(N + i - 2) % N].Pulse();
                }
            }
        }

        public void SpeakingDone(int i)
        {
            lock (obj)
            {
                if (i == 0) // case if is the king
                {
                    kingSpeaking = false;
                    kingStopsSpeaking.PulseAll();
                }

                isBusy[i] = false;
                isBusy[(i + 1) % N] = false;
                if (isWaiting[(N + i - 1) % N] && !isBusy[(N + i - 1) % N])
                    waitPrev = true;
                if (isWaiting[(i + 1) % N] && !isBusy[(i + 2) % N])
                    hisTurnCondition[(i + 1) % N].Pulse();
                if (waitPrev)
                {
                    waitPrev = false;
                    hisTurnCondition[(N + i - 1) % N].Pulse();
                }
            }
        }

    
    }
}
