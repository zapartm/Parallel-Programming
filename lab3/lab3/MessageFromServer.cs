using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    enum ServerMessageType { ID, MOVE_ON, RESOLVED}

    class MessageFromServer
    {
        int id;
        ServerMessageType type;

        public MessageFromServer(ServerMessageType type)
        {
            this.type = type;
        }

        public MessageFromServer(ServerMessageType type, int id = -1)
        {
            this.id = id;
            this.type = type;
        }

        public ServerMessageType GetMessageType()
        {
            return type;
        }

        public int GetID()
        {
            return id;
        }
    }

}
