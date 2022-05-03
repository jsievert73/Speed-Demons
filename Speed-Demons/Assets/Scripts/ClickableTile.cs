using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;
    public TowerType[] TowerTypes;
    public static bool active = false;
    public static int range = 0;
    public static bool stab = false;
    private float cooldown = 0.5f;

    async void OnMouseDown()
    {
        print("towerTileType: " + map.tiles[tileX,tileY]);
        if(map.tiles[tileX,tileY]==3)
        {
            cooldown = 0;
            print("Found Tower Tile");
            map.tiles[tileX,tileY]=1;
            map.towers += 1;
            map.tower[tileX,tileY].SetActive(false);
            //delete tower
        }
    }
    void Update()
    {
        if(cooldown < 0.5f)
        {
            cooldown += Time.deltaTime;
        }
    }
    async void OnMouseUp()
    {
        print("click");
        
        if(map.tiles[tileX,tileY] < 2 && map.towers != 0 && cooldown >= 0.5f)
        {
            map.tiles[tileX,tileY] = 3;
            map.towers -= 1;
            TowerType tt = TowerTypes [0];
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);
            GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(tileX,tileY,-0.5f), rotation);
            map.tower[tileX,tileY] = go;
            //if(map.graph[tileX,tileY].inUse)
            //{
                if((map.graph[tileX,tileY].chokePoint || map.graph[tileX,tileY].chokeAdjacent))
                {
                    map.EditPath(0,0,map.selectedUnit.GetComponent<Unit>(),8,8);
                }
                else
                {
                    map.EditPath(map.graph[tileX,tileY].predecessor.x, map.graph[tileX,tileY].predecessor.y, map.selectedUnit.GetComponent<Unit>(), map.graph[tileX,tileY].follower.x,map.graph[tileX,tileY].follower.y);
                }
            //}
        }
        
        if (active)
        {
            for (int x = 1; x < range+1;x++)
            {
                if (tileX+x == map.selectedUnit.GetComponent<Unit>().tileX || tileX-x == map.selectedUnit.GetComponent<Unit>().tileX || tileX == map.selectedUnit.GetComponent<Unit>().tileX)
                {
                    for(int y=1; y<range+1; y++)
                    {
                        if (tileY+y == map.selectedUnit.GetComponent<Unit>().tileY || tileY-y == map.selectedUnit.GetComponent<Unit>().tileY)
                        {
                            if (stab && map.graph[tileX, tileY].housingUnit!=null)
                            {
                                map.GeneratePathTo(tileX, tileY,map.selectedUnit.GetComponent<Unit>());
                                active = false;
                                return;
                            }
                            if (!stab && map.graph[tileX, tileY].housingUnit==null)
                            {
                                map.GeneratePathTo(tileX, tileY,map.selectedUnit.GetComponent<Unit>());
                                active = false;
                                return;
                            }
                        }
                        else if (tileY == map.selectedUnit.GetComponent<Unit>().tileY && tileX != map.selectedUnit.GetComponent<Unit>().tileX)
                        {
                            map.GeneratePathTo(tileX, tileY,map.selectedUnit.GetComponent<Unit>());
                            active = false;
                            return;
                        }
                    }
                }
            }
            Debug.Log("Invalid Selection");
        }
    }
}