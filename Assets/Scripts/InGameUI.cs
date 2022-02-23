using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
  [SerializeField] TMPro.TextMeshProUGUI scoreValue;
  void Start()
  {
    scoreValue.text = "0";
    EventBus.Instance.Register("set-score", ShowScore);
  }

  private void OnDestroy()
  {
    EventBus.Instance.Remove("set-score", ShowScore);
  }

  void ShowScore(object sender, EventArgs args)
  {
    scoreValue.text = (args as ScoreMessage).score.ToString();
  }
}
