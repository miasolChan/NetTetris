using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int[,] shape
    {
        private set;
        get;
    }
    public enum BlockType {I,J,L,O,S,Z,T,U,H,A,Y}
    public BlockType type;
    public Color blockColor = new Color(0,0,0,187);

    private int curRotationState = 0;
    private Vector2 curPos = new Vector2(3,Map.mapRow - 1);

    public void InitShape()
    {
        switch(type)
        {
            case BlockType.I:
                shape = new int[4,16]
                {
                    {0,0,0,0,
                     1,1,1,1,
                     0,0,0,0,
                     0,0,0,0},

                     {0,1,0,0,
                      0,1,0,0,
                      0,1,0,0,
                      0,1,0,0 },

                     {0,0,0,0,
                      1,1,1,1,
                      0,0,0,0,
                      0,0,0,0},

                     {0,1,0,0,
                      0,1,0,0,
                      0,1,0,0,
                      0,1,0,0 },   
                };
                break;
            case BlockType.J:
                shape = new int[4,16]
                {
                    {0,2,0,0,
                     0,2,0,0,
                     2,2,0,0,
                     0,0,0,0},

                    {2,2,2,0,
                     0,0,2,0,
                     0,0,0,0,
                     0,0,0,0},

                    {2,2,0,0,
                     2,0,0,0,
                     2,0,0,0,
                     0,0,0,0},

                    {2,0,0,0,
                     2,2,2,0,
                     0,0,0,0,
                     0,0,0,0},
                };
                break;
            case BlockType.L:
                shape = new int[4,16]
                {
                    {3,0,0,0,
                        3,0,0,0,
                        3,3,0,0,
                        0,0,0,0},

                    {0,0,3,0,
                        3,3,3,0,
                        0,0,0,0,
                        0,0,0,0},

                    {3,3,0,0,
                        0,3,0,0,
                        0,3,0,0,
                        0,0,0,0},

                        {3,3,3,0,
                        3,0,0,0,
                        0,0,0,0,
                        0,0,0,0},
                };
                break;
            case BlockType.O:
                shape = new int[4,16]
                {
                    {4,4,0,0,
                        4,4,0,0,
                        0,0,0,0,
                        0,0,0,0},

                    {4,4,0,0,
                        4,4,0,0,
                        0,0,0,0,
                        0,0,0,0},

                    {4,4,0,0,
                        4,4,0,0,
                        0,0,0,0,
                        0,0,0,0},

                        {4,4,0,0,
                        4,4,0,0,
                        0,0,0,0,
                        0,0,0,0},
                };
                break;
            case BlockType.S:
                shape = new int[4,16]
                {
                    {0,5,5,0,
                        5,5,0,0,
                        0,0,0,0,
                        0,0,0,0},

                    {5,0,0,0,
                        5,5,0,0,
                        0,5,0,0,
                        0,0,0,0},

                    {0,5,5,0,
                        5,5,0,0,
                        0,0,0,0,
                        0,0,0,0},

                        {5,0,0,0,
                        5,5,0,0,
                        0,5,0,0,
                        0,0,0,0},
                };
                break;
            case BlockType.Z:
                shape = new int[4,16]
                {
                    {6,6,0,0,
                        0,6,6,0,
                        0,0,0,0,
                        0,0,0,0},

                    {0,6,0,0,
                        6,6,0,0,
                        6,0,0,0,
                        0,0,0,0},

                    {6,6,0,0,
                        0,6,6,0,
                        0,0,0,0,
                        0,0,0,0},

                        {0,6,0,0,
                        6,6,0,0,
                        6,0,0,0,
                        0,0,0,0},
                };
                break;
            case BlockType.T:
                shape = new int[4,16]
                {
                    {0,7,0,0,
                        7,7,7,0,
                        0,0,0,0,
                        0,0,0,0},

                    {0,7,0,0,
                        7,7,0,0,
                        0,7,0,0,
                        0,0,0,0},

                    {7,7,7,0,
                        0,7,0,0,
                        0,0,0,0,
                        0,0,0,0},

                        {0,7,0,0,
                        0,7,7,0,
                        0,7,0,0,
                        0,0,0,0},
                };
                break;

            case BlockType.U:
                shape = new int[4,16]
                {
                    {0,0,0,0,
                    9,0,9,0,
                    9,0,9,0,
                    9,9,9,0},

                    {0,0,0,0,
                    9,9,9,0,
                    0,0,9,0,
                    9,9,9,0},

                    {0,0,0,0,
                    9,9,9,0,
                    9,0,9,0,
                    9,0,9,0},

                    {0,0,0,0,
                    9,9,9,0,
                    9,0,0,0,
                    9,9,9,0},
                };
                break;

            case BlockType.H:
                shape = new int[4,16]
                {
                    {0,0,0,0,
                    10,0,10,0,
                    10,10,10,0,
                    10,0,10,0},

                    {0,0,0,0,
                    10,10,10,0,
                    0,10,0,0,
                    10,10,10,0},

                    {0,0,0,0,
                    10,0,10,0,
                    10,10,10,0,
                    10,0,10,0},

                    {0,0,0,0,
                    10,10,10,0,
                    0,10,0,0,
                    10,10,10,0},
                };
                break;
            case BlockType.A:
                shape = new int[4,16]
                {
                    {0,0,0,0,
                    0,0,11,0,
                    0,11,11,0,
                    0,0,0,0},

                    {0,0,0,0,
                    0,11,11,0,
                    0,0,11,0,
                    0,0,0,0},

                    {0,0,0,0,
                    0,11,11,0,
                    0,11,0,0,
                    0,0,0,0},

                    {0,0,0,0,
                    0,11,0,0,
                    0,11,11,0,
                    0,0,0,0},
                };
                break;

                case BlockType.Y:
                shape = new int[4,16]
                {
                    {0,0,0,0,
                    0,0,12,0,
                    0,12,0,0,
                    12,0,0,0},

                    {0,0,0,0,
                    12,0,0,0,
                    0,12,0,0,
                    0,0,12,0},

                    {0,0,0,0,
                    0,0,12,0,
                    0,12,0,0,
                    12,0,0,0},

                    {0,0,0,0,
                    12,0,0,0,
                    0,12,0,0,
                    0,0,12,0},
                };
                break;


                    
                        

            }
                        
        }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        InitShape();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2 GetCurPos()
    {
        return curPos;
    }
    public Vector2 RotateBlock()
    {
        curRotationState = (curRotationState + 1) % 4;
        return curPos;
    }
    public Vector2 InverseRotateBlock()
    {
        curRotationState = (curRotationState + 3) % 4;
        return curPos;
    }
    public Vector2 MoveLeft()
    {
        curPos.x --;
        return curPos;
    }
    public Vector2 MoveRight()
    {
        curPos.x++;
        return curPos;
    }
    public Vector2 MoveDown()
    {
        curPos.y--;
        return curPos;
    }
    public Vector2 MoveUp()
    {
        curPos.y++;
        return curPos;
    }

    public int GetBlockRotateState()
    {
        return curRotationState;
    }
}
