using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundImage : MonoBehaviour
{
  [SerializeField] Camera cam;
  void Start()
  {
    if (cam == null)
      cam = Camera.main;

    var renderer = GetComponent<SpriteRenderer>();
    var ppu = renderer.sprite.pixelsPerUnit;
    var size = renderer.sprite.rect.size;

    var aspect = Screen.width / Screen.height;

    var sprite_width_in_units = size.x / ppu;
    var sprite_height_in_units = size.y / ppu;

    var scale_x = cam.orthographicSize * 2 / sprite_height_in_units;
    var scale_y = (cam.orthographicSize * aspect) * 2 / sprite_width_in_units;
    var s = Mathf.Max(scale_x, scale_y);
    transform.localScale = new Vector3(s, s, 1);
  }
}
