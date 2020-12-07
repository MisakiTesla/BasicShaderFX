using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader shader;
    Material _mat;
    public Texture _mask;
    int kernelHandle;
    int[] DestRect;
    public RenderTexture tex;
    public Texture2D copyTex;
    public Text tt;
    GraphicsFormat format;
    void Awake()
    {
        DestRect = new int[4] { 0, 0, 256, 256 };
    }
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        format = GraphicsFormat.RGBA_ETC2_UNorm;
        shader.EnableKeyword("_COMPRESS_ETC2");
#else
        format = GraphicsFormat.RGBA_DXT5_UNorm;
        //shader.DisableKeyword("_COMPRESS_ETC2");
#endif
        kernelHandle = shader.FindKernel("CSMain");
        tex = new RenderTexture(64, 64, 24)
        {
            format = RenderTextureFormat.ARGB32,
            enableRandomWrite = true,
        };
        tex.Create();
        //tt.text = format.ToString() + SystemInfo.IsFormatSupported(format, FormatUsage.Linear).ToString() + SystemInfo.supportsComputeShaders + SystemInfo.copyTextureSupport;

        shader.SetTexture(kernelHandle, "Result", tex);
        shader.SetTexture(kernelHandle, "RenderTexture0", _mask);
        shader.SetInts("DestRect", DestRect);
        shader.Dispatch(kernelHandle, (256 / 4 + 7) / 8, (256 / 4 + 7) / 8, 1);
        copyTex = new Texture2D(256, 256, format, TextureCreationFlags.None);
        Graphics.CopyTexture(tex, 0,0,0,0,64,64,copyTex,0,0,0,0);
        _mat = GetComponent<MeshRenderer>().sharedMaterial;
        _mat.mainTexture = copyTex;
    }
}