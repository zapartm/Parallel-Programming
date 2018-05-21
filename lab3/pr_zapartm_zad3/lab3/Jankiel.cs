using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace lab3
{
    enum JankielStatus { winner, loser, unknown }
    class Jankiel
    {
        public (int, int) position;
        public List<Jankiel> neighbours;
        MailBox mail;
        public int id;
        public JankielStatus status;

        public Jankiel(int id, int x, int y, MailBox mailBox)
        {
            this.id = id;
            mail = mailBox;
            position = (x, y);
            neighbours = new List<Jankiel>();
            status = JankielStatus.unknown;
        }


        public void DoWork()
        {
            bool work = true;
            while (work)
            {
                List<int> neighboursIds = new List<int>();
                bool keepAsking = true;

                if (this.status == JankielStatus.unknown)
                {
                    MessageFromClient msg = new MessageFromClient(this, id);
                    mail.ClientSend(msg);
                }

                while (keepAsking && work)
                {
                    MessageFromServer resp = mail.ClientReceive(id);
                    switch (resp.GetMessageType())
                    {
                        case ServerMessageType.ID:
                            neighboursIds.Add(resp.GetID());
                            break;
                        case ServerMessageType.MOVE_ON:
                            keepAsking = false;
                            break;
                        case ServerMessageType.RESOLVED:
                            work = false;
                            break;
                    }
                }
                if (!work || this.status == JankielStatus.loser || this.status == JankielStatus.winner) continue;

                bool tmp = true;
                foreach (var v in neighboursIds)
                {
                    if (v > this.id)
                    {
                        tmp = false;
                        break; 
                    }
                }
                if (tmp)
                {
                    this.status = JankielStatus.winner;
                }

            }
            //Console.WriteLine("Jankiel {0} gra", this.id);
        }


        public void FindNeighbours(List<Jankiel> list)
        {
            neighbours.AddRange(list.Where(jj => IsInRange(jj)));
        }


        bool IsInRange(Jankiel jankiel)
        {
            var DIST = 3.0;
            if (jankiel.position.Item1 == this.position.Item1 && jankiel.position.Item2 == this.position.Item2) return false;
            return Math.Sqrt(Math.Pow(jankiel.position.Item1 - this.position.Item1, 2) + Math.Pow(jankiel.position.Item2 - this.position.Item2, 2)) <= DIST;
        }
    }

}
;