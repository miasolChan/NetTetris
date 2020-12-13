using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Reflection;

namespace TetrisServer
{
    public partial class FrmMain : Form
    {
        
        public static Dictionary<Socket, ClientState> clients = new Dictionary<Socket, ClientState>();
        bool bThreadIsRunning = false;
        public void AcceptClient(Socket server)
        {
            Socket client = server.Accept();
            ClientState state = new ClientState();
            state.socket = client;
            AppendText(client.RemoteEndPoint.ToString()+ " is enter!!");
            state.desc = client.RemoteEndPoint.ToString();
            clients.Add(client, state);
        }
        bool ReadClient(ClientState state)
        {
            Socket client = state.socket;
            int count = 0;
            try
            {
                count = client.Receive(state.buffer,state.buffCount,1024-state.buffCount,0);
            }
            catch (SocketException ex)
            {
                EventHandler.OnDisconnect(state);
                clients.Remove(client);
                client.Close();
                Console.WriteLine("Exception: " + ex.ToString());
                return false;
            }
            if (count == 0)
            {
                //删除这个客户端
                EventHandler.OnDisconnect(state);
                clients.Remove(client);
                client.Close();
                Console.WriteLine("Socket Close");
                return false;
            }
            //解析长度信息
            state.buffCount += count;
            if (state.buffCount < 2) return true;
            int msgLen = BitConverter.ToInt16(state.buffer, 0);
            if (!BitConverter.IsLittleEndian)
            {
                byte[] lenBytes = new byte[2];
                lenBytes[0] = state.buffer[1];
                lenBytes[1] = state.buffer[0];
                msgLen = BitConverter.ToInt16(lenBytes, 0);
            }
            if (state.buffCount < 2 + msgLen) return true;

            string readStr = System.Text.Encoding.UTF8.GetString(state.buffer, 2, msgLen);
            state.buffCount = state.buffCount - 2 - msgLen;
            if (state.buffCount > 0)
                Array.Copy(state.buffer, msgLen + 2, state.buffer, 0, state.buffCount);
            
            string[] msg = readStr.Split('|');
            string msgName = msg[0];
            string msgArgs = msg[1];

            MethodInfo mi = typeof(MsgHandler).GetMethod("Msg"+msgName);

            if(mi!=null)
            {
                mi.Invoke(null,new object[]{ state,msgArgs});
            }
           
            AppendText("[" + ((IPEndPoint)client.RemoteEndPoint).Address.ToString() + "]:" + readStr);
            return true;
        }
        public FrmMain()
         {
            InitializeComponent();
         }


        private void btnStartSvr_Click(object sender, EventArgs e)
        {
            bThreadIsRunning = !bThreadIsRunning;
            if (bThreadIsRunning)
            {
                Thread t = new Thread(ReceiveClientData);
                t.Start(this);
                btnStartSvr.Text = "停止服务器";
            }
            else
            {
                btnStartSvr.Text = "启动服务器";
            }
            //btnStartSvr.Enabled = !bThreadIsRunning;
        }

        //线程操作窗体中的rtbChatContent，需要用委托去实现
        delegate void AppendTextCallback(string text);
        private void AppendText(string text)
        {
            text += "\n";
            if(rtbChatContent.InvokeRequired)
            {
                AppendTextCallback callback = new AppendTextCallback(rtbChatContent.AppendText);
                this.Invoke(callback, new object[] { text });
            }
            else
                rtbChatContent.AppendText(text);            
        }

        private  void ReceiveClientData(object data)
        {
            Socket listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //检测输入IP与端口合法性
            IPAddress ip;
            IPEndPoint addr;
            int port;
            try
            {
                ip = IPAddress.Parse(tbxSvrIP.Text);
                port = int.Parse(tbxPort.Text);
                addr = new IPEndPoint(ip, port);
                listenfd.Bind(addr);
                listenfd.Listen(0);
            }
            catch (FormatException)//格式检查
            {
                AppendText("输入IP或端口格式不合法");
            }
            catch (SocketException)//连接检查
            {
                AppendText("输入IP或端口格式不合法");
            }

            AppendText("Server is running...");

            List<Socket> checkRead = new List<Socket>();
            //List<Socket> clientsKey = new List<Socket>(clients.Keys);

            while (bThreadIsRunning)
            {
                checkRead.Clear();
                checkRead.Add(listenfd);
                //for (int i = 0; i < clientsKey.Count; i++)
                //{
                //    Socket key = clientsKey[i];
                //    ClientState item = clients[key];
                //    checkRead.Add(item.socket);
                //}
                foreach (ClientState item in clients.Values)
                {
                    checkRead.Add(item.socket);
                }
                Socket.Select(checkRead, null, null, 50000);
                foreach (Socket socket in checkRead)
                {
                    if (socket == listenfd)
                        AcceptClient(listenfd);
                    else
                        ReadClient(clients[socket]);
                }
            }

            //线程要退出了，需要回收各种资源
            foreach (ClientState state in clients.Values)
            {
                state.socket.Close();
            }
            listenfd.Close();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //主窗体退出了，因此线程也需要退出
            bThreadIsRunning = false;
        }


        public static void Send(ClientState cs, string msg)
        {
            try
            {
                byte[] bodyBytes = System.Text.Encoding.UTF8.GetBytes(msg);
                Int16 len = (Int16)bodyBytes.Length;
                byte[] lenBytes = BitConverter.GetBytes(len);
                if (!BitConverter.IsLittleEndian)
                {
                    lenBytes.Reverse();
                }
                byte[] sendBytes = lenBytes.Concat<byte>(bodyBytes).ToArray();

                cs.sendBuffer.Write(sendBytes, 0, sendBytes.Length);

                int ret = cs.socket.Send(cs.sendBuffer.bytes, cs.sendBuffer.readIdx, cs.sendBuffer.length,0);
                if(ret >=0 )
                {
                    cs.sendBuffer.readIdx += ret;
                    cs.sendBuffer.CheckAndMoveBytes();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch(NullReferenceException ex)
            {
                //Console.WriteLine(ex.ToString());
            }
            
        }
    }
}
