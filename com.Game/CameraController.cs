using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () 
	{
		//
		// make the camera follow the player
		//

		target = GameContext3D.Get.MainPlayer.transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		FollowTarget();
	}

	void FollowTarget()
	{
		transform.position = new Vector3(target.position.x, transform.position.y, target.position.z - 15f);
	}
}
