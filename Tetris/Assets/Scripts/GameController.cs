using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour
{
    void Start(){

        NetManager.AddListener("Prepare",OnPrepare);
        NetManager.AddListener("Down",OnDown);
        NetManager.AddListener("Left",OnLeft);
        NetManager.AddListener("Right",OnRight);
        NetManager.AddListener("Change",OnChange);
        NetManager.Connection("127.0.0.1",8888);
    }

    public void GameStart(){
        // GameObject gameObject = new GameObject();
        // BaseGame baseGame = gameObject.AddComponent<BaseGame>();

    }
    public void OnPrepare(string msg){
        //string msg = "Prepare" + "|"+NetManager.GetDesc();//准备协议
        NetManager.Send(msg);
    }
    public void OnDown(string msg){
        Debug.Log("OnDown" + msg);
        NetManager.Send(msg);
    }

    public void OnLeft(string msg){
        Debug.Log("OnLeft" + msg);
    }
    public void OnRight(string msg){
        Debug.Log("OnRight" + msg);
    }
    public void OnChange(string msg){
        Debug.Log("onChange" + msg);
    }
    
}
