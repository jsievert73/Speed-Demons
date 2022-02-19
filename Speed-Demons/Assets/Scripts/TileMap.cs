using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit;
    public GameObject player;
    public TileType[] tileTypes;
    int[,] tiles;
    public Node[,] graph;
    public int enemyCount;

    int mapSizeX = 10;
    int mapSizeY = 10;

    void Start() 
    {
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisuals();
    }
    void GenerateMapData() 
    {
        //Allocate our map tiles
        tiles = new int[mapSizeX, mapSizeY];
        //Initalize our map tiles.
        for (int x =0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                tiles[x,y]=0;
            }
        }

        //Swampy Area
        for (int x= 3; x <= 5; x++)
        {
            for (int y=0; y <4; y++)
            {
                tiles [x,y] = 1;
            }
        }

        // Let's make a u-shaped mountain range
        tiles[4, 4] = 2;
        tiles[5, 4] = 2;
        tiles[6, 4] = 2;
        tiles[7, 4] = 2;
        tiles[8, 4] = 2;
        tiles[4, 5] = 2;
        tiles[4, 6] = 2;
        tiles[8, 5] = 2;
        tiles[8, 6] = 2;
    }

    public float CostToEnterTile(int x, int y)
    {
        TileType tt = tileTypes[tiles[x,y]];
        if (tt.isWalkable == false)
        {
            return Mathf.Infinity;
        }
        return tt.movementCost;
    }

    

    void GeneratePathfindingGraph()
    {
        graph = new Node[mapSizeX, mapSizeY];
        for (int x =0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                graph[x,y] = new Node();
                graph[x,y].x = x;
                graph[x,y].y = y;
            }
        }
        for (int x =0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                bool left = false;
                bool right = false;
                if (x != 0)
                {
                    graph[x,y].neighbours.Add(graph[x-1,y]);
                    left =true;
                }
                if (x < mapSizeX-1)
                {
                    graph[x,y].neighbours.Add(graph[x+1,y]);
                    right = true;
                }
                if (y != 0)
                {
                    graph[x,y].neighbours.Add(graph[x,y-1]);
                    if (left)
                    {
                        graph[x,y].neighbours.Add(graph[x-1,y-1]);
                    }
                    if (right)
                    {
                        graph[x,y].neighbours.Add(graph[x+1,y-1]);
                    }
                }
                if (y < mapSizeX-1)
                {
                    graph[x,y].neighbours.Add(graph[x,y+1]);
                    if (left)
                    {
                        graph[x,y].neighbours.Add(graph[x-1,y+1]);
                    }
                    if (right)
                    {
                        graph[x,y].neighbours.Add(graph[x+1,y+1]);
                    }
                }
            }
        }
    }

    void GenerateMapVisuals() 
    {
        for (int x =0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeX; y++)
            {
                TileType tt = tileTypes [tiles[x,y]];
                GameObject go = (GameObject)Instantiate(tt.tileVisualPrefab, new Vector3(x,y,0), Quaternion.identity);
                ClickableTile ct = go.GetComponent<ClickableTile>();
                ct.tileX = x;
                ct.tileY = y;
                ct.map = this;
            }
        }
    }

    public Vector3 TileCoordToWorldCoord(int x, int y)
    {
        return new Vector3(x,y,0);
    }

    public bool UnitCanEnterTile(int x, int y)
    {
        //could even use this for checking terrain flags, like flying versus walking
        return tileTypes[tiles[x,y]].isWalkable;
    }

    public void GeneratePathTo(int x, int y, Unit piece)
    {
        piece.GetComponent<Unit>().currentPath = null;

        if(UnitCanEnterTile(x,y) == false)
        {
            //quits out because no path should be found.
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        Node source = graph[piece.GetComponent<Unit>().tileX,piece.GetComponent<Unit>().tileY];

        Node target = graph[x,y];

        dist[source]=0;
        prev[source]=null;

        foreach(Node v in graph)
        {
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            // u is going to be unvisited node w/ smallest distance.
            Node u = null;

            foreach (Node possibleU in unvisited)
            {
                if (u == null || dist[possibleU] < dist[u])
                {
                    u = possibleU;
                }
            }

            if (u == target)
            {
                break;//Exits the while loop!
            }

            unvisited.Remove(u);
            foreach(Node v in u.neighbours)
            {
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(v.x,v.y);
                //in ties, the first neighbour will be selected.
                //this can create odd paths that are technically the same cost.
                //I left this in, because it makes the AI move in ways that often
                //give it more options as the player is a moving target.
                if(alt < dist[v])
                {
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        //We have the shortest route, or there is no route.
        if (prev[target] == null)
        {
            //No route to target
            return;
        }
        List<Node> currentPath = new List<Node>();
        Node curr = target;

        //Step through "prev" chain and add it to our path.
        while (curr != null)
        {
            currentPath.Add(curr);
            curr = prev[curr];
        }
        //Right now it is backwards though, so Reverse!
        currentPath.Reverse();
        piece.GetComponent<Unit>().currentPath = currentPath;
    }
}
