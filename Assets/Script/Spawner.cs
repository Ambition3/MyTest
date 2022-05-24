using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] enemies;
    void Start()
    {
        InvokeRepeating("SpawnEnemy", 2, 1);//程序运行两秒后，每隔1秒调用一次
    }
    void SpawnEnemy()
    {
        int index = Random.Range(0, 1);
        Instantiate(enemies[index], transform.position, transform.localRotation);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
