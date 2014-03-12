using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public float speed = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//
		// get the distance between this player and the ball
		//

		Vector3 playerPos = transform.position;

		Vector3 ballPos = GameContext.Get.MainGameBall.transform.position;

		Debug.DrawLine(playerPos, ballPos);

		float distance = Vector3.Distance(playerPos, ballPos);

		//Debug.Log("vienen las distancias" + distance);

		Camera.main.gameObject.GetComponent<tk2dCamera>().ZoomFactor = 2.5f - (distance * 0.2f);

		//
		//
		//

		Vector3 pos = (playerPos + ballPos) / 2;

		if(pos == Vector3.zero)
		{
			pos = ballPos;
		}

		Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(pos.x,pos.y, Camera.main.transform.position.z), speed * Time.deltaTime);
	} 
}
