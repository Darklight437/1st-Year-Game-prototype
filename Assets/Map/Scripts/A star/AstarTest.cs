using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarTest : MonoBehaviour
{
    public Tiles start = null;
    public Tiles end = null;

    public List<Tiles> path;
	
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //sets out a raycast from the mouses position
            if (Physics.Raycast(ray, out hit))
            {
                //if we hit a tile we enter paint mode
                if (hit.transform.tag == "Tile")
                {
                    if (start == null)
                    {
                        start = hit.transform.GetComponent<Tiles>();
                    }
                    else if (end == null)
                    {
                        end = hit.transform.GetComponent<Tiles>();
                    }
                    else
                    {
                        start = end;
                        end = hit.transform.GetComponent<Tiles>();
                    }

                }
            }
        }

        if (Input.GetKeyDown("space"))
        {
            foreach (Tiles tile in path)
            {
                tile.tileType = eTileType.NORMAL;
                tile.GenerateRandomTileVariant();
            }
            path = AStar.g_AStarInstance.GetAStarPath(start, end);
            foreach (Tiles tile in path)
            {
                tile.tileType = eTileType.PATH;
                tile.GenerateRandomTileVariant();
            }
        }
    }
}
