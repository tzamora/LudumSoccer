using UnityEngine;
using System.Collections;

public class GoalStateManager 
{
	private MonoBehaviour ParentBehaviour;

	public GoalStateManager(MonoBehaviour parentBehaviour)
	{
		ParentBehaviour = parentBehaviour;
	}

	//
	// set the actions that occur when a goal is met
	//

	public void StartRoutine(TeamType teamType)
	{
		ParentBehaviour.StartCoroutine(Routine(teamType));
	}

	private IEnumerator Routine(TeamType teamType)
	{
		//
		// stop the characters movement 
		//

		GameContext.Get.GameEvent.ToggleCharactersMovement(false);

		//
		// disable the goal triggers
		//

		GameContext.Get.GameEvent.ToggleGoalTriggers(false);

		//
		// Increase the respective score
		//
		
		// Debug.Log("One point more");
		
		GameContext.Get.Match.AddScore(teamType);

		//
		// first show the goal message 
		//
		
		GameContext.Get.GameEvent.ShowGoalAlert();
		
		//
		// wait a second
		//

		yield return new WaitForSeconds(1f);

		//
		// celebrate & anger wait until it finishes
		//

		//
		// Every one jump !! 
		//

		// GameContext.Get.GameEvent.ToggleJumpAnimation(true);

		// GameContext.Get.GameEvent.GoalCelebration();

		//
		// wait a few moments for the celebration to finish
		//

		// yield return new WaitForSeconds(1f);

		// GameContext.Get.GameEvent.ToggleJumpAnimation(false);

		//
		// restart positions
		//

		GameContext.Get.GameEvent.OnRestartPosition(RestartFinishHandler);

		//
		// put ball in center of field
		//

		GameContext.Get.MainGameBall.transform.position = Vector2.zero;

		//
		// re enable the goal triggers
		//

		GameContext.Get.GameEvent.ToggleGoalTriggers(true);

		yield return null;
	}

	void RestartFinishHandler()
	{
		GameContext.Get.GameEvent.ToggleCharactersMovement(true);
	}
}
