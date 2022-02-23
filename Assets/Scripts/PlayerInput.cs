using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
  [SerializeField] string horizontalAxis;
  [SerializeField] string fireButton;
  [Space]
  [SerializeField] PlayerController controller;

  void Start()
  {
    if (controller == null)
      controller = GetComponent<PlayerController>();
    if (controller == null)
      this.enabled = false;
  }

  void Update()
  {
    var horiz = Input.GetAxisRaw(horizontalAxis) * transform.right;
    var fire = Input.GetButton(fireButton);
    controller.fire = fire;
    controller.movementInput = horiz;
  }
}
