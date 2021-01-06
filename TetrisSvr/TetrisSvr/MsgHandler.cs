using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisServer
{
    class MsgHandler

    {
        public static void MsgBlocksInfo(ClientState c, string msgArgs)
        {
            string[] split = msgArgs.Split(',');
            string desc = split[0];
            string curIndexStr = split[1];
            string nextIndexStr = split[2];
            //广播
            string sendStr = "BlocksInfo|" ;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                sendStr += desc + ",";
                sendStr += curIndexStr + ",";
                sendStr += nextIndexStr+ ",";
                FrmMain.Send(cs, sendStr);
            }
        }
        public static void MsgList(ClientState c, string msgArgs)
        {

            //广播
            string sendStr = "List|";
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                sendStr += cs.socket.RemoteEndPoint.ToString() + ",";
            }
            FrmMain.Send(c, sendStr);
        }
        public static void MsgEnter(ClientState c, string msgArgs)
        {
            string[] split = msgArgs.Split(',');
            string desc = split[0];

            //广播
            string sendStr = "Enter|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }
        }

        public static void MsgPrepare(ClientState c,string msgArgs)
        {
            string[] split = msgArgs.Split(',');
            string desc = split[0];

            //广播
            string sendStr = "Prepare|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }
        }

        public static void MsgStart(ClientState c, string msgArgs)
        {
            string[] split = msgArgs.Split(',');
            string desc = split[0];

            //广播
            string sendStr = "Start|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }
        }
        public static void MsgDown(ClientState c, string msgArgs)
        {
            string[] split = msgArgs.Split(',');
            string desc = split[0];

            //广播
            string sendStr = "Down|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }
        }

    }


        /*public static void MsgEnter(ClientState c, string msgArgs)

        {

            string[] split = msgArgs.Split(',');

            string desc = split[0];

            c.x = float.Parse(split[1]);

            c.y = float.Parse(split[2]);

            c.z = float.Parse(split[3]);

            c.eulY = float.Parse(split[4]);

            //广播

            string sendStr = "Enter|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }

        }

        public static void MsgList(ClientState c, string msgArgs)

        {
            string sendStr = "List|";
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                sendStr += cs.socket.RemoteEndPoint.ToString() + ",";
                sendStr += cs.x + ",";
                sendStr += cs.y + ",";
                sendStr += cs.z + ",";
                sendStr += cs.eulY + ",";
                sendStr += cs.hp + ",";
            }

            FrmMain.Send(c, sendStr);

        }
        public static void MsgMove(ClientState c, string msgArgs)

        {

            string[] split = msgArgs.Split(',');

            string desc = split[0];

            c.x = float.Parse(split[1]);

            c.y = float.Parse(split[2]);

            c.z = float.Parse(split[3]);

            string sendStr = "Move|" + msgArgs;



            foreach (ClientState cs in FrmMain.clients.Values)

            {

                FrmMain.Send(cs, sendStr);

            }

        }
        public static void MsgAttack(ClientState c, string msgArgs)
        {
            //广播
            string sendStr = "Attack|" + msgArgs;
            foreach (ClientState cs in FrmMain.clients.Values)
            {
                FrmMain.Send(cs, sendStr);
            }
        }
        public static void MsgHit(ClientState c, string msgArgs)
        {
            //解析参数
            string[] split = msgArgs.Split(',');
            string attDesc = split[0];
            string hitDesc = split[1];
            //被攻击
            ClientState hitCS = null;

            foreach (ClientState cs in FrmMain.clients.Values)
            {
                if (cs.socket.RemoteEndPoint.ToString() == hitDesc)
                {
                    hitCS = cs;
                    break;
                }
            }

            if (hitCS == null)
                return;

            hitCS.hp -= 25;
            if (hitCS.hp <= 0)
            {
                string sendStr = "Die|" + hitCS.socket.RemoteEndPoint.ToString();
                foreach (ClientState cs in FrmMain.clients.Values)
                {
                    FrmMain.Send(cs, sendStr);
                }
            }

        }*/
    }

