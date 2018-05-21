using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{

    class MessageFromClient
    {
        public Jankiel sender;
        public int id;

        public MessageFromClient(Jankiel sender, int id)
        {
            this.sender = sender;
            this.id = id;
        }

        //public MessageFromClient(Jankiel sender, int id, ClientState clientState)
        //{
        //    this.sender = sender;
        //    this.id = id;
        //    this.clientState = clientState;
        //}
    }

   

}
