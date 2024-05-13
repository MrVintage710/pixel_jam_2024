using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

    public Camera camera;
    public Material backgroundMaterial;
    
    // Start is called before the first frame update
    void Start() {
        var half_size = camera.orthographicSize;
        var aspect_ratio = camera.aspect;
        var height = half_size * 2;
        var width = height * aspect_ratio;
        
        var transform = GetComponent<Transform>();
        transform.localScale = new Vector3( width, height, 1);
        Debug.Log(half_size);

    }

    // Update is called once per frame
    void Update() {
        backgroundMaterial.mainTextureOffset = GameManager.virtualPosition;
    }

    public void scroll(Vector2 delta) {
        
    }
}
