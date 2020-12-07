using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShipTail : MonoBehaviour
{
    public float velocity = 1;
    private LineRenderer _lineRender;
    private List<Vector3> _trailPosList = new List<Vector3>();

    private Vector3 _lastPos;

    // Start is called before the first frame update
    void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        var currentOffset = -transform.forward * Time.deltaTime * velocity;

        for (int i = 0; i < _trailPosList.Count; i++)
        {
            _trailPosList[i] += currentOffset;
        }
        Debug.Log(_trailPosList.Count);
        _trailPosList.Add(transform.position);

        if (_trailPosList.Count > 200)
        {
            _trailPosList.RemoveAt(0);
        }

        _lineRender.SetPositions(_trailPosList.ToArray());
        _lineRender.positionCount = _trailPosList.Count;
    }
}
