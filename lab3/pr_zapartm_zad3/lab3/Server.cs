using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace lab3
{
    class Server
    {
        int N;
        MailBox mail;
        List<Jankiel> list;

        public Server(int n, MailBox mail, List<Jankiel> list)
        {
            N = n;
            this.mail = mail;
            this.list = list;
        }

        public void DoWork()
        {
            Thread.Sleep(500);
            int counter = 0;
            int roundNumber = 1;

            int clientsLeft = 0;
            while (list.Count > 0)
            {
                Console.WriteLine("Runda {0}", roundNumber++);

                while (true)
                {
                    clientsLeft = 0;
                    counter = 0;

                    Thread.Sleep(250);
                    foreach (var j in list)
                    {
                        if (j.status == JankielStatus.winner)
                        {
                            foreach (var jj in j.neighbours)
                            {
                                jj.status = JankielStatus.loser;
                            }
                        }
                    }

                    foreach (var j in list)
                    {
                        if (j.status == JankielStatus.unknown)
                            clientsLeft++;
                    }

                    while (counter++ < clientsLeft)
                    {
                        MessageFromClient msg = mail.ServerReceive();

                        foreach (var jj in msg.sender.neighbours)
                        {
                            if (jj.status == JankielStatus.unknown)
                            {
                                MessageFromServer message = new MessageFromServer(ServerMessageType.ID, msg.sender.id);
                                mail.ServerSend(jj.id, message);
                            }
                        }
                    }

                    Thread.Sleep(250);

                    if (clientsLeft > 0)
                    {
                        foreach (var j in list)
                        {
                            MessageFromServer message = new MessageFromServer(ServerMessageType.MOVE_ON);
                            mail.ServerSend(j.id, message);
                        }
                    }
                    else
                    {
                        foreach (var j in list)
                        {
                            if (j.status != JankielStatus.winner) continue;
                            MessageFromServer message = new MessageFromServer(ServerMessageType.RESOLVED);
                            mail.ServerSend(j.id, message);
                        }
                        break; 
                    }
                    Thread.Sleep(250);

                } // koniec rundy

                list.ForEach(item =>
                {
                    if (item.status == JankielStatus.loser)
                        item.status = JankielStatus.unknown;
                });

                foreach (var j in list)
                {
                    if (j.status == JankielStatus.winner)
                        Console.WriteLine("Jankiel {0} gra", j.id);
                }

                list.RemoveAll(p => { return p.status == JankielStatus.winner; });
            }

        }
    }
}
