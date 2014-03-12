using UnityEngine;
using System.Collections;

public class FSM 
{
	public GoalStateManager GoalState;

	private MonoBehaviour ParentBehaviour;

	public FSM(MonoBehaviour parentBehaviour)
	{
		GoalState = new GoalStateManager(parentBehaviour);
	}
}
