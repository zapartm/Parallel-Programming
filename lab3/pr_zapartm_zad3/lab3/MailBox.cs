using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeExMachina;

namespace lab3
{
    class MailBox
    {
        Object obj = new object();
        Queue<MessageFromClient> messages = new Queue<MessageFromClient>();
        ConditionVariable empty = new ConditionVariable();
        Queue<MessageFromServer>[] clientMessages;
        ConditionVariable[] emptyClient;

        public MailBox(int N)
        {
            clientMessages = new Queue<MessageFromServer>[N];
            emptyClient = new ConditionVariable[N];

            for (int i = 0; i < N; i++)
            {
                clientMessages[i] = new Queue<MessageFromServer>();
                emptyClient[i] = new ConditionVariable();
            }
        }

        public void ClientSend(MessageFromClient msg)
        {
            lock(obj)
            {
                messages.Enqueue(msg);
                if (messages.Count == 1)
                    empty.Pulse();
            }
        }

        public MessageFromServer ClientReceive(int id)
        {
            lock (obj)
            {
                if (clientMessages[id].Count == 0)
                    emptyClient[id].Wait(obj);
                MessageFromServer message = clientMessages[id].Dequeue();
                return message;
            }
        }

        public void ServerSend(int id, MessageFromServer msg)
        {
            lock (obj)
            {
                clientMessages[id].Enqueue(msg);
                if (clientMessages[id].Count == 1)
                    emptyClient[id].Pulse();
            }
        }

        public MessageFromClient ServerReceive()
        {
            lock(obj)
            {
                if (messages.Count == 0)
                    empty.Wait(obj);
                MessageFromClient message = messages.Dequeue();
                return message;
            }
        }
    }
}
