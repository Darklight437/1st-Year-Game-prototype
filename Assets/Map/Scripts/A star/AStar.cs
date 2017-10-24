using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AStar
* 
* this finds the shortest path between two tiles and is a singleton class
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class AStar
{
    //a private static instance of our A* script
    private static AStar g_aStar;

    //this returns the private static instance 
    public static AStar g_AStarInstance
    {
        get
        {
            if (g_aStar == null)
            {
                g_aStar = new AStar();
            }
            return g_aStar;
        }
    }

    public List<Tiles> GetAStarPath(Tiles startTile, Tiles endTile)
    {
        if (startTile == null || endTile == null || startTile.IsPassible == false || endTile.IsPassible == false)
        {
            return new List<Tiles>();
        }
        
        List<Tiles> openSet = new List<Tiles>();
        List<Tiles> closedSet = new List<Tiles>();

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            Tiles currentTile = openSet[0];

            for (int i = 0; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentTile.FCost)
                {
                    currentTile = openSet[i];
                }
                else if (openSet[i].FCost == currentTile.FCost && openSet[i].HCost < currentTile.HCost)
                {
                    currentTile = openSet[i];
                }
            }

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == endTile)
            {
                return RetracePath(startTile, currentTile);
            }

            foreach (Tiles tile in currentTile.tileEdges)
            {
                if (tile.IsPassible != true || closedSet.Contains(tile))
                {
                    continue;
                }

                int newMovmeantCostToNeighbour = Mathf.RoundToInt(currentTile.GCost + GetDistance(currentTile, tile));

                if (newMovmeantCostToNeighbour < tile.GCost || FindInContainer(openSet, tile) == false)
                {
                    tile.GCost = newMovmeantCostToNeighbour;
                    tile.HCost = GetDistance(tile, endTile);
                    tile.parent = currentTile;

                    if (FindInContainer(openSet, tile) == false)
                    {
                        openSet.Add(tile);
                    }
                }
            }

        }

        return new List<Tiles>();
    }

    public List<Tiles> RetracePath(Tiles startTile, Tiles endTile)
    {
        List<Tiles> path = new List<Tiles>();

        Tiles startpos = endTile;
        Tiles currentTile = endTile.parent;

        while (startpos != startTile)
        {
            path.Add(startpos);
            startpos = currentTile;
            currentTile = startpos.parent;
        }

        path.Reverse();

        return path;
    }

    public bool FindInContainer(List<Tiles> list, Tiles toFind)
    {
        foreach (Tiles tiles in list)
        {
            if (tiles == toFind)
            {
                return true;
            }
        }

        return false;
    }

    public int GetDistance(Tiles tileA, Tiles tileB)
    {
        int disX = Mathf.RoundToInt(Mathf.Abs(tileA.transform.position.x - tileB.transform.position.x));
        int disY = Mathf.RoundToInt(Mathf.Abs(tileA.transform.position.y - tileB.transform.position.y));

        if (disX > disY)
        {
            return disY + 10 * (disX - disY);
        }
        else
        {
            return disX + 10 * (disY - disX);
        }
    }
    
}
