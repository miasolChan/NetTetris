using System;



namespace TetrisServer

{

    class EventHandler

    {

        public static void OnDisconnect(ClientState c)

        {
            string desc = c.socket.RemoteEndPoint.ToString();

            string sendStr = "Leave|" + desc + ",";

            foreach (ClientState cs in FrmMain.clients.Values)

                FrmMain.Send(cs, sendStr);
        }

    }

}