using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public TileMap map;
    public static bool active = false;
    public static int range = 0;
    public static bool stab = false;
    async void OnMouseUp()
    {
        print("click");
        map.tiles[tileX,tileY] = 2;
        if(map.graph[tileX,tileY].inUse)
        {
            if((map.graph[tileX,tileY].chokePoint || map.graph[tileX,tileY].chokeAdjacent))
            {
                map.EditPath(0,0,map.selectedUnit.GetComponent<Unit>(),8,8);
            }
            else
            {
                map.EditPath(map.graph[tileX,tileY].predecessor.x, map.graph[tileX,tileY].predecessor.y, map.selectedUnit.GetComponent<Unit>(), map.graph[tileX,tileY].follower.x,map.graph[tileX,tileY].follower.y);
            }
        }
        
        /*if (active)
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
        }*/
    }
}