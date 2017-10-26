﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class AStar
* 
* this finds the shortest path between two tiles and is a singleton class
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public static class AStar
{
    /*
    * GetAStarPath
    * public List<Tiles> function (Tiles startTile, Tiles endTile)
    * 
    * this function attempts to find the shortes path between the two tiles passed in
    * and once it has it returns a list of tiles that lead between the two
    * 
    * @returns List<Tiles>
    */
    public static List<Tiles> GetAStarPath(Tiles startTile, Tiles endTile)
    {
        //check to see if one of the tiles passed in does not exist or is not passible meaning they can not move to/from
        if (startTile == null || endTile == null || startTile.IsPassible == false || endTile.IsPassible == false)
        {
            return new List<Tiles>();
        }
        
        //list of tiles we can look at
        List<Tiles> openSet = new List<Tiles>();
        //list of tiles we have looked at
        List<Tiles> closedSet = new List<Tiles>();

        //add the start tile to our open list to start off the search
        openSet.Add(startTile);

        //while there are things to search keep searching
        while (openSet.Count > 0)
        {
            //set the first tile in the list to be the tile we look at
            Tiles currentTile = openSet[0];

            //search through the list for a tile that is closer to our target and set it to the current
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

            //remove the tile we just looked at from open set and add it to closed set
            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            //check if the current tile is the tile we are trying to get to
            //if it is get the path and return it
            if (currentTile == endTile)
            {
                return RetracePath(startTile, currentTile);
            }

            //go through all the tiles that are adgacent to the tile we are currently looking at
            foreach (Tiles tile in currentTile.tileEdges)
            {
                //check if the tile is passible or we have already looked at it
                //if so we skip over this tile
                if (tile.IsPassible != true || closedSet.Contains(tile))
                {
                    continue;
                }

                int newMovmeantCostToNeighbour = Mathf.RoundToInt(currentTile.GCost + GetDistance(currentTile, tile));

                //check if it has a lower g score now for what ever reason or if the node is not in the open list
                if (newMovmeantCostToNeighbour < tile.GCost || FindInContainer(openSet, tile) == false)
                {
                    //set/re set it vlaues
                    tile.GCost = newMovmeantCostToNeighbour;
                    tile.HCost = GetDistance(tile, endTile);
                    tile.parent = currentTile;

                    //if tile is not it the open list add it
                    if (FindInContainer(openSet, tile) == false)
                    {
                        openSet.Add(tile);
                    }
                }
            }

        }

        //if we get here there was no path so we return a empty container
        return new List<Tiles>();
    }

    /*
    * RetracePath
    * public List<Tiles> function (Tiles startTile, Tiles endTile)
    * 
    * this goes through the tiles parents starting at the end tile to
    * trace a path back to the start node to get us our path
    * 
    * @returns List<Tiles>
    */
    public static List<Tiles> RetracePath(Tiles startTile, Tiles endTile)
    {
        List<Tiles> path = new List<Tiles>();

        Tiles startpos = endTile;
        Tiles currentTile = endTile.parent;

        //loops through the tiles parent tiles and adds them selfes to the list
        while (startpos != startTile)
        {
            path.Add(startpos);
            startpos = currentTile;
            currentTile = startpos.parent;
        }

        //reverse the path so it does not lead from the end to the start
        //but from the start to the end
        path.Reverse();

        return path;
    }

    /*
    * FindInContainer
    * public bool function (List<Tiles> list, Tiles toFind)
    * 
    * this goes through a list of tiles that is passed in and
    * searches through it looking for the tile that was also passed it
    * if it finds the tile it returns true else it returns false
    * 
    * @returns bool
    */
    public static bool FindInContainer(List<Tiles> list, Tiles toFind)
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

    /*
    * GetDistance
    * public int function (Tiles tileA, Tiles tileB)
    * 
    * this calculates a huristic for the distance the path will need to travel between two points
    * 
    * @returns int
    */
    public static int GetDistance(Tiles tileA, Tiles tileB)
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
