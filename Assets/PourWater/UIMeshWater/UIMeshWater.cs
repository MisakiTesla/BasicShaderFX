using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
public class UIMeshWater : BaseMeshEffect
{
    public Transform impactorTrans;
    public Transform testorTrans;
    [System.Serializable]
    public struct Bound
    {
        public float top;
        public float right;
        public float bottom;
        public float left;
    }

    [Header("Water Settings")]
    public Bound bound;
    public int quality;
    
    private Vector3[] _vertices;
    private int[] _triangles;


    
    [Header("Phisics Settings")]
    public float sprigConstant = .02f;
    public float damping = .1f;
    public float spread = .1f;
    public float collisionVelocityFactor = .04f;

    float[] velocities;
    float[] accelerations;
    float[] leftDeltas;
    float[] rightDeltas;
    
    private float timer;
    private BoxCollider2D _collider2D;


    protected override void Start()
    {
        InitializePhysics();
        GenerateMesh();
        SetBoxCollider2D();
        testorTrans.position = transform.localToWorldMatrix.MultiplyPoint(_vertices[0]);
    }
    
    void InitializePhysics()
    {
        velocities = new float[quality];
        accelerations = new float[quality];
        leftDeltas = new float[quality];
        rightDeltas = new float[quality];
    }

    private void GenerateMesh()
    {
        float range = (bound.right - bound.left) / (quality - 1);
        _vertices = new Vector3[quality * 2];

        for (int i = 0; i < quality; i++)
        {
            _vertices[i] = new Vector3(bound.left + (i * range), bound.top, 0);
        }

        for (int i = 0; i < quality; i++)
        {
            _vertices[i + quality] = new Vector2(bound.left + (i * range), bound.bottom);
        }

        // 0
        // | \
        // |   \
        // |     \
        // |       \
        // |         \
        // |           \
        // |             \
        // quality - quality+1
        int[] template = new int[6];
        template[0] = quality;
        template[1] = 0;
        template[2] = quality + 1;
        template[3] = 0;
        template[4] = 1;
        template[5] = quality + 1;

        int marker = 0;
        _triangles = new int[((quality - 1) * 2) * 3];
        for (int i = 0; i < _triangles.Length; i++)
        {
            _triangles[i] = template[marker++]++;
            if (marker >= 6) marker = 0;
        }

    }

    private void SetBoxCollider2D()
    {
        _collider2D = gameObject.GetComponent<BoxCollider2D>();
        if (!_collider2D)
        {
            _collider2D = gameObject.AddComponent<BoxCollider2D>();
        }
        _collider2D.size = (transform as RectTransform).rect.size;
        _collider2D.isTrigger = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
        Splash(collision, rb.velocity.y * collisionVelocityFactor);
    }
    
    private void Splash(Collider2D collision, float force)
    {
        timer = 3f;
        float radius = collision.bounds.max.x - collision.bounds.min.x;
        Debug.Log($"{radius}");
        
        Vector2 center = new Vector2(collision.bounds.center.x, _collider2D.bounds.max.y);
        
        impactorTrans.position = center;
        
        //Debug.Log("old -- center = " + center + " / radius = " + radius);

        //GameObject splashGO = Instantiate(splash, new Vector3(center.x, center.y, 0), Quaternion.Euler(0, 0, 60));
        //Destroy(splashGO, 2f);

        for (int i = 0; i < quality; i++)
        {
            //Debug.Log("old -- vertices " + i + " = " + vertices[i] + " / globalPosition = " + (vertices[i] + transform.position));
            var pos = transform.localToWorldMatrix.MultiplyPoint(_vertices[i]);
            Debug.Log($"{pos}");
            
            if (PointInsideCircle(pos, center, radius))
            {
                velocities[i] = force;
                Debug.Log("old -- boom");
            }
        }
    }
    
    bool PointInsideCircle(Vector2 point, Vector2 center, float radius)
    {
        var dist = Vector2.Distance(point, center);
        Debug.Log($"{dist}");
        
        return dist < radius;
    }

    private void Update()
    {
        if (timer <= 0) return;
        timer -= Time.deltaTime;

        for (int i = 0; i < quality; i++)
        {
            float force = sprigConstant * (_vertices[i].y - bound.top) + velocities[i] * damping;
            accelerations[i] = -force;
            _vertices[i].y += velocities[i];
            velocities[i] += accelerations[i];
        }

        for (int i = 0; i < quality; i++)
        {
            if (i > 0)
            {
                leftDeltas[i] = spread * (_vertices[i].y - _vertices[i - 1].y);
                velocities[i - 1] += leftDeltas[i];
            }
            if (i < quality - 1)
            {
                rightDeltas[i] = spread * (_vertices[i].y - _vertices[i + 1].y);
                velocities[i + 1] += rightDeltas[i];
            }
        }
        
        this.graphic.SetVerticesDirty();
    }

    public override void ModifyMesh(VertexHelper vh)
    {

        if (_vertices == null || _triangles == null)
        {
            return;
        }
        vh.Clear();
        // Debug.Log($"BME{_meshFilter.mesh.GetInstanceID()}");
        
        // var stream = new List<UIVertex>();
        // vh.GetUIVertexStream(stream);
        Color32 color32 = graphic.color;
        for (var index = 0; index < _vertices.Length; index++)
        {
            var vertex = _vertices[index];
            // Debug.Log($"{vertex}");
            vh.AddVert(new UIVertex()
            {
                uv0 = index < quality ? Vector2.up + ((float)index/quality)*Vector2.right :  Vector2.zero + ((float)(index-quality)/quality)*Vector2.right,
                position = vertex,
                color = color32,
            });
        }

        for (var index = 0; index < _triangles.Length;)
        {
            var triangle1 = _triangles[index++];
            var triangle2 = _triangles[index++];
            var triangle3 = _triangles[index++];
            
            vh.AddTriangle(triangle1,triangle2,triangle3);
        }

    }
}
