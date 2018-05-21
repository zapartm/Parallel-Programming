using CodeExMachina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    class MonitorForDrinking
    {
        readonly int N;
        readonly int initialC;
        readonly int initialW;
        Object obj = new object();

        int KnightsAlive;
        int wineState;
        int[] cucumbersState;
        ConditionVariable cucumbersRefillCond;
        ConditionVariable wineRefillCond;

        bool[] isWaiting;
        bool[] isBusy;
        ConditionVariable[] hisTurnCond;
        bool waitPrev;

        public MonitorForDrinking(int N, int w, int c)
        {
            this.N = N;
            KnightsAlive = N;
            wineState = w;
            cucumbersState = new int[N / 2];
            initialC = c;
            initialW = w;

            isWaiting = new bool[N];
            isBusy = new bool[N];
            hisTurnCond = new ConditionVariable[N];
            cucumbersRefillCond = new ConditionVariable();
            wineRefillCond = new ConditionVariable();

            for (int i = 0; i < N; i++)
            {
                cucumbersState[i / 2] = c;
                isBusy[i] = false;
                hisTurnCond[i] = new ConditionVariable();
            }
        }

        public void StartDrinking(int i)
        {
            lock (obj)
            {
                bool check = true;
                do{
                    if (cucumbersState[((N + i - 1) % N) / 2] == 0)
                    {
                        Console.WriteLine("Run out of cucumbers");
                        cucumbersRefillCond.Wait(obj);
                    }
                    if (wineState == 0)
                    {
                        Console.WriteLine("Run out of wine");
                        wineRefillCond.Wait(obj);
                    }
                    check = (cucumbersState[((N + i - 1) % N) / 2] == 0); // sprawdzamy jeszcze raz stan ogórków, podczas czekania na wino mogły sie skończyć
                    check = (wineState == 0);
                } while (check);
                cucumbersState[((N + i - 1) % N) / 2]--;
                wineState--;

                if (isBusy[i] == true || isBusy[(i + 1) % N] == true)
                {
                    isWaiting[i] = true;
                    hisTurnCond[i].Wait(obj);
                    isWaiting[i] = false;
                }

                isBusy[i] = true;
                isBusy[(i + 1) % N] = true;
                if (waitPrev)
                {
                    waitPrev = false;
                    hisTurnCond[(N + i - 2) % N].Pulse();
                }
            }
        }

        public void DrinkingDone(int i)
        {
            lock (obj)
            {
                isBusy[i] = false;
                isBusy[(i + 1) % N] = false;
                if (isWaiting[(N + i - 1) % N] && !isBusy[(N + i - 1) % N])
                    waitPrev = true;
                if (isWaiting[(i + 1) % N] && !isBusy[(i + 2) % N])
                    hisTurnCond[(i + 1) % N].Pulse();
                if (waitPrev)
                {
                    waitPrev = false;
                    hisTurnCond[(N + i - 1) % N].Pulse();
                }
            }
        }

        public bool RefillWine()
        {
            lock (obj)
            {
                if (KnightsAlive == 0)
                    return false;

                wineState = initialW;
                wineRefillCond.PulseAll();
                return true;
            }
        }

        public bool RefillCucumbers()
        {
            lock (obj)
            {
                if (KnightsAlive == 0)
                    return false;

                for (int i = 0; i < N / 2; i++)
                {
                    cucumbersState[i] = initialC;
                    cucumbersRefillCond.PulseAll();
                }

                return true;
            }
        }

        public void Die()
        {
            lock (obj)
            {
                KnightsAlive--;
            }
        }
    }
}
