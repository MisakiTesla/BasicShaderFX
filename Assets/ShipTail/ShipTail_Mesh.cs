using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class ShipTail_Mesh : MonoBehaviour
{
    public float velocity = 1;
    public float waveWidth = 1;
    private List<Vector3> _trailPosList = new List<Vector3>();


    public GameObject tail;
    private MeshFilter _meshFilter;


    //Mesh
    private Mesh _mesh;
    private List<Vector3> _vertices = new List<Vector3>();
    private List<int> _triangles = new List<int>();
    private List<Vector2> _uvs = new List<Vector2>();

    // Start is called before the first frame update
    void Awake()
    {
        _meshFilter = tail.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {

        var currentOffset = -transform.forward * Time.deltaTime * velocity;

        for (int i = 0; i < _trailPosList.Count; i++)
        {
            _trailPosList[i] += currentOffset;

        }

        _trailPosList.Add(Vector3.zero);


        if (_trailPosList.Count > 200)
        {
            _trailPosList.RemoveAt(0);
        }


        var offset = transform.right * waveWidth;
        _vertices.Clear();
        foreach (var item in _trailPosList)
        {
            _vertices.Add(item + offset);
            _vertices.Add(item - offset);
        }
        UpdateMesh();

    }

    void UpdateMesh()
    {
        Debug.Log("Updating Mesh");
        _mesh = new Mesh();

        int triangleAmount = _vertices.Count - 2;
        _triangles = new List<int>(3 * triangleAmount);
        //根据三角形的个数，来计算绘制三角形的顶点顺序（索引）
        //顺序必须为顺时针或者逆时针
        for (int i = 0; i < triangleAmount;)
        {
            _triangles.Add(i);
            _triangles.Add(i + 1);
            _triangles.Add(i + 2);

            _triangles.Add(i + 2);
            _triangles.Add(i + 1);
            _triangles.Add(i + 3);

            i += 2;
        }
        _uvs.Clear();
        for (int i = 0; i < _vertices.Count; )
        {
            _uvs.Add(new Vector2(i / (_vertices.Count / 2), 0));
            _uvs.Add(new Vector2(i / (_vertices.Count / 2), 1));
            i += 2;
        }

        _mesh.vertices = _vertices.ToArray();
        _mesh.triangles = _triangles.ToArray();
        _mesh.uv = _uvs.ToArray();
        _meshFilter.mesh = _mesh;

    }
}
