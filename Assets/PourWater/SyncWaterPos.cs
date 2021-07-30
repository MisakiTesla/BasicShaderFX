using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Graphic))]
public class SyncWaterPos : MonoBehaviour
{
   public readonly int worldPosBottomID = Shader.PropertyToID("_WorldPosBottom");
   public readonly int worldPosTopID = Shader.PropertyToID("_WorldPosTop");

   public Mask mask;
   
   // public readonly int height0ID = Shader.PropertyToID("_Height0");
   // public readonly int height1ID = Shader.PropertyToID("_Height1");
   // public readonly int height2ID = Shader.PropertyToID("_Height2");
   // public readonly int height3ID = Shader.PropertyToID("_Height3");
   //
   // [Range(0,1)]
   // public float height0;
   //
   // [Range(0,1)]
   // public float height1;
   //
   // [Range(0,1)]
   // public float height2;
   //
   // [Range(0,1)]
   // public float height3;

   
   [SerializeField] private Material _mat;
   [SerializeField] private Vector3[] _corners;
   [SerializeField] private RectTransform _rectTransform;
   
   //cache for performance
   private Matrix4x4 _lastLocalToWorldMatrix;

   private void Awake()
   {
      _rectTransform = transform as RectTransform;
      _mat = GetComponent<Graphic>().material;
      _corners = new Vector3[4];
      _lastLocalToWorldMatrix = transform.localToWorldMatrix;
   }

   private void Update()
   {
      if (_lastLocalToWorldMatrix != transform.localToWorldMatrix)
      {
         _lastLocalToWorldMatrix = transform.localToWorldMatrix;
         Debug.Log($"Pos Change");
         
         _rectTransform.GetWorldCorners(_corners);
         _mat.SetVector(worldPosBottomID, (_corners[0] + _corners[3])/2f);
         _mat.SetVector(worldPosTopID, (_corners[1] + _corners[2])/2f);
         // mask.GetComponent<Graphic>().SetMaterialDirty();
      
         MaskUtilities.NotifyStencilStateChanged((Component) mask);
         // _mat.SetFloat(height0ID,_h);
      }

   }
}
