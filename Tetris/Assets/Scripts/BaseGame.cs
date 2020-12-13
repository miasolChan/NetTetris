using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseGame : MonoBehaviour
{
    public Block[] blocks;
    private Block curBlock;
    private Block nextBlock;
    private bool isFirst = true;

    public Map mapSnapShot;
    public bool isGameOver=false;
    public float moveDownSpeed = 1f;
    private float timeCount=0;
    private GameObject[,] nextShowBlocks;
    public GameObject nextShowBlockFrame;
    public GameObject Over;

    public GameController gameController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        MoveBlocks();
        ChangeSpeed();
        
    }

    public void CreateBlocks()
    {
        if(isFirst == true)
        {
            int curBlockIndex = Random.Range(0,blocks.Length);
            curBlock = Instantiate(blocks[curBlockIndex]);
            isFirst = false;
            int nextBlockIndex = Random.Range(0,blocks.Length);
            nextBlock = Instantiate(blocks[nextBlockIndex]);
            Debug.Log(curBlock.name + " " + nextBlock.name);
        }
        else
        {
            Destroy(curBlock.gameObject);
            curBlock = nextBlock;
            nextBlock = Instantiate(blocks[Random.Range(0,blocks.Length)]);
        }
        ShowNextBlock();
        SetBlockToMap(curBlock.GetCurPos());
    }
    public void MoveBlocks()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
           MoveCurBlockLeft();
           NetManager.Send("Left" + "|"+NetManager.GetDesc());//左移动协议
           
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveCurBlockright();
            NetManager.Send("Right" + "|"+NetManager.GetDesc());//右移动协议
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCurBlockDown();
            NetManager.Send("Down" + "|"+NetManager.GetDesc());//下移动协议
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateCurBlock();
            NetManager.Send("Up" + "|"+NetManager.GetDesc());//变化（上键）移动协议
        }
        if(TimeCountDown()==true)
       {
           MoveCurBlockDown();
           NetManager.Send("Down" + "|"+NetManager.GetDesc());//下移动协议
        }
    }
    private void MoveCurBlockLeft()
    {
        curBlock.MoveLeft();
        if(!CanMoveBlock (curBlock.GetCurPos()))
        {
            curBlock.MoveRight();
            return;
        }
        UpdateMap();
    }
    private void MoveCurBlockright()
    {
        curBlock.MoveRight();
        if(!CanMoveBlock (curBlock.GetCurPos()))
        {
            curBlock.MoveLeft();
            return;
        }
        UpdateMap();
    }
    private void MoveCurBlockDown()
    {
        curBlock.MoveDown();
        if(!CanMoveBlock (curBlock.GetCurPos()))
        {
            curBlock.MoveUp();
            SetFixedBlockAtMap(curBlock.GetCurPos());
            if(mapSnapShot.DetectFullRow()==true)
            {
                Debug.Log("Game Over");
                GameOver();
                return;
            }
            mapSnapShot.DetectLine();
            CreateBlocks();
            return;
        }
        UpdateMap();
    }
    private void RotateCurBlock()
    {
        curBlock.RotateBlock();
        if(!CanMoveBlock (curBlock.GetCurPos()))
        {
            curBlock.InverseRotateBlock();
            return;
        }
        UpdateMap();
    }
    bool CanMoveBlock(Vector2 curBlockPos)
    {
        for(int blockRow = 0 ; blockRow < 4 ; blockRow++)
        {
            for(int blockCol = 0 ; blockCol < 4 ; blockCol++)
            {
                if(curBlock.shape[curBlock.GetBlockRotateState(),blockRow*4 + blockCol] != 0)
                {
                    if(HitWall(blockRow,blockCol,curBlockPos)||HitOtherBlock(curBlockPos)||ArriveBottom())
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    void SetBlockToMap(Vector2 curBlockPos)
    {
        for(int blockRow = 0 ; blockRow < 4 ; blockRow++)
        {
            for(int blockCol = 0 ; blockCol < 4 ; blockCol++)
            {
                if(curBlock.shape[curBlock.GetBlockRotateState(),blockRow*4 + blockCol] != 0)
                {
                    mapSnapShot.SetMapInfo((int)curBlockPos.y - blockRow,blockCol + (int)curBlockPos.x,curBlock.shape[curBlock.GetBlockRotateState(),blockRow * 4 + blockCol],curBlock.blockColor);

                }
            }
        }
    }
    bool HitOtherBlock(Vector2 curPos)
    {
        for(int blockRow = 0 ; blockRow < 4 ; blockRow++)
        {
            for(int blockCol = 0 ; blockCol < 4 ; blockCol++)
            {
                if(curBlock.shape[curBlock.GetBlockRotateState(),blockRow*4 + blockCol] != 0 && mapSnapShot.GetMapInfo((int)curPos.y - blockRow,(int)curPos.x + blockCol) == 8)
                {
                    return true;
                }
            }
        }
        return false;
    }
    bool HitWall(int blockRow, int blockCol,Vector2 curPos)
    {
        if(mapSnapShot.GetMapInfo((int)curPos.y - blockRow,(int)curPos.x + blockCol) == -1)
        {
            return true;
        }
        return false;
    }
    bool ArriveBottom()
    {
        for(int blockRow = 0; blockRow< 4 ; blockRow++)
        {
            for(int blockCol = 0 ; blockCol < 4 ; blockCol++)
            {
                if(curBlock.shape[curBlock.GetBlockRotateState(),blockRow * 4 + blockCol]!= 0)
                {
                    if(curBlock.GetCurPos().y - blockRow <= 0)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void SetFixedBlockAtMap(Vector2 curPos)
    {
        for (int blockRow = 0;blockRow < 4;blockRow++)
        {
            for(int blockCol=0;blockCol<4;blockCol++)
            {
                if (curBlock.shape[curBlock.GetBlockRotateState(),blockRow * 4 + blockCol] !=0)
                {
                mapSnapShot.SetFixedItem((int)curPos.y - blockRow,blockCol+(int)curPos.x);       
                }
            }
        }
    }
    bool TimeCountDown()
    {
        timeCount += Time.deltaTime;
        if(timeCount>moveDownSpeed)
        {
            timeCount=0;
            return true;
        }
        return false;
    }

    void ShowNextBlock()
    {
        if(nextShowBlocks==null)
        {
            nextShowBlocks=new GameObject[4,4];
        }
        for(int row=0;row<4 ;row++)
        {
            for(int col=0;col<4;col++)
            {
                if (nextShowBlocks[row,col]==null)
                {
                    nextShowBlocks[row,col]=Instantiate(nextShowBlockFrame,new Vector3(col+15,-row+10,0),Quaternion.identity);
                }
                nextShowBlocks[row,col].GetComponent<SpriteRenderer>().color=new Color(0,0,0,0);
                if(nextBlock.shape[nextBlock.GetBlockRotateState(),row*4+col]!=0)
                {
                    nextShowBlocks[row,col].GetComponent<SpriteRenderer>().color=nextBlock.blockColor;
                }
            }
        }
    }

    void UpdateMap()
    {
        mapSnapShot.ClearMap();
        SetBlockToMap(curBlock.GetCurPos());
       // mapSnapShot.ShowMapInfo();
    }

    void ChangeSpeed(){
       
        int tScore = mapSnapShot.getScore();
        if(tScore > 10) moveDownSpeed = 0.2f;
        else if (tScore >8) moveDownSpeed = 0.4f;
        else if (tScore >6) moveDownSpeed = 0.6f;
        else if (tScore >2) moveDownSpeed = 0.8f;
        else moveDownSpeed = 1f;

    }
    void GameOver(){
        Over.SetActive(true);
        isGameOver=true;
        curBlock=null;
    }
}
