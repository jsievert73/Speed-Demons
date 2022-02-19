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
    void OnMouseUp()
    {
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