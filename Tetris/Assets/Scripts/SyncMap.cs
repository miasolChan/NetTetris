using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMap : Map
{

    //重载偏移坐标
    public void InitOffset(){
        offsetRaw = 0;
        offsetCol = 30;
    }
    // Start is called before the first frame update
    void Start()
    {
        InitOffset();
        InitMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
