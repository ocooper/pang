using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameUI : MonoBehaviour
{
  [SerializeField] TMPro.TextMeshProUGUI scoreValue;
  [SerializeField] GameObject[] livesIconsPlayer1;
  [SerializeField] GameObject[] livesIconsPlayer2;
  [SerializeField] GameObject mobileTouchControls;

  void Start()
  {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_EDITOR
  mobileTouchControls.SetActive(true);
#else
  mobileTouchControls.SetActive(false);
#endif

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
    int player = (args as PlayerHealthMsg).player;
    int left = (args as PlayerHealthMsg).lives_left;
    var icons = player == 1 ? livesIconsPlayer1 : livesIconsPlayer2;
    for (int i = 0; i < icons.Length; i++)
      icons[i].SetActive(i < left);
  }

  void ShowScore(object sender, EventArgs args)
  {
    scoreValue.text = (args as ScoreMessage).score.ToString();
  }
}
