using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pixel
{
    public bool isIn;
    public float distance;
}

public class SDFTexGenerator : MonoBehaviour
{
    public Texture2D src;
    public Texture2D dest;
    public RawImage rawImageDest;
    public RawImage rawImageDestWithShader;
    private readonly string _yourFile = "/UIGlow/computeTex.png"; //路径记得要修改！注意该路径斜线是向左的，和电脑里文件将爱路径相反

    private void OnGUI()
    {
        if (GUI.Button(new Rect(300, Screen.height - 100, 200, 100), "Generate"))
        {
            dest = new Texture2D (64, 64, TextureFormat.Alpha8, false);
            GenerateSDF(src,dest);
            rawImageDestWithShader.texture = dest;
            rawImageDest.texture = dest;
        }
        if (GUI.Button(new Rect(300, Screen.height - 200, 200, 100), "Save"))
        {
            if (dest)
            {
                var savePath = Path.Combine(Application.dataPath + _yourFile);
                saveTexture2D(dest, savePath);
            }
        }
    }

    public static void GenerateSDF(Texture2D source, Texture2D destination)
    {
        int sourceWidth = source.width;
        int sourceHeight = source.height;
        int targetWidth = destination.width;
        int targetHeight = destination.height;

        var pixels = new Pixel[sourceWidth, sourceHeight];
        var targetPixels = new Pixel[targetWidth, targetHeight];
        
        int x, y;
        Color targetColor = Color.white;
        for (x = 0; x < sourceWidth; x++)
        {
            for (y = 0; y < sourceHeight; y++)
            {
                pixels[x, y] = new Pixel();
                if (source.GetPixel(x, y).a >= 1f)
                    pixels[x, y].isIn = true;
                else
                    pixels[x, y].isIn = false;
            }
        }


        float gapX = (sourceWidth / (float)targetWidth);
        float gapY = (sourceHeight / (float)targetHeight);
        Debug.Log($"{sourceWidth}_{targetWidth}_{gapX},{sourceHeight}_{targetHeight}_{gapY}");
        
        int MAX_SEARCH_DIST = 512;
        int minx, maxx, miny, maxy;
        float max_distance = -MAX_SEARCH_DIST;
        float min_distance = MAX_SEARCH_DIST;

        for (x = 0; x < targetWidth; x++)
        {
            for (y = 0; y < targetHeight; y++)
            {
                targetPixels[x, y] = new Pixel();
                int sourceX = (int)(x * gapX);
                int sourceY = (int)(y * gapY);
                int min = MAX_SEARCH_DIST;
                minx = sourceX - MAX_SEARCH_DIST;
                if (minx < 0)
                {
                    minx = 0;
                }

                miny = sourceY - MAX_SEARCH_DIST;
                if (miny < 0)
                {
                    miny = 0;
                }

                maxx = sourceX + MAX_SEARCH_DIST;
                if (maxx > (int) sourceWidth)
                {
                    maxx = sourceWidth;
                }

                maxy = sourceY + MAX_SEARCH_DIST;
                if (maxy > (int) sourceHeight)
                {
                    maxy = sourceHeight;
                }

                int dx, dy, iy, ix, distance;
                bool sourceIsInside = pixels[sourceX, sourceY].isIn;
                if (sourceIsInside)
                {
                    for (iy = miny; iy < maxy; iy++)
                    {
                        dy = iy - sourceY;
                        dy *= dy;
                        for (ix = minx; ix < maxx; ix++)
                        {
                            bool targetIsInside = pixels[ix, iy].isIn;
                            if (targetIsInside)
                            {
                                continue;
                            }

                            dx = ix - sourceX;
                            distance = (int) Mathf.Sqrt(dx * dx + dy);
                            if (distance < min)
                            {
                                min = distance;
                            }
                        }
                    }

                    if (min > max_distance)
                    {
                        max_distance = min;
                    }

                    targetPixels[x, y].distance = min;
                }
                else
                {
                    for (iy = miny; iy < maxy; iy++)
                    {
                        dy = iy - sourceY;
                        dy *= dy;
                        for (ix = minx; ix < maxx; ix++)
                        {
                            bool targetIsInside = pixels[ix, iy].isIn;
                            if (!targetIsInside)
                            {
                                continue;
                            }

                            dx = ix - sourceX;
                            distance = (int) Mathf.Sqrt(dx * dx + dy);
                            if (distance < min)
                            {
                                min = distance;
                            }
                        }
                    }

                    if (-min < min_distance)
                    {
                        min_distance = -min;
                    }

                    targetPixels[x, y].distance = -min;
                }
            }
        }

        //EXPORT texture
        float clampDist = max_distance - min_distance;
        for (x = 0; x < targetWidth; x++)
        {
            for (y = 0; y < targetHeight; y++)
            {
                targetPixels[x, y].distance -= min_distance;
                float value = targetPixels[x, y].distance / clampDist;
                destination.SetPixel(x, y, new Color(1, 1, 1, value));
                destination.Apply(true);
            }
        }
    }
    
    public static void saveTexture2D (Texture2D texture, string path) {
        byte[] bytes = texture.EncodeToPNG ();
        //UnityEngine.Object.Destroy (texture);
        System.IO.File.WriteAllBytes (path, bytes);
        Debug.Log ("write to File over");
        UnityEditor.AssetDatabase.Refresh (); //自动刷新资源
    }

}