using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
* class Map
* inherits MonoBehaviour
* 
* this class holds all the map tiles parented to its game object
* 
* author: Callum Dunstone, Academy of Interactive Entertainment, 2017
*/
public class Map : MonoBehaviour
{
    //list of all the tiles
    public List<Tiles> mapTiles;

    /*
    * Start
    * public void function
    * 
    * this function is called at the start of the scripts life
    * in play mode
    * 
    * @returns nothing
    */
    void Start ()
    {
        SetTileEdges();
    }

    /*
    * SetTileEdges
    * public void function
    * 
    * this function goes through all the tiles finding there
    * adjacents on North, South, East and west and connects them up for pathfinding
    * purposes
    * 
    * @returns nothing
    */
    private void SetTileEdges()
    {
        //goes through each of the nodes
        foreach (Tiles tileA in mapTiles)
        {
            tileA.tileEdges = new List<Tiles>();
            foreach (Tiles tileB in mapTiles)
            {
                //gets the offset of the x and z to determin if the tile is next to the one we are looking at
                float offsetX = tileA.transform.position.x - tileB.transform.position.x;
                float offsetZ = tileA.transform.position.z - tileB.transform.position.z;

                //true if tile to the East
                if (offsetX == 1 && offsetZ == 0)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the South
                if (offsetX == 0 && offsetZ == -1)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the West
                if (offsetX == -1 && offsetZ == 0)
                {
                    tileA.tileEdges.Add(tileB);
                }

                //true if tile to the North
                if (offsetX == 0 && offsetZ == 1)
                {
                    tileA.tileEdges.Add(tileB);
                }
            }
        }
    }
}
