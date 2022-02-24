using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
  [SerializeField] float speed;
  [SerializeField] LayerMask walls;
  [SerializeField] LayerMask enemies;
  Rigidbody2D rb;
  Collider2D[] colliders = new Collider2D[64];

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    rb.isKinematic = true;
  }

  private void FixedUpdate()
  {
    rb.MovePosition(rb.position + new Vector2(0, speed * Time.deltaTime));
    var filter = new ContactFilter2D { useLayerMask = true, layerMask = walls | enemies };
    var count = rb.OverlapCollider(filter, colliders);
    for (int i = 0; i < count; i++)
    {
      if (colliders[i].gameObject.name == "Ceiling")
      {
        Die();
      }
      //if ((1 << colliders[i].gameObject.layer) == enemies.value)
      {
        if (colliders[i].attachedRigidbody == null)
        {
          if (colliders[i].gameObject.TryGetComponent<Breakable>(out var br))
          {
            br.TakeHit();
            Die();
          }
        }
        else
        {
          if (colliders[i].attachedRigidbody.TryGetComponent<Breakable>(out var br))
          {
            br.TakeHit();
            Die();
          }
        }
        //Die();
      }
    }
  }
  private void Die()
  {
    Destroy(gameObject);
    EventBus.Instance.Send("projectile-die");
  }
}
