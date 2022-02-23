using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class InitWalls : MonoBehaviour
{
  [SerializeField] Transform[] objectsToGoOnLeft;
  [SerializeField] Transform[] objectsToGoOnRight;

  void Start()
  {
    var cam = GetComponent<Camera>();
    var height = cam.orthographicSize;
    var scrAspect = (float)Screen.width / Screen.height;
    var width = (scrAspect * height);
    if (objectsToGoOnLeft != null)
    {
      foreach (var obj in objectsToGoOnLeft)
      {
        obj.localPosition = new Vector3(-width, obj.localPosition.y, obj.localPosition.z);
      }
    }

    if (objectsToGoOnRight != null)
    {
      foreach (var obj in objectsToGoOnRight)
      {
        obj.localPosition = new Vector3(width, obj.localPosition.y, obj.localPosition.z);
      }
    }
  }
}
