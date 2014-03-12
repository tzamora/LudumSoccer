using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour 
{
	public virtual float GetHorizontalAxis()
	{
		return 0f;
	}
	
	public virtual float GetVerticalAxis()
	{
		return 0f;
	}
	
	public virtual bool Kick()
	{
		return false;
	}

	public virtual bool Pass()
	{
		return false;
	}
}
