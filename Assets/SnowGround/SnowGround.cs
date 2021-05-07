using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowGround : MonoBehaviour
{
    public RenderTexture rt;
    public RenderTexture rtTmp;

    public Texture drawTexture;
    public Texture defaultTexture;
    public Material stampMat;
    public Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        GetComponent<Renderer>().material.mainTexture = rt;
        DrawDefault();
        rtTmp = RenderTexture.GetTemporary(rt.width,rt.height,32,rt.format);
    }
    
    private void DrawDefault()
    {
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);

        var rect = new Rect(0, 0, rt.width, rt.height);
        Graphics.DrawTexture(rect, defaultTexture);
        GL.PopMatrix();
        RenderTexture.active = null;
    }

    private void Draw(int x, int y)
    {
        Graphics.Blit(rt,rtTmp);
        RenderTexture.active = rt;
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);
        //修正绘制中心点
        x -= drawTexture.width >> 1;
        y -= drawTexture.height >> 1;
        
        var rect = new Rect(x, y, drawTexture.width, drawTexture.height);

        Vector4 sourceUV = Vector4.zero;
        //offset
        sourceUV.x = rect.x / rt.width;
        sourceUV.y = 1 - (rect.y / rt.height);
        //tilling
        sourceUV.z = rect.width / rt.width;
        sourceUV.w = rect.height / rt.height;
        sourceUV.y -= sourceUV.w;
        stampMat.SetTexture("_SourceTex",rtTmp);
        stampMat.SetVector("_SourceUV",sourceUV);
        Graphics.DrawTexture(rect, drawTexture, stampMat);
        GL.PopMatrix();
        RenderTexture.active = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"Click{hit.transform.name}");
                int x = (int) (hit.textureCoord.x * rt.width);
                //Graphics和uv y轴坐标相反
                int y = (int) (rt.height - hit.textureCoord.y * rt.height);
                Draw(x,y);
                
            }
        }
    }
}
