using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonSpawner : MonoBehaviour
{
    public GameObject demonPrefab;
    public static EnemyController recentDemon = null;
    public Unit referenceUnit;
    public TileMap map;
    float timer = 3f;
    private bool spawned = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3f)
        {
            timer = 0f;
            Instantiate(demonPrefab, new Vector3(0.66f,0,-2),Quaternion.identity);
            spawned = true;
        }
        else if(spawned)
        {
            print("RecentDemon: "+recentDemon);
            recentDemon.referenceUnit = referenceUnit;
            recentDemon.map = map;
            map.enemies.Add(recentDemon);
            spawned = false;
        }
    }
    
    public static void UpdateDemon(EnemyController given)
    {
        recentDemon = given;
    }
}
