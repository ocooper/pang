using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundImage : MonoBehaviour
{
  [SerializeField] Camera cam;
  [SerializeField, Range(-1,1)] float verticalPivot;
  void Start()
  {
    if (cam == null)
      cam = Camera.main;

    var renderer = GetComponent<SpriteRenderer>();
    var ppu = renderer.sprite.pixelsPerUnit;
    var size_px = renderer.sprite.rect.size;
    var size_units = size_px / ppu;
    var aspect = (float)Screen.width / (float)Screen.height;
    
    var sy = (2 * cam.orthographicSize) / size_units.y;
    var sx = (2 * cam.orthographicSize * aspect) / size_units.x;
    var s = Mathf.Max(sx, sy);
    transform.localScale = new Vector3(s, s, 1);

    // var aspect = Screen.currentResolution.width / Screen.currentResolution.height;
    // var sprite_width_in_units = size.x / ppu;
    // var sprite_height_in_units = size.y / ppu;
    // var sprite_aspect = size.x / size.y;

    // if (sprite_aspect < aspect)
    // {
    //   var s = 2*cam.orthographicSize / sprite_height_in_units;
    //   transform.localScale = new Vector3(s, s, 1);
    // }
    // else
    // {
    //   var s = (2*cam.orthographicSize * aspect) / sprite_width_in_units;
    //   transform.localScale = new Vector3(s, s, 1);
    // }
    transform.localPosition = new Vector3(0, verticalPivot * cam.orthographicSize, 0);
  }
}
