using System;
using System.Net;
using System.Net.Sockets;
using TetrisServer;


public class ClientState
{
    public Socket socket;
    public byte[] buffer = new byte[1024];
    public int buffCount = 0;

    public string desc;

    public string curIndexStr;
    public string nextIndexStr;

    public ByteArray sendBuffer = new ByteArray();//��������

    public bool isHost;//�Ƿ��Ƿ���
}