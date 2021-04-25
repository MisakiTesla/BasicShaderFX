using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleByPixelIndex : MonoBehaviour
{
    
    private static int _shaderPropID_GPUSkinning_TextureSize_NumPixelsPerFrame = Shader.PropertyToID("_GPUSkinning_TextureSize_NumPixelsPerFrame");
    private static int _index = Shader.PropertyToID("_Index");
    public Material mat;
    public Texture2D tex;
    public int hIndex;
    public int vIndex;

    private void OnGUI()
    {
        hIndex = (int) GUI.HorizontalSlider(new Rect((Screen.width - 600) / 2, Screen.height - 400, 600, 100), (float) hIndex, 0f, tex.width);
        vIndex = (int) GUI.VerticalSlider(new Rect(Screen.width - 40, Screen.height / 2 - 75 - 300, 100, 600), (float) vIndex, tex.height, 0f);
        if (GUI.Button(new Rect((Screen.width - 200)/2 , Screen.height - 300, 200, 100), $"SetIndex [ {hIndex} * {vIndex} ]"))
        {
            mat.SetVector(_shaderPropID_GPUSkinning_TextureSize_NumPixelsPerFrame, new Vector4(tex.width,tex.height,8));
            mat.SetFloat(_index, hIndex + vIndex * tex.width);
        }
    }
}
