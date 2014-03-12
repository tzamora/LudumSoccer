using UnityEngine;
using System.Collections;

public class GoalTriggerController : MonoBehaviour {

	public TeamType TeamSource;

	public bool ColliderEnabled = true;

	void Awake()
	{
		GameContext.Get.GameEvent.ToggleGoalTriggers += ToggleGoalTriggerHandler;
	}

	void ToggleGoalTriggerHandler(bool toggle)
	{
		ColliderEnabled = toggle;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(!ColliderEnabled)
		{
			return;
		}

		BallController ball = other.GetComponent<BallController>();

		if(ball != null)
		{
			//
			// START GOAL ROUTINE!! GO MY DEAR CODE ... RUN! RUN!
			//

			//Debug.Log("esto deberia de llamar solo una vez");

			GameContext.Get.FSM.GoalState.StartRoutine(TeamSource);

			//
			// disable the triggers
			//

			ColliderEnabled = false;
		}
	}
}
