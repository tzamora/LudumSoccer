using UnityEngine;
using System.Collections;

public class FollowBal : MonoBehaviour {

	private Transform ballTransform;

	void Start()
	{
		ballTransform = GameContext3D.Get.MainGameBall.transform;
	
		print("Balltransform " + ballTransform);
	}

	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(ballTransform.position.x,0f,ballTransform.position.z);
	}
}
