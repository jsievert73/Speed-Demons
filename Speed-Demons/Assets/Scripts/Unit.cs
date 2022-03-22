using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	// tileX and tileY represent the correct map-tile position
	// for this piece.  Note that this doesn't necessarily mean
	// the world-space coordinates, because our map might be scaled
	// or offset or something of that nature.  Also, during movement
	// animations, we are going to be somewhere in between tiles.
	public int tileX;
	public int tileY;

	public TileMap map;

	// Our pathfinding info.  Null if we have no destination ordered.
	public List<Node> currentPath = null;
    public Node houser;
    public string unitType;
	public string attackType;
	public int health;
	public bool alive = true;
	public HealthBarController bar;

	// How far this unit can move in one turn. Note that some tiles cost extra.
	int moveSpeed = 2;
	float remainingMovement=0;

	void Start()
	{
		//map.GeneratePathTo(8,8, this);
	}

	void Update() {
		// Draw our debug line showing the pathfinding!
		// NOTE: This won't appear in the actual game view.
		bar.UpdateHP(health);
		if(currentPath != null) {
			int currNode = 0;

			while( currNode < currentPath.Count-1 ) {

				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, 0, -0.5f) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, 0, -0.5f) ;

				Debug.DrawLine(start, end, Color.red);

				currNode++;
			}
		}

		// Have we moved our visible piece close enough to the target tile that we can
		// advance to the next step in our pathfinding?
		if(Vector3.Distance(transform.position, map.TileCoordToWorldCoord( tileX, tileY )) < 0.1f)
			AdvancePathing();
			if (alive)
			{
            	UpdateHousing();
			}
		// Smoothly animate towards the correct map tile.
		transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord( tileX, tileY ), 5f * Time.deltaTime);
	}

	// Advances our pathfinding progress by one tile.
	void AdvancePathing() {
		if(currentPath==null)
			return;
		if (unitType == "enemy"&&alive)
		{
        	if(attackType=="melee" || attackType=="ranged")
			{
				if(currentPath[1].housingUnit != null&&remainingMovement != 0)
				{
					Attack();
					remainingMovement = 0;
					print("found a unit");
				}
			}
			if(attackType=="ranged"&&remainingMovement != 0)
			{
				if(currentPath[2].housingUnit != null)
				{
					Attack();
					remainingMovement = 0;
				}
			}
		}
		if (unitType == "player" && ClickableTile.stab)
		{
			if(currentPath[1].housingUnit != null && remainingMovement != 0)
				{
					print("foundtarget");
					Attack();
					remainingMovement = 0;
					ClickableTile.stab = false;
					currentPath = null;
				}
		}
		if(remainingMovement <= 0)
			return;

		// Teleport us to our correct "current" position, in case we
		// haven't finished the animation yet.
		transform.position = map.TileCoordToWorldCoord( tileX, tileY );

		// Get cost from current tile to next tile
		remainingMovement -= map.CostToEnterTile(currentPath[1].x, currentPath[1].y );
		
		// Move us to the next tile in the sequence
		tileX = currentPath[1].x;
		tileY = currentPath[1].y;
		
		// Remove the old "current" tile from the pathfinding list
		if (alive)
		{
        	currentPath[0].housingUnit = null;
			currentPath.RemoveAt(0);
        	currentPath[0].housingUnit = this;
		}
		
		if(currentPath.Count == 1) {
			// We only have one tile left in the path, and that tile MUST be our ultimate
			// destination -- and we are standing on it!
			// So let's just clear our pathfinding info.
			currentPath = null;
		}
	}

	// The "Next Turn" button calls this.
	public void NextTurn() {
        if(unitType == "enemy")
        {
            map.GeneratePathTo(map.selectedUnit.GetComponent<Unit>().tileX, map.selectedUnit.GetComponent<Unit>().tileY,this);
        }
		// Make sure to wrap-up any outstanding movement left over.
		while(currentPath!=null && remainingMovement > 0) {
			AdvancePathing();
		}

		// Reset our available movement points.
		remainingMovement = moveSpeed;
	}
    public void UpdateHousing()
    {
        if (houser != null)
        {
            houser.housingUnit=null;
        }
        houser = map.graph[tileX,tileY];
        houser.housingUnit=this;
    }
	public void Attack()
	{
		if (unitType == "player" && !ClickableTile.stab)
		{
			for (int x =-1; x < 2; x++)
        	{
            	for (int y =-1; y < 2; y++)
        		{
            		Unit targetUnit = map.graph[tileX+x,tileY+y].housingUnit;
					if (targetUnit!=null && targetUnit.unitType!="player")
					{
						targetUnit.health -= 1;
						if (targetUnit.health <= 0)
						{
							Kill(targetUnit);
						}
					}
        		}
        	}
		}
		if (unitType == "player" && ClickableTile.stab)
		{
			Unit targetUnit = currentPath[1].housingUnit;
			targetUnit.health -= 1;
			if (targetUnit.health <= 0)
			{
			Kill(targetUnit);
			}
		}
		if (attackType == "melee")
		{
			for (int x =-1; x < 2; x++)
        	{
            	for (int y =-1; y < 2; y++)
        		{
            		Unit targetUnit = map.graph[tileX+x,tileY+y].housingUnit;
					if (targetUnit!=null && targetUnit.unitType =="player")
					{
						targetUnit.health -= 1;
						if (targetUnit.health <= 0)
						{
							Kill(targetUnit);
						}
					}
        		}
        	}
		}
		if (attackType == "ranged")
		{
			for (int x =-2; x < 3; x++)
        	{
            	for (int y =-2; y < 3; y++)
        		{
            		Unit targetUnit = map.graph[tileX+x,tileY+y].housingUnit;
					if (targetUnit!=null && targetUnit.unitType =="player")
					{
						targetUnit.health -= 1;
						if (targetUnit.health <= 0)
						{
							Kill(targetUnit);
						}
					}
        		}
        	}
		}
	}
	
	public void Kill(Unit corpse)
	{
		if (corpse.unitType == "enemy")
		{
			corpse.gameObject.SetActive(false);
			map.graph[corpse.tileX,corpse.tileY].housingUnit = null;
			map.enemyCount-=1;
			corpse.alive = false;
			if (houser != null)
        	{
            	houser.housingUnit=null;
        	}
			if (map.enemyCount<= 0)
			{
				SceneController.Win();
			}
		}
		if (corpse.unitType == "player")
		{
			SceneController.Loss();
		}
	}
}
