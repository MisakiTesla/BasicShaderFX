using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUInstancingProp : MonoBehaviour
{
    public List<GameObject> objects;
    private void OnGUI()
    {
        if (GUI.Button(new Rect((Screen.width - 200)/2 , Screen.height - 300, 200, 100), $"SetProp"))
        {
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            MeshRenderer renderer;

            foreach (GameObject obj in objects)
            {
                float r = Random.Range(0.0f, 1.0f);
                float g = Random.Range(0.0f, 1.0f);
                float b = Random.Range(0.0f, 1.0f);
                props.SetColor("_TestProp", new Color(r, g, b));
   
                renderer = obj.GetComponent<MeshRenderer>();
                renderer.SetPropertyBlock(props);
            }

        }
    }

}
