using UnityEngine;

public class TerrainPiece
{
    public SpriteRenderer objectRenderer; //Used as the handle for the actual gameobject.
    public bool isInUse; //Is the piece on the map? If not, it is back in the pool.
    public float Radius { get; private set; }

    public void PlacePiece(Vector2 position, Sprite sprite)
    {
        //Set sprite and radius
        objectRenderer.sprite = sprite;
        Radius = sprite.bounds.extents.x;
        CircleCollider2D collider = objectRenderer.GetComponent<CircleCollider2D>();
        collider.radius = Radius;

        //Set Position
        objectRenderer.transform.position = position;
        objectRenderer.gameObject.SetActive(true);
        isInUse = true;
    }

    public void RemovePiece()
    {
        objectRenderer.gameObject.SetActive(false);
        isInUse = false;
    }
}
