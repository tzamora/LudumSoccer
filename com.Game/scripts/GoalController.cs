using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour 
{
	public TriggerListener GoalTrigger;

	public ExploderObject ExplosiveWall;

	// Use this for initialization
	void Start () 
	{
		GoalTrigger.OnEnter += OnEnter;
	}

	void OnEnter(Collider other)
	{
		SoccerBallController soccerBall = other.GetComponent<SoccerBallController>();

		if(soccerBall!=null)
		{
			//soccerBall.GetComponent<Rigidbody>().velocity = new Vector3(10f, 0f, 0f);

			ExplosiveWall.Explode();
		}
	}
}
