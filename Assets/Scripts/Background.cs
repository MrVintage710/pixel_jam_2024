using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {
    
    public Camera camera;
    public Material backgroundMaterial;

    private float _aspectRatio = 1.0f;
    private Vector2 _screenDims = Vector2.zero;
    
    // Start is called before the first frame update
    void Start() {
        UpdateSize(true);
    }

    // Update is called once per frame
    void Update() {
        backgroundMaterial.mainTextureOffset = new Vector2((GameManager.virtualPosition.x / _screenDims.x) * _aspectRatio, GameManager.virtualPosition.y / _screenDims.y);
        UpdateSize();
    }

    private void UpdateSize(bool forceUpdate = false) {
        var halfSize = camera.orthographicSize;
        _aspectRatio = camera.aspect;
        var height = halfSize * 2;
        var width = height * _aspectRatio;

        if (!Mathf.Approximately(width, _screenDims.x) || !Mathf.Approximately(height, _screenDims.y) || forceUpdate) {
            var transform = GetComponent<Transform>();
            transform.localScale = new Vector3( width, height, 1.0f);
            backgroundMaterial.mainTextureScale = new Vector2(_aspectRatio, 1.0f);
            _screenDims.x = width;
            _screenDims.y = height;
        }
    }
}
