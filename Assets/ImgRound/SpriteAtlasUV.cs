using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SpriteAtlasUV : MonoBehaviour
{
    public SpriteAtlas atlas;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(atlas.GetSprite("e").uv[0]);
        Debug.Log(atlas.GetSprite("p").uv[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
