using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public static float aspectRatio = 1.0f;
    
    public Camera camera;
    public Material backgroundMaterial;

    private Vector2 screenDims = Vector2.zero;
    
    // Start is called before the first frame update
    void Start() {
        updateSize(true);
    }

    // Update is called once per frame
    void Update() {
        backgroundMaterial.mainTextureOffset = new Vector2((GameManager.virtualPosition.x / screenDims.x) * aspectRatio, GameManager.virtualPosition.y / screenDims.y);
        updateSize();
    }

    private void updateSize(bool forceUpdate = false) {
        var halfSize = camera.orthographicSize;
        aspectRatio = camera.aspect;
        var height = halfSize * 2;
        var width = height * aspectRatio;

        if (!Mathf.Approximately(width, screenDims.x) || !Mathf.Approximately(height, screenDims.y) || forceUpdate) {
            var transform = GetComponent<Transform>();
            transform.localScale = new Vector3( width, height, 1.0f);
            backgroundMaterial.mainTextureScale = new Vector2(aspectRatio, 1.0f);
            screenDims.x = width;
            screenDims.y = height;
        }
    }
}
