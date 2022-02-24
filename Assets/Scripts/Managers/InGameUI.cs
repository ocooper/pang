using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
  [SerializeField] TMPro.TextMeshProUGUI scoreValue;
  [SerializeField] GameObject[] livesIcons;

  void Start()
  {
    scoreValue.text = "0";
    EventBus.Instance.Register("set-score", ShowScore);
    EventBus.Instance.Register("set-lives", ShowLives);
  }

  private void OnDestroy()
  {
    EventBus.Instance.Remove("set-score", ShowScore);
  }

  void ShowLives(object sender, EventArgs args)
  {
    int left = (args as PlayerHealthMsg).lives_left;
    for (int i = 0; i < livesIcons.Length; i++)
      livesIcons[i].SetActive(i < left);
  }

  void ShowScore(object sender, EventArgs args)
  {
    scoreValue.text = (args as ScoreMessage).score.ToString();
  }
}
