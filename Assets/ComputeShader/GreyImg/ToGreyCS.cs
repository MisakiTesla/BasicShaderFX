using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;

public class ToGreyCS : MonoBehaviour
{
    public ComputeShader shader;
    public RawImage sourceImage;
    public RawImage targetImage;
    private Texture2D _inputTex;


    public void RunShader()
    {
        Debug.Log($"=======Run le");
        _inputTex = sourceImage.texture as Texture2D;
        int kernelHandle = shader.FindKernel("CSMain");

        // RenderTexture tex = new RenderTexture(256,256,24);
        RenderTexture tex = new RenderTexture(_inputTex.width,_inputTex.height,24);
        tex.enableRandomWrite = true;
        tex.Create();

        shader.SetTexture(kernelHandle, "Result", tex);
        shader.SetTexture(kernelHandle, "InputTexture", _inputTex);
        // shader.Dispatch(kernelHandle, 256/8, 256/8, 1);
        shader.Dispatch(kernelHandle, _inputTex.width,_inputTex.height, 1);
        targetImage.texture = tex;
    }

    public async UniTask RunCompute()
    {

    }
}
