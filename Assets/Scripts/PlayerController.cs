using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
  [SerializeField] float speed;
  [SerializeField] float skinWidth = 0.01f;
  [Space]
  [SerializeField] LayerMask wallLayer;
  [SerializeField] LayerMask enemyLayer;
  public Weapon weapon;

  [HideInInspector]
  public Vector2 movementInput;
  [HideInInspector]
  public bool fire;

  private Rigidbody2D rb;
  private Collider2D[] enemies = new Collider2D[64];
  private RaycastHit2D[] hits = new RaycastHit2D[16];
  private Vector2 boxsize;

  private void Awake()
  {
    rb = GetComponent<Rigidbody2D>();
    var box = GetComponentInChildren<BoxCollider2D>();
    boxsize = Vector2.Scale(box.size, box.transform.lossyScale);
  }

  void Update()
  {
    if (fire)
    {
      weapon.Fire();
      fire = false;
    }
  }

  private void FixedUpdate()
  {
    movementInput.y = 0; // no jumping for now
    var move = movementInput * speed * Time.deltaTime;

    int count = Physics2D.BoxCastNonAlloc(transform.position, boxsize, 0, move.normalized, hits, move.magnitude, wallLayer);
    for (int i = 0; i < count; i++)
    {
      if (hits[i].normal == new Vector2(0, 1)) // ignore floor for now
        continue;
      move += hits[i].normal * (move.magnitude - hits[i].distance);
    }
    rb.MovePosition(rb.position + move * (1 - skinWidth));

    var filter = new ContactFilter2D() { useLayerMask = true, layerMask = enemyLayer };
    count = rb.OverlapCollider(filter, enemies);
    for (int i = 0; i < count; i++)
      TakeDamage();
  }

  private void TakeDamage()
  {
    print("boom!!");
  }
}
