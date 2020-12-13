using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class localGame : BaseGame
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBlocks();
    }

    public void GameStart(){
        CreateBlocks();
        Debug.Log("游戏开始");
    }

    public void PrepareBtn(){
        NetManager.Send("Prepare" + "|"+NetManager.GetDesc());
    }
}
