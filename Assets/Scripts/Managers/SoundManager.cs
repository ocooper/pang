using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
  [SerializeField] AudioMixerSnapshot mainMenu;
  [SerializeField] AudioMixerSnapshot inGame;
  [SerializeField, Range(0, 2)] float transitionTime;

  private void Start()
  {
    EventBus.Instance.Register("level-start", (sender, args) => inGame.TransitionTo(transitionTime));
    EventBus.Instance.Register("loading-main-menu", (sender, args) => mainMenu.TransitionTo(transitionTime));
    mainMenu.TransitionTo(transitionTime);
  }
}
