using UnityEngine;
using System.Collections;
using System;

public class TriggerListener : MonoBehaviour {

	public Action<Collider> OnEnter;

	public Action<Collider> OnStay;

	public Action<Collider> OnExit;

	void OnTriggerEnter(Collider other)
	{
		if(OnEnter != null)
		{
			OnEnter(other);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(OnStay != null)
		{
			OnStay(other);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(OnExit != null)
		{
			OnExit(other);
		}
	}

}
