using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    private Mesh _mesh;
    private Vector3[] _vertices;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = new Mesh();
        _vertices = new[] {Vector3.zero, Vector3.up, Vector3.right};

        _mesh.vertices = _vertices;
        _mesh.triangles = new[] {0, 1, 2};
        
        Debug.Log($"{_mesh.GetInstanceID()}_mesh");
        Debug.Log($"{_meshFilter.mesh.GetInstanceID()}_meshFilter");

        _meshFilter.mesh = _mesh;
        Debug.Log($"{_meshFilter.mesh == _mesh}");
        
        Debug.Log($"{_mesh.GetInstanceID()}_mesh");
        Debug.Log($"{_meshFilter.mesh.GetInstanceID()}_meshFilter");

        
        _meshFilter.mesh = _mesh;
        Debug.Log($"{_meshFilter.mesh == _mesh}");
        
        Debug.Log($"_mesh{_mesh.GetInstanceID()}");
        Debug.Log($"_meshFilter.mesh{_meshFilter.mesh.GetInstanceID()}");
        Debug.Log($"_meshFilter.sharedMesh{_meshFilter.sharedMesh.GetInstanceID()}");
    }

    // Update is called once per frame
    void Update()
    {

        _vertices[0] += Vector3.left *Mathf.Sin(Time.realtimeSinceStartup);

        _mesh.vertices = _vertices;
        Debug.Log($"{_mesh.GetInstanceID()}");
        Debug.Log($"{_meshFilter.mesh.GetInstanceID()}");
        _meshFilter.mesh.vertices = _vertices;
        Debug.Log($"{_meshFilter.mesh.GetInstanceID()}");
        var vertices = _vertices;
        Debug.Log($"{_meshFilter.mesh.GetInstanceID()}");



        // var mesh = _meshFilter.mesh;
        if (Input.GetKeyDown("a"))
        {
            _meshFilter.mesh = _mesh;
        }

    }
    
}
