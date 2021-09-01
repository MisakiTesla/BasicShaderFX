using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MipmapGenerator : MonoBehaviour
{
    public RawImage rawImage;
    public Texture2D texture;
    private RenderTexture _inputRT;
    private RenderTexture _outputRT;
    public ComputeShader computeShader;
    // Start is called before the first frame update
    void Start()
    {
        _inputRT = RenderTexture.GetTemporary(new RenderTextureDescriptor(texture.width, texture.height, RenderTextureFormat.Default));
        _outputRT = RenderTexture.GetTemporary(new RenderTextureDescriptor(texture.width/2, texture.height/2, RenderTextureFormat.Default));
        Graphics.Blit(texture, _inputRT);
        
        _outputRT.enableRandomWrite = true;
        _outputRT.filterMode = FilterMode.Point;
        _outputRT.Create();

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Vector2.zero, Vector2.one*100),"Dispatch"))
        {
            int kernel = computeShader.FindKernel("CSMain");
            computeShader.SetTexture(kernel, "_MainTex", _inputRT);
            computeShader.SetTexture(kernel, "Result", _outputRT);
            computeShader.SetVector("_MainTex_Size", new Vector4(_inputRT.width, _inputRT.height, 0, 0));

            computeShader.Dispatch(kernel, _outputRT.width / 8, _outputRT.height / 8, 1);
            rawImage.texture = _outputRT;
        }
        if (GUI.Button(new Rect(Vector2.right*100, Vector2.one*100),"Test"))
        {
            
        }
    }
}
