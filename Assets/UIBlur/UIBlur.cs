using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBlur : MonoBehaviour
{
    public Camera uiCamera;
    public RawImage rawImage;
    [Range(0, 4)]
    public int downSample = 0;
    [Range(1, 8)]
    public int iterations = 1;
    [Range(0f, 1f)]
    public float interpolation = 1f;
    public Material blurMaterial;
    private static readonly int _Radius = Shader.PropertyToID("_Radius");
    private RenderTexture rt;

    public void Refresh()
    {
        rawImage.enabled = false;
        // 创建一个RenderTexture对象  
        // NPOT
        //rt = new RenderTexture((int)Screen.width>> downSample, (int)Screen.height>> downSample, 24, RenderTextureFormat.ARGB32);
        // 非NPOT
        if (!rt)
        {
            rt = new RenderTexture((int) Screen.width / (downSample + 1), (int) Screen.height / (downSample + 1), 24, RenderTextureFormat.ARGB32);
        }
        
        //尽量用GetTemporary
        //rt = RenderTexture.GetTemporary(Screen.width>> downSample, Screen.height>> downSample, 24, RenderTextureFormat.Default);
        //rt = RenderTexture.GetTemporary(Screen.width / (downSample + 1), Screen.height / (downSample + 1), 24, RenderTextureFormat.Default);

        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        uiCamera.targetTexture = rt;
        uiCamera.Render();
        // 重置相关参数，以使用camera继续在屏幕上显示  
        uiCamera.targetTexture = null;
        ProcessRT(rt, null);
        rawImage.texture = rt;
        rawImage.enabled = true;
    }

    private void OnDestroy()
    {
        rt.DiscardContents();
    }

    private void ProcessRT(RenderTexture source, RenderTexture destination)
    {

            var rt2 = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);

            for (int i = 0; i < iterations; i++)
            {
                // helps to achieve a larger blur
                //float radius = (float)i * interpolation + interpolation;
                //blurMaterial.SetFloat(_Radius, radius);

                Graphics.Blit(source, rt2, blurMaterial, 0);
                source.DiscardContents();

                // is it a last iteration? If so, then blit to destination
                if (i == iterations - 1)
                {
                    Graphics.Blit(rt2, destination, blurMaterial, 1);
                }
                else
                {
                    Graphics.Blit(rt2, source, blurMaterial, 1);
                    rt2.DiscardContents();
                }
            }

            RenderTexture.ReleaseTemporary(rt2);
        
    }

    Texture2D CaptureCamera(Camera camera, Rect rect)   
    {  
        // 创建一个RenderTexture对象  
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);  
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera.targetTexture = rt;  
        camera.Render();  
        //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
        //ps: camera2.targetTexture = rt;  
        //ps: camera2.Render();  
        //ps: -------------------------------------------------------------------  

        // 激活这个rt, 并从中中读取像素。  
        RenderTexture.active = rt;  
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);  
        screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        screenShot.Apply();  

        // 重置相关参数，以使用camera继续在屏幕上显示  
        camera.targetTexture = null;  
        //ps: camera2.targetTexture = null;  
        RenderTexture.active = null; // JC: added to avoid errors  
        GameObject.Destroy(rt);  
        // 最后将这些纹理数据，成一个png图片文件  
        byte[] bytes = screenShot.EncodeToPNG();  
        string filename = Application.dataPath + "/Screenshot.png";  
        System.IO.File.WriteAllBytes(filename, bytes);  
        Debug.Log(string.Format("截屏了一张照片: {0}", filename));  

        return screenShot;  
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(300, 100, 200, 100), "Refresh"))
        {
            Refresh();
        }
    }
}
