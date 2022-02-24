using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
  [Header("Movement")]
  [SerializeField] float xSpeed = 5;
  [SerializeField] float initialYSpeed = 3;
  [SerializeField] float Height = 5;
  [SerializeField] float JumpTime = 3;
  [Header("Physics")]
  [SerializeField] LayerMask wallLayer;
  [SerializeField] float skinWidth = 0.001f;


  private Rigidbody2D rb;
  private float velx, vely;
  private float radius;
  RaycastHit2D[] hits = new RaycastHit2D[16];
  private float v_0;
  private float g;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.isKinematic = true;
    if (velx == 0)
      velx = xSpeed;
    vely = 0;
    var circle = GetComponentInChildren<CircleCollider2D>();
    radius = circle.radius * circle.transform.lossyScale.x;

    var T = JumpTime / 2;
    g = -(2f * Height) / (T * T);
    v_0 = 2f * Height / T;
    vely = initialYSpeed;
  }

  public void MoveRight(bool yes)
  {
    velx = yes ? xSpeed : -xSpeed;
  }

  private void FixedUpdate()
  {
    int count;
    //vely += Physics2D.gravity.y * Time.deltaTime;
    vely += g * Time.deltaTime;
    var move_x = velx * Time.deltaTime;
    var move_y = vely * Time.deltaTime;

    count = Physics2D.CircleCastNonAlloc(transform.position, radius, new Vector2(move_x, 0), hits, move_x, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal.x > 0 && move_x < 0)
      {
        move_x = -(hits[i].distance - skinWidth);
        velx = -velx;
      }
      else if (hits[i].normal.x < 0 && move_x > 0)
      {
        move_x = (hits[i].distance - skinWidth);
        velx = -velx;
      }
    }

    count = Physics2D.CircleCastNonAlloc(transform.position, radius, new Vector2(move_y, 0), hits, move_y, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal.y > 0 && move_y < 0)
      {
        //we hit floor
        move_y = -(hits[i].distance - skinWidth);
        vely = v_0;
      }
      else if (hits[i].normal.y < 0 && move_y > 0)
      {
        // hit ceiling
        vely = -vely;
        move_y = hits[i].distance - skinWidth;
      }
    }

    rb.MovePosition(rb.position + new Vector2(move_x, move_y));
  }
}
