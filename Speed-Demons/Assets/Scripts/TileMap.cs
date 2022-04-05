using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileMap : MonoBehaviour
{
    public GameObject selectedUnit;
    public GameObject player;
    public int towers = 3;
    public TileType[] tileTypes;
    public int[,] tiles;
    public Node[,] graph;
    public int enemyCount;
    private List<Node> currentPath;
    public List<EnemyController> enemies = new List<EnemyController>();
    public GameObject[,] tower;
    public int health = 3;

    int mapSizeX = 10;
    int mapSizeY = 10;
    public GameObject towerPic1;
    public GameObject towerPic2;
    public GameObject towerPic3;
    public GameObject healthPic1;
    public GameObject healthPic2;
    public GameObject healthPic3;

    void Start() 
    {
        selectedUnit.GetComponent<Unit>().tileX = (int)selectedUnit.transform.position.x;
        selectedUnit.GetComponent<Unit>().tileY = (int)selectedUnit.transform.position.y;
        selectedUnit.GetComponent<Unit>().map = this;

        GenerateMapData();
        GeneratePathfindingGraph();
        GenerateMapVisuals();
        tower = new GameObject[mapSizeX,mapSizeY];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y <10; y++)
            {
                tower[x,y] = player;
            }
        }
        GeneratePathTo(8,8,selectedUnit.GetComponent<Unit>());
        foreach (Node waypoint in selectedUnit.GetComponent<Unit>().currentPath)
        {
            Vector3 sampler = new Vector3(0,0,-5);
            sampler.x = waypoint.x;
            sampler.y = waypoint.y;
            //print("SAMPLER" + sampler);
        }
    }
    void Update()
    {
        if(towers == 3)
        {
            towerPic3.SetActive(true);
        }
        if(towers == 2)
        {
            towerPic3.SetActive(false);
            towerPic2.SetActive(true);
        }
        if(towers == 1)
        {
            towerPic2.SetActive(false);
            towerPic1.SetActive(true);
        }
        if(towers == 0)
        {
            towerPic1.SetActive(false);
        }
        if(health == 2)
        {
            healthPic3.SetActive(false);
        }
        else if(health == 1)
        {
            healthPic2.SetActive(false);
        }
        else if(health == 0)
        {
            healthPic1.SetActive(false);
        }
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

    //Accessing our tileTypes data to determine movementCosts
    public float CostToEnterTile(int x, int y)
    {
        TileType tt = tileTypes[tiles[x,y]];
        if (tt.isWalkable == false)
        {
            return Mathf.Infinity;
        }
        return tt.movementCost;
    }

    
    //A modular graph of Nodes are generated to store map data.
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
                    if(tiles[x-1,y] != 2)
                    {
                        graph[x,y].neighbours.Add(graph[x-1,y]);
                    }
                    left =true;
                }
                if (x < mapSizeX-1)
                {
                    if(tiles[x+1,y] != 2)
                    {
                        graph[x,y].neighbours.Add(graph[x+1,y]);
                    }
                    right = true;
                }
                if (y != 0)
                {
                    if(tiles[x,y-1] != 2)
                    {
                        graph[x,y].neighbours.Add(graph[x,y-1]);
                    }
                    if (left)
                    {
                        //graph[x,y].neighbours.Add(graph[x-1,y-1]);
                    }
                    if (right)
                    {
                        //graph[x,y].neighbours.Add(graph[x+1,y-1]);
                    }
                }
                if (y < mapSizeX-1)
                {
                    if(tiles[x,y+1] != 2)
                    {
                        graph[x,y].neighbours.Add(graph[x,y+1]);
                    }
                    if (left)
                    {
                        //graph[x,y].neighbours.Add(graph[x-1,y+1]);
                    }
                    if (right)
                    {
                        //graph[x,y].neighbours.Add(graph[x+1,y+1]);
                    }
                }
                if(graph[x,y].neighbours.Count == 2)
                {
                    graph[x,y].chokePoint = true;
                    foreach(Node neighbor in graph[x,y].neighbours)
                    {
                        neighbor.chokeAdjacent = true;
                    }
                }
            }
        }
    }

    //spawn prefabs for each of the tiles that are defined by tileTypes.
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
        //But currently we say all tiles areWalkable by default.
        return tileTypes[tiles[x,y]].isWalkable;
    }

    public void GeneratePathTo(int x, int y, Unit piece)
    {
        //print("BEGIN PATH GENERATION");
        piece.GetComponent<Unit>().currentPath = null;

        if(UnitCanEnterTile(x,y) == false)
        {
            //print("QUIT PATH GENERATION");
            //quits out because no path should be found.
            return;
        }

        Dictionary<Node, float> dist = new Dictionary<Node, float>();
        Dictionary<Node, Node> prev = new Dictionary<Node, Node>();

        List<Node> unvisited = new List<Node>();

        //take our starting point
        Node source = graph[piece.GetComponent<Unit>().tileX,piece.GetComponent<Unit>().tileY];
        //and our eventual goal
        Node target = graph[x,y];

        //Make our dist dictionary and set source to 0, so that we will use it as our starting point.
        dist[source]=0;
        prev[source]=null;

        foreach(Node v in graph)
        {
            //resets our data, so that our starting point, source, is the only data we know.
            if(v != source)
            {
                dist[v] = Mathf.Infinity;
                prev[v] = null;
            }

            unvisited.Add(v);
        }

        while (unvisited.Count > 0)
        {
            print("UNVISITED COUNT: "+ unvisited.Count);
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
                print("TARGET FOUND");
                break;//Exits the while loop, because we hit our goal.
            }

            unvisited.Remove(u);
            foreach(Node v in u.neighbours)
            {
                print("Checking v's neighbors");
                //float alt = dist[u] + u.DistanceTo(v);
                float alt = dist[u] + CostToEnterTile(v.x,v.y);
                print("CALCULATED U NODE: " + u.x + "," + u.y);
                print("DISTANCE[U]: "+ dist[u]+ "   CALCULATED ALT: "+ alt+"   DISTANCE[V]: "+ dist[v]);
                //in ties, the first neighbour will be selected.
                //this can create odd paths that are technically the same cost.
                if(alt < dist[v])
                {
                    print("PATH UPDATED PREV DATA");
                    dist[v] = alt;
                    prev[v] = u;
                }
            }
        }
        //We have the shortest route, or there is no route.
        if (prev[target] == null)
        {
            //print("QUIT PATH GENERATION: NO ROUTE");
            //No route to target
            return;
        }
        currentPath = new List<Node>();
        Node curr = target;
        Node predicate = null;

        //Step through "prev" chain and add it to our path.
        while (curr != null)
        {
            curr.inUse = true;
            if(predicate != null)
            {
                curr.predecessor = predicate;
                predicate.follower = curr;
            }
            currentPath.Add(curr);
            //print("ADDED TO CURRENTPATH");
            predicate = curr;
            curr = prev[curr];
        }
        //Right now it is backwards though, so Reverse!
        currentPath.Reverse();
        piece.GetComponent<Unit>().currentPath = currentPath;
        //print("FINISHED PATH CREATION");
    }

    public void EditPath(int x, int y, Unit piece, int startX, int startY)
    {
        piece.GetComponent<Unit>().currentPath = null;
        piece.tileX = startX;
        piece.tileY = startY;
        piece.UpdateHousing();
        List<Node> oldPath = currentPath;
        List<Node> finishPath = new List<Node>();
        GeneratePathTo(x,y,piece);
        //print("EDITED PATH CREATED");
        bool skip = false;
        foreach(Node n in oldPath)
        {
            //print("EDITED OLD PATH ITERATED");
            if(!skip)
            {
                finishPath.Add(n);
                //print("ADDED UNEDITED PATH COMPONENT");
            }
            skip = false;
            if(n == currentPath[0])
            {
                foreach(Node cur in currentPath)
                {
                    //print("ADDED EDITED PATH COMPONENTS");
                    finishPath.Add(cur);
                    skip = true;
                }
            }
        }
        currentPath = finishPath;
        //print("FINISHED EDITED CURRENTPATH LENGTH: " + currentPath.Count);
        foreach(EnemyController e in enemies)
        {
            //print("BEGAN EDITED ENEMY");
            List<Node> disposablePath = currentPath;
            while(disposablePath != null)
            {
                //print("DISPOSABLE PATH NOT EMPTY");
                Node n = disposablePath[0];
                if(n.x == (int) (e.targetWaypoint.x-0.5f) && n.y == (int) (e.targetWaypoint.y+0.5f))
                {
                    //print("FOUND DISPOSABLE CROSSOVER");
                    e.UpdatePath(disposablePath);
                    disposablePath = null;
                }
                else
                {
                    disposablePath.Remove(n);
                }
            }
        }
    }
}
