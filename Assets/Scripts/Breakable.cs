using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
  [SerializeField] GameObject breaksInto;
  [SerializeField] int score;

  public void TakeHit()
  {
    if (score > 0)
      EventBus.Instance.Send("add-score", null, new ScoreMessage { score = this.score, hitPosition = transform.position });
    if (breaksInto != null)
    {
      var piece1 = Instantiate(breaksInto, transform.position, Quaternion.identity);
      var piece2 = Instantiate(breaksInto, transform.position, Quaternion.identity);
      if (piece1.TryGetComponent<BallMovement>(out var move_1))
        move_1.MoveRight(true);
      if (piece2.TryGetComponent<BallMovement>(out var move_2))
        move_2.MoveRight(false);
    }
    Destroy(gameObject);
  }
}
