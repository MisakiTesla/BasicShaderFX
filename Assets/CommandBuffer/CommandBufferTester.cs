using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CommandBufferTester : MonoBehaviour
{
    public RawImage rawImage;
    public Material mat;
    
    private CommandBuffer _cb;
    

    private Mesh _quad;
    private Matrix4x4 _trsMatrix4X4;
    public RenderTexture _rt;
    private Matrix4x4 _matrixProj;
    public Vector2 _maskSize = Vector2.one*512;

    public bool isExecute;


    // Start is called before the first frame update
    void Start()
    {
        if (!mat)
        {
            mat = new Material(Shader.Find("UI/Default"));
            mat.SetColor("_Color",Color.red);
        }
        _rt = new RenderTexture((int)_maskSize.x, (int)_maskSize.y, 0, RenderTextureFormat.R8, 0);

        _cb = new CommandBuffer();
        _matrixProj = Matrix4x4.Ortho(0, _maskSize.x, 0, _maskSize.y, -1f, 1f);

        _cb.SetViewProjectionMatrices(Matrix4x4.identity, _matrixProj);
        _cb.SetRenderTarget(_rt);

        
        _quad = new Mesh();
        _quad.SetVertices(new List<Vector3>() {new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0)});

        _quad.SetUVs(0, new List<Vector2>()
        {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 0),
            new Vector2(1, 1)
        });

        _quad.SetIndices(new int[] { 0, 1, 2, 3, 2, 1 }, MeshTopology.Triangles, 0, false);
        _quad.UploadMeshData(true);
        
        Vector2 localPt = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, Camera.main, out localPt);


        

    }
    
    private void SetupPaintContext(bool clearRT)
    {
        _cb.Clear();
        _cb.SetRenderTarget(_rt);

        if (clearRT)
        {
            _cb.ClearRenderTarget(true, true, Color.clear);
        }

        _cb.SetViewProjectionMatrices(Matrix4x4.identity, _matrixProj);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            SetupPaintContext(false);

            Vector2 localPt = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition, Camera.main, out localPt);
            // _trsMatrix4X4 = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one*99);
            _trsMatrix4X4 = Matrix4x4.TRS(localPt, Quaternion.identity, Vector3.one*99);
            Debug.Log($"{isExecute}");
            _cb.DrawMesh(_quad, _trsMatrix4X4, mat, 0);
            isExecute = true;
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Vector2.zero, Vector2.one*100),"Add"))
        {

            _cb.DrawMesh(_quad, _trsMatrix4X4, mat, 0);
        }
        if (GUI.Button(new Rect(Vector2.right*100, Vector2.one*100),"Excute"))
        {
            isExecute = true;
        }
    }

    private void LateUpdate()
    {
        if (isExecute)
        {
            isExecute = false;
            Graphics.ExecuteCommandBuffer(_cb);
        }
        
        rawImage.texture = _rt;

    }
}
