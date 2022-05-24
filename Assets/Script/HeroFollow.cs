using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFollow : MonoBehaviour
{
    // Start is called before the first frame update
    Transform playerTran;
    //[SerializeField]
    public Vector3 offset = new Vector3(0, 1, 0);//public使其可视化
    void Start()
    {
        playerTran = GameObject.Find("Hero").transform;//找英雄位置
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTran.position + offset;
    }
}
