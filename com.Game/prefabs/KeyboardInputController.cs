using UnityEngine;
using System.Collections;
using System;

public class KeyboardInputController : InputController
{
	override public float GetHorizontalAxis()
	{
		return Input.GetAxis("Horizontal");
	}

	override public float GetVerticalAxis()
	{
		return Input.GetAxis("Vertical");
	}

	override public bool Kick()
	{
		return Input.GetKeyDown(KeyCode.K);
	}

	public override bool Pass ()
	{
		return Input.GetKeyDown(KeyCode.O);
	}
}
