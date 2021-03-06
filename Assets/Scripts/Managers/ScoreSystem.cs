using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScoreMessage : EventArgs
{
  public int score;
  public Vector3 hitPosition;
}

public class ScoreSystem : MonoBehaviour
{
  int score;
  int highscore;

  private void Start()
  {
    EventBus.Instance.Register("add-score", AddScore);
    EventBus.Instance.Register("game-start", ResetScore);
    EventBus.Instance.Register("level-start", UpdateUI);
  }

  private void ResetScore(object sender, EventArgs e)
  {
    score = 0;
  }

  private void UpdateUI(object sender, EventArgs e)
  {
    EventBus.Instance.Send("set-score", null, new ScoreMessage { score = score });
  }
  private void AddScore(object sender, EventArgs e)
  {
    score += (e as ScoreMessage).score;
    highscore = Mathf.Max(score, highscore);
    EventBus.Instance.Send("set-score", null, new ScoreMessage { score = score });
  }
}
