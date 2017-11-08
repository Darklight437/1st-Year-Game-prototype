using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Statistics", menuName = "Inventory/Statistics", order = 1)]
public class Statistics : ScriptableObject
{
    //reference to the armour function
    public AnimationCurve armourCurve = AnimationCurve.Linear(0,0,4,50);

    //amount of health gained when a healing tile is stepped on
    public float tileHealthGained = 500.0f;

    //amount of damage a trap tile does when stepped on
    public float trapTileDamage = 100.0f;

    public int normalTileTypeAmount;
    private List<int> m_normalTileVariantUnused;

    public int damageTileTypeAmount;
    private List<int> m_damageTileVariantUnused;

    public int impassableTileTypeAmount;
    private List<int> m_impassableTileVariantUnused;

    public int defenseTileTypeAmount;
    private List<int> m_defenseTileVariantUnused;

    public int RandomNum(eTileType type)
    {
        RestockTileVarientsUnsused();
        int num = 0;
        
        switch (type)
        {
            case eTileType.NORMAL:
                num = Random.Range(0, m_normalTileVariantUnused.Count);
                num = m_normalTileVariantUnused[num];
                m_normalTileVariantUnused.Remove(num);
                return num;

            case eTileType.DAMAGE:
                num = Random.Range(0, m_damageTileVariantUnused.Count);
                num = m_damageTileVariantUnused[num];
                m_damageTileVariantUnused.Remove(num);
                return num;

            case eTileType.DEFENSE:
                num = Random.Range(0, m_defenseTileVariantUnused.Count);
                num = m_defenseTileVariantUnused[num];
                m_defenseTileVariantUnused.Remove(num);
                return num;

            case eTileType.IMPASSABLE:
                num = Random.Range(0, m_impassableTileVariantUnused.Count);
                num = m_impassableTileVariantUnused[num];
                m_impassableTileVariantUnused.Remove(num);
                return num;

            case eTileType.NULL:
                return 0;

            case eTileType.DEBUGGING:
                return 0;

            default:
                return 0;
        }

        return 0;
    }

    public void RestockTileVarientsUnsused()
    {
        if (m_normalTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < normalTileTypeAmount; i++)
            {
                m_normalTileVariantUnused.Add(i);
            }
        }

        if (m_damageTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < damageTileTypeAmount; i++)
            {
                m_damageTileVariantUnused.Add(i);
            }
        }

        if (m_defenseTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < defenseTileTypeAmount; i++)
            {
                m_defenseTileVariantUnused.Add(i);
            }
        }

        if (m_impassableTileVariantUnused.Count == 0)
        {
            for (int i = 0; i < impassableTileTypeAmount; i++)
            {
                m_impassableTileVariantUnused.Add(i);
            }
        }
    }
}