using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    //初始坐标偏移量
    protected int offsetRaw = 0;
    protected int offsetCol = 0;
    public static int mapRow = 20;
    public static int mapCol = 12;
    public GameObject blockFrame;
    public GameObject blockWall;
    private int[,] mapSnapshot;
    private GameObject[,] backgroundObjs;

    private GameObject textRoot;
    public TextMesh mapText;
    private TextMesh[,] mapInfoTexts;

    public Text ScoreText;
    public static int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        //ScoreText = GameObject.Find("Score").GetComponent<Text>();
        ScoreText.text = "Score: "+ score;
        InitOffset();
        InitMap();
    }

    //初始坐标
    public void InitOffset(){
        
    }
    public void InitMap()
    {
        backgroundObjs = new GameObject[mapRow,mapCol];
        mapSnapshot = new int [mapRow,mapCol];
        for(int row = 0 ; row < mapRow ; row++)
        {
            for (int col = 0 ; col <mapCol ; col++)
            {
                //墙体
                if(col == 0 || col == mapCol-1 || row == 0)
                {
                    mapSnapshot[row,col] = -1;
                    backgroundObjs[row,col] = Instantiate(blockWall,new Vector2(col+offsetCol,row+offsetRaw),Quaternion.identity);
                    backgroundObjs[row,col].name = "Wall" + row + " " +col;
                }
                //空位
                else
                {
                    mapSnapshot[row,col] = 0;
                    backgroundObjs[row,col] = Instantiate(blockFrame , new Vector2(col+offsetCol,row+offsetRaw),Quaternion.identity);
                    backgroundObjs[row,col].name = "Frame" + row + " " +col;
                }
                backgroundObjs[row,col].transform.SetParent(transform);
            }
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //ShowMapInfo();
        ScoreText.text = "Score: "+ score;
    }
    public void ShowMapInfo()
    {
        if(mapInfoTexts == null)
        {
            mapInfoTexts = new TextMesh[mapRow,mapCol];
            textRoot = new GameObject("TextRoot");
        }
        for(int row = 0 ; row < mapRow ; row++)
        {
            for(int col = 0 ; col < mapCol ;col++)
            {
                if(mapInfoTexts[row,col] == null)
                {
                    mapInfoTexts[row,col] = Instantiate(mapText,new Vector3(col,row ,0),Quaternion.identity);
                    mapInfoTexts[row,col].transform.SetParent(textRoot.transform);

                }
                mapInfoTexts[row,col].text = mapSnapshot[row,col].ToString();
            }
        }
    }
     public void ClearMap()
    {
        for(int row = 0 ; row < mapRow ; row++)
        {
            for(int col = 0 ; col < mapCol ; col++)
            {
                if(mapSnapshot[row,col] != -1 && mapSnapshot[row,col] != 8)
                {
                    mapSnapshot[row,col] = 0;
                    backgroundObjs[row,col].GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1,0.2f);
                }
            }
        }
    }

    public void SetMapInfo(int row ,int col ,int flag , Color blockColor)
    {
        if(mapSnapshot == null)
        {
            return;
        }
        mapSnapshot[row,col] = flag;
        backgroundObjs[row,col].GetComponentInChildren<SpriteRenderer>().color = blockColor;
    }


    public int GetMapInfo(int row , int col)
    {
        return mapSnapshot[row,col];
    }
    public bool DetectFullRow()
        {
            for(int fullCol = 1;fullCol<mapCol-1;fullCol++)
            {
                if(mapSnapshot[mapRow-1,fullCol]==8)
                {
                return true;
                }
            }
            return false;
        }

    public void SetFixedItem(int row,int col)
    {
        mapSnapshot[row,col]=8;
    }
    public void DetectLine()
    {
        int count = 0;
        for(int row = 1;row<mapRow - 1;row++)
        {
            count = 0 ;
            for(int col=1;col<mapCol -1;col++)
            {
                if(mapSnapshot[row,col]==8)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            if(count == mapCol - 2)
            {
                Debug.Log("Whole Line"+row);
                DelectLine(row);
                score++;
                
            }
        }
    }
    public void DelectLine(int delectRow)
    {
        for(int row = delectRow;row<mapRow-1;row++)
        {
            for(int col=1;col<mapCol-1;col++)
            {
                mapSnapshot[row,col]=mapSnapshot[row+1,col];
                backgroundObjs[row,col].GetComponent<SpriteRenderer>().color = 
                backgroundObjs[row+1,col].GetComponent<SpriteRenderer>().color;
            }
        }
        DetectLine();
    }
    public int getScore(){
        return score;
    }
}
