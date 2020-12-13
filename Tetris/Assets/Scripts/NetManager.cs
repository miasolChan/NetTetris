using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System;
using System.Linq;
public static class NetManager
{
    public static Socket socket;

    //接收缓冲区
    static byte[] readBuff = new byte[1024];
    static int buffCount = 0;

    //委托类型
    public delegate void MsgListener(string str);
    
    static Dictionary<string,MsgListener> listeners = new Dictionary<string, MsgListener>();
    // 消息列表
    static List<string> msgList = new List<string>();

    //添加监听
    public static void AddListener(string msgName, MsgListener listener){
        listeners[msgName] = listener;
    }

    //获取描述
	public static string GetDesc(){

		if(socket == null) return "";

		if(!socket.Connected) return "";

		return socket.LocalEndPoint.ToString();

	}
    public static void Connection(string ip, int port)
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            socket.BeginConnect(ip, port, ConnectCallback, socket);
        }
        catch (FormatException)//格式检查
        {
            Debug.Log("输入IP或端口格式不合法");
            //str = "输入IP或端口格式不合法";
        }
        catch (SocketException)//连接检查
        {
            Debug.Log("输入IP或端口格式不合法");
            //str = "输入IP或端口格式不合法";
        }

    }
    //Connect回调
    public static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            socket.EndConnect(ar);
            Debug.Log("Socket Connect Succ ");
            //str = "连接成功"+"\n";
            socket.BeginReceive(readBuff, 0, 1024, 0,
                ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Connect fail" + ex.ToString());
            //str = "连接失败"+"\n";
        }
    }

    //Receive回调
    public static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            buffCount +=  socket.EndReceive(ar);
			Debug.Log("buffCount: "+ buffCount);
            //长度信息法，添加两字节长度信息
			if(buffCount<2){
				socket.BeginReceive( readBuff, buffCount, 1024-buffCount, 0,ReceiveCallback, socket);
				return;
			}
			int msgLen = BitConverter.ToInt16(readBuff, 0);
			if (!BitConverter.IsLittleEndian)
            {
                byte[] lenBytes = new byte[2];
                lenBytes[0] = readBuff[1];
                lenBytes[1] = readBuff[0];
                msgLen = BitConverter.ToInt16(lenBytes, 0);
            }
			Debug.Log("buffCount: "+ buffCount+ ", msgLen: "+ msgLen);
            if (buffCount < 2 + msgLen) {
				socket.BeginReceive( readBuff, buffCount, 1024-buffCount, 0,ReceiveCallback, socket);
				return;
			}

			string recvStr = System.Text.Encoding.Default.GetString(readBuff, 2, msgLen);
			buffCount = buffCount - 2 - msgLen;
			if(buffCount > 0)
				Array.Copy(readBuff, msgLen + 2, readBuff, 0, buffCount);
			Debug.Log(recvStr);
			msgList.Add(recvStr);
			socket.BeginReceive( readBuff, buffCount, 1024-buffCount, 0,ReceiveCallback, socket);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Receive fail" + ex.ToString());
        }
    }

    static Queue<ByteArray> writeQueue = new Queue<ByteArray>();
    public static void Send(string sendStr)
    {
        if(socket == null) return;
		if(!socket.Connected)return;

		byte[] bodyBytes = System.Text.Encoding.Default.GetBytes(sendStr);
		Int16 msgLen =(Int16) bodyBytes.Length;
		byte[] lenBytes =  BitConverter.GetBytes(msgLen);
		if (!BitConverter.IsLittleEndian)
		{
			lenBytes.Reverse();
		}
		byte[] sendBytes = lenBytes.Concat(bodyBytes).ToArray();


		ByteArray ba = new ByteArray(sendBytes);
        //加锁
        lock(writeQueue){
            writeQueue.Enqueue(ba);
			if( writeQueue.Count ==1){
				socket.BeginSend(sendBytes, 0, sendBytes.Length, 0, SendCallback, socket);
			}       
        }

    }
    //Send回调
    public static void SendCallback(IAsyncResult ar)
    {
        try
        {
            Socket socket = (Socket)ar.AsyncState;
            int count = socket.EndSend(ar);
            // string recvStr = System.Text.Encoding.Default.GetString(readBuff,0,count);
            // msgList.Add(recvStr);
            ByteArray ba;

			lock(writeQueue){
				ba = writeQueue.First();
			}
	        ba.readIdx += count;
	        if(ba.length == 0){
				lock(writeQueue){
					writeQueue.Dequeue();
					ba = writeQueue.First();
				}
	        }
			if(ba != null){
				socket.BeginSend(ba.bytes, ba.readIdx, ba.length, 0, SendCallback, socket);
			}
            Debug.Log("Socket Send succ " + count);
        }
        catch (SocketException ex)
        {
            Debug.Log("Socket Send fail" + ex.ToString());
        }

    }

    // Update is called once per frame
    public static void Update()
    {
        
        if(msgList.Count <= 0) return;

        string msg = msgList[0];
        msgList.RemoveAt(0);
        string[] split = msg.Split('|');
        string msgName = split[0];
        string msgArgs = split[1];
        //
        if(listeners.ContainsKey(msgName))
        {
            listeners[msgName](msgArgs);
        }
        
    }

}
