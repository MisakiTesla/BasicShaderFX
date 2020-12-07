using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDepth : MonoBehaviour
{
    public Material postMat;

    public bool effect;
    public float distance = 0;
    public float speed = 1;
    public RawImage rawImg;

    public RenderTexture rt;
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        postMat.SetFloat("_Distance", distance);
        Graphics.Blit(src, dest, postMat);
        rt = dest;
    }

    /// <summary>
    /// OnPostRender is called after a camera finishes rendering the scene.
    /// </summary>
    void OnPostRender()
    {
    }

    private void Update()
    {
                rawImg.texture = rt;

        if (effect)
        {
            distance += speed * Time.deltaTime;
        }
        else
        {
            distance = 0;
        }
    }
}
