using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : SingletonBase<MapManager>
{
    [Header("MapInfo")]
    public TilemapRenderer m_TileMap;
    [HideInInspector] public Vector2Int m_MapDimensions;
    [HideInInspector] public Bounds m_MapBoundary;

    public Grid m_MapGrid;
    [HideInInspector] public Vector2 m_GridSize;

    public void Awake()
    {
        if (m_TileMap != null)
        {
            m_MapBoundary = m_TileMap.bounds;
            //m_MapDimensions = (Vector2Int)m_TileMap.size;
            //m_MapBoundary = m_TileMap.GetBoundsLocal();
        }

        if (m_MapGrid != null)
            m_GridSize = (Vector2)m_MapGrid.cellSize;
    }



}
