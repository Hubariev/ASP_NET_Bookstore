using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Urok1_povtor_metanit.Models;

namespace Urok1_povtor_metanit.Hubs
{
    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();

        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }

        //Połączenie nowego usera
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Name = userName });

                // Wysłanie wiadomości current user
                Clients.Caller.onConnected(id, userName, Users);

                // Wiadomość do wszystkich userów oprócz current
                Clients.AllExcept(id).onNewUserConnected(id, userName);
            }
        }



        // Odłączenie użytkownika

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
}