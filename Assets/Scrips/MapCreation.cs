using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreation : MonoBehaviour
{
    //用来装初始化地图所需物体的数组
    //0.home 1.wall 2.barrier 3.born effect 4.river 5.grass 6.air barrier
    //wall可以被子弹击破，而barrier不行
    public GameObject[] item;
    private List<Vector3> itemPositionList = new List<Vector3>();//已经有物体的位置列表
    private void Awake()
    {
        InitMap();
    }
    private void InitMap()
    {

        //实例化老家在地图中间下方
        CreateItem(item[0], new Vector3(0, -8, 0), Quaternion.identity);
        //用墙把老家围起来
        CreateItem(item[1], new Vector3(-1, -8, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -8, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(-1, -7, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(0, -7, 0), Quaternion.identity);
        CreateItem(item[1], new Vector3(1, -7, 0), Quaternion.identity);

        //实例化外围墙
        for (int i = -20; i <= 20; i++)
        {
            CreateItem(item[6], new Vector3(i, 9, 0), Quaternion.identity);
            CreateItem(item[6], new Vector3(i, -9, 0), Quaternion.identity);
        }
        for (int i = -8; i <= 8; i++)
        {
            CreateItem(item[6], new Vector3(-20, i, 0), Quaternion.identity);
            CreateItem(item[6], new Vector3(20, i, 0), Quaternion.identity);

        }
        //示例化草障碍河流各25格，墙80格
        for (int i = 0; i <= 80; i++)
        {
            CreateItem(item[1], CreateRandomPosition(), Quaternion.identity);

        }
        for (int i = 0; i <= 25; i++)
        {
            CreateItem(item[2], CreateRandomPosition(), Quaternion.identity);

        }
        for (int i = 0; i <= 25; i++)
        {
            CreateItem(item[4], CreateRandomPosition(), Quaternion.identity);

        }
        for (int i = 0; i <= 25; i++)
        {
            CreateItem(item[5], CreateRandomPosition(), Quaternion.identity);

        }
        //初始化玩家
        GameObject go = Instantiate(item[3], new Vector3(-2, -8, 0), Quaternion.identity);
        go.GetComponent<born>().createPlayer = true;
        //产生敌人
        Instantiate(item[3], new Vector3(-19, 8, 0), Quaternion.identity);
        Instantiate(item[3], new Vector3(0, 8, 0), Quaternion.identity);
        Instantiate(item[3], new Vector3(19, 8, 0), Quaternion.identity);

        InvokeRepeating("createEnemy", 4, 5);

    }
    //采用封装的方法
    private void CreateItem(GameObject createGameObject, Vector3 CreatePosition, Quaternion createRotation)
    {//该方法让实例化产生的物体依附于mapcreation而不散落在game栏
        GameObject itemgo = Instantiate(createGameObject, CreatePosition, createRotation);
        itemgo.transform.SetParent(gameObject.transform);
        itemPositionList.Add(CreatePosition);
    }
    //产生随机位置的方法
    private Vector3 CreateRandomPosition()
    {
        //地图边缘不产生物体，否则可能产生无法通关的地图
        while (true)
        {
            //产生一个随机位置
            Vector3 createPosition = new Vector3(Random.Range(-18, 19), Random.Range(-7, 8), 0);
            //如果随机位置不在列表中,就返回
            if (!HasThePosition(createPosition))
                return createPosition;
        }

    }
    private bool HasThePosition(Vector3 createpos)
    {
        for (int i = 0; i < itemPositionList.Count; i++)
        {
            if (createpos == itemPositionList[i])
                return true;
        }
        return false;
    }
    private void createEnemy()
    {
        int num = Random.Range(0, 3);
        if (num == 0)
        {
            Instantiate(item[3], new Vector3(-19, 8, 0), Quaternion.identity);
        }
        else if (num == 1)
        {
            Instantiate(item[3], new Vector3(0, 8, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(item[3], new Vector3(19, 8, 0), Quaternion.identity);
        }
    }
}
