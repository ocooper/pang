using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
  [SerializeField] int playerNumber;
  [SerializeField] float speed;
  [SerializeField] float skinWidth = 0.01f;
  [Space]
  [SerializeField] LayerMask wallLayer;
  [SerializeField] LayerMask enemyLayer;

  public Weapon weapon;

  float movementInput => InputSystem.Instance.horizontal(playerNumber);
  bool fire => InputSystem.Instance.fire(playerNumber);

  private Rigidbody2D rb;
  private Collider2D[] enemies = new Collider2D[64];
  private RaycastHit2D[] hits = new RaycastHit2D[16];
  private Vector2 boxsize;
  private Health health;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    var box = GetComponentInChildren<BoxCollider2D>();
    boxsize = Vector2.Scale(box.size, box.transform.lossyScale);
    health = GetComponent<Health>();
  }

  void Update()
  {
    if (fire)
    {
      weapon.Fire();
    }
  }

  private void FixedUpdate()
  {
    var move = new Vector2(movementInput * speed * Time.deltaTime, 0);

    int count = Physics2D.BoxCastNonAlloc(transform.position, boxsize, 0, move.normalized, hits, move.magnitude, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal == new Vector2(0, 1)) // ignore floor for now
        continue;
      move += hits[i].normal * (move.magnitude - (hits[i].distance - skinWidth));
    }
    rb.MovePosition(rb.position + move);

    if (health)
    {
      var filter = new ContactFilter2D() { useLayerMask = true, layerMask = enemyLayer };
      count = rb.OverlapCollider(filter, enemies);
      for (int i = 0; i < count; i++)
        health.TakeDamage();
    }
  }
}
