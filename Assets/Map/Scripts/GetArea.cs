using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class GetArea
* 
* this finds all tiles around a single tiles with in a given radius
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public static class GetArea
{
    /*
    * GetAreaOfMoveable
    * public List<Tiles> function (Tiles start, int radius)
    * 
    * this function gets all moveable tiles with in the radius given to it
    * around the tile also passed in. once it has all the tiles it returns it as a list
    * 
    * @returns List<Tiles>
    */
    public static List<Tiles> GetAreaOfMoveable(Tiles start, int radius)
    {
        List<Tiles> openSet = new List<Tiles>();
        List<Tiles> closedSet = new List<Tiles>();

        openSet.Add(start);

        for (int i = 0; i <= radius; i++)
        {
            List<Tiles> currTiles = new List<Tiles>();

            foreach (Tiles tile in openSet)
            {
                currTiles.Add(tile);
            }

            openSet.Clear();

            foreach (Tiles tile in currTiles)
            {
                closedSet.Add(tile);
            }

            foreach (Tiles openTile in currTiles)
            {
                foreach (Tiles lookAtTile in openTile.tileEdges)
                {
                    if (lookAtTile.IsPassible && FindInContainer(closedSet, lookAtTile) == false)
                    {
                        openSet.Add(lookAtTile);
                    }
                }
            }
        }

        return closedSet;
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
}
