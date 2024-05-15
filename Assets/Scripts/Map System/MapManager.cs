using UnityEngine;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject terrainPrefab;
    [SerializeField]
    float tileScale = 100f;
    [SerializeField, Range(0,99)]
    int tilePadding = 1;
    [SerializeField, Range(0f,1f)]
    float terrainDensity = 0.5f;
    [SerializeField]
    Sprite[] terrainSprites;

    private List<TerrainPiece> terrainPool;

    private int lastXCoord = 0;
    private int lastYCoord = 0;

    //Event
    public static event System.Action<System.ValueTuple<Vector2, float>[]> UpdateTerrainCollision; //Reports changes in the tile map for the benefit of ecs.
    private List<System.ValueTuple<Vector2, float>> collisionList;


    public void Start()
    {
        terrainPool = new List<TerrainPiece>();
        collisionList = new List<(Vector2, float)> ();
        RefreshTerrainGrid();
    }

    public void Update()
    {
        int currentXCoord = Mathf.RoundToInt(GameManager.playerPosition.x / tileScale);
        int currentYCoord = Mathf.RoundToInt(GameManager.playerPosition.y / tileScale);

        if(lastXCoord != currentXCoord || lastYCoord != currentYCoord)
        {
            lastXCoord = currentXCoord;
            lastYCoord = currentYCoord;
            RefreshTerrainGrid();
        }
    }

    private void RefreshTerrainGrid()
    {
        collisionList.Clear();

        foreach(TerrainPiece piece in terrainPool)
        {
            piece.RemovePiece();
        }
        
        
        for(int i = -tilePadding; i <= tilePadding; i++)
        {
            for(int j = -tilePadding; j <= tilePadding; j++)
            {
                FormatTerrainInTile(i + lastXCoord, j + lastYCoord);
            }
        }

        UpdateTerrainCollision.Invoke(collisionList.ToArray());
    }

    private void FormatTerrainInTile(int xCoord, int yCoord)
    {
        //Set the random seed using coordinates
        Random.InitState(xCoord);
        int xPart = Random.Range(0,int.MaxValue);
        Random.InitState(yCoord);
        int yPart = UnityEngine.Random.Range(int.MinValue, 0);
        Random.InitState(xPart + yPart);

        if(Random.value > terrainDensity) { return; } //End if not terrain in this tile.
        
        Sprite sprite = terrainSprites[Random.Range(0,terrainSprites.Length)];

        //Place Terrain
        Vector2 tileCenter = new Vector2(tileScale * xCoord, tileScale * yCoord);
        Vector2 terrainPosition = (0.4f * tileScale * Random.insideUnitCircle) + tileCenter; //Placement area is a little bit less than tile size.
        TerrainPiece tPiece = GetFreeTerrainPiece();
        tPiece.PlacePiece(terrainPosition, sprite);
        
        //Add to collision list
        collisionList.Add(new System.ValueTuple<Vector2, float>(terrainPosition, tPiece.Radius));
    }

    #region Pool Management
    public TerrainPiece GetFreeTerrainPiece()
    {
        foreach (TerrainPiece piece in terrainPool)
        {
            if (!piece.isInUse)
            {
                return piece;
            }
        }

        //If no free piece is found, instantiate another one.
        TerrainPiece terrainPiece = new TerrainPiece();
        GameObject terrainObject = Instantiate(terrainPrefab);
        terrainObject.transform.parent = transform; //Parent to this. We will use this object to maneuver all the terrain.
        terrainPiece.objectRenderer = terrainObject.GetComponent<SpriteRenderer>();
        terrainPool.Add(terrainPiece);
        return terrainPiece;
    }

    #endregion
}
