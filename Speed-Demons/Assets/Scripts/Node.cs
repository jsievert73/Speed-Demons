using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
    {
        public List<Node> neighbours;
        public int x;
        public int y;
        public Unit housingUnit;
        public bool chokePoint;
        public bool chokeAdjacent;
        public bool inUse;
        public Node predecessor;
        public Node follower;

        public Node()
        {
            neighbours = new List<Node>();
            housingUnit = null;
            chokePoint = false;
            chokeAdjacent = false;
            inUse = false;
            predecessor = null;
            follower = null;
        }
        public float DistanceTo(Node n)
        {
            //This will make diagonals cost a little more.
            //While from a game mechanic standpoint that is not true,
            //It will make the paths straighter in general
            //Giving a more reasonable path.
            return Vector2.Distance(new Vector2(x,y), new Vector2(n.x, n.y));
        }
    }
