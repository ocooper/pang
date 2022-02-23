using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BallMovement : MonoBehaviour
{
  [SerializeField]
  float xSpeed = 5;
  [SerializeField]
  float maxHeightAfterJump = 4;
  [SerializeField]
  float maxYVelocity = 3;
  [SerializeField]
  LayerMask wallLayer;
  [SerializeField]
  float skinWidth = 0.001f;


  private Rigidbody2D rb;
  private ContactPoint2D[] contacts = new ContactPoint2D[64];
  private float velx, vely;
  private float radius;
  RaycastHit2D[] hits = new RaycastHit2D[16];
  int layermask_others;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.isKinematic = true;
    if (velx == 0)
      velx = xSpeed;
    vely = 0;
    var circle = GetComponentInChildren<CircleCollider2D>();
    radius = circle.radius * circle.transform.lossyScale.x;
    layermask_others = ~(1 << gameObject.layer);
  }

  public void MoveRight(bool yes)
  {
    velx = yes ? xSpeed : -xSpeed;
  }

  private void FixedUpdate()
  {
    int count;
    vely += Physics2D.gravity.y * Time.deltaTime;
    var move_x = velx * Time.deltaTime;
    var move_y = vely * Time.deltaTime;
    count = Physics2D.CircleCastNonAlloc(transform.position, radius, new Vector2(move_x, 0), hits, move_x, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal.x != 0)
      {
        move_x += (move_x - hits[i].distance * Mathf.Sign(move_x));
        if (hits[i].normal.x * velx < 0)
          velx = -velx;
      }
    }
    count = Physics2D.CircleCastNonAlloc(transform.position, radius, new Vector2(move_y, 0), hits, move_y, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal.y != 0)
      {
        move_y += (move_y - hits[i].distance * Mathf.Sign(move_y));
        if (hits[i].normal.y * vely < 0)
          vely = -vely;
      }
    }
    rb.MovePosition(rb.position + new Vector2(move_x, move_y) * (1 - skinWidth));
  }
}
