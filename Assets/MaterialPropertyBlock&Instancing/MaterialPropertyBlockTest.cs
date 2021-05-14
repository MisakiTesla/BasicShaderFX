using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPropertyBlockTest : MonoBehaviour
{
    public bool IsMaterialPorpertyBlock;

    public Color Color1, Color2;
    public float Speed = 1, Offset;
 
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
 
    void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }
 
    void Update()
    {
        if (IsMaterialPorpertyBlock)
        {
            // Get the current value of the material properties in the renderer.
            _renderer.GetPropertyBlock(_propBlock);
            // // Assign our new value.
            // _propBlock.SetColor("_Color", Color.Lerp(Color1, Color2, (Mathf.Sin(Time.time * Speed + Offset) + 1) / 2f));
            _propBlock.SetColor("_Color", new Color(Random.Range(0, 1f), 0, 0));
            _propBlock.SetFloat("_Test", Random.Range(0, 1f));
            // Apply the edited values to the renderer.
            _renderer.SetPropertyBlock(_propBlock);
        }
        else
        {
            _renderer.material.SetColor("_Color", new Color(Random.Range(0, 1f), 0, 0));
        }
    }

}
