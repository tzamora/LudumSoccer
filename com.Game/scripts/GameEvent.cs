using UnityEngine;
using System.Collections;
using System;

public class GameEvent
{
	//
	// put here all the centric events of the game
	// since this is a ludum dare, I hope this class
	// doesnt grow too much
	// 

	/// <summary>
	/// Raises the goal event.
	/// </summary>
	/// <typeparam name="TeamType">The 1st type parameter.</typeparam>
	public Action<TeamType> OnGoal;

	/// <summary>
	/// Raises the show goal alert event
	/// </summary>
	public Action ShowGoalAlert;

	/// <summary>
	/// Raises the restart match event.
	/// restarts the match usually used to reset
	/// the position of the players when starting a
	/// game period
	/// </summary>
	public Action<Action> OnRestartPosition;

	/// <summary>
	/// The toggle characters movement.
	/// </summary>
	public Action<bool> ToggleCharactersMovement;

	/// <summary>
	/// Dos the jump animation on the winner team
	/// </summary>
	/// <returns>The jump animation.</returns>
	public Action<bool> ToggleJumpAnimation;

	/// <summary>
	/// Toggle goal triggers.
	/// </summary>
	public Action<bool> ToggleGoalTriggers;
}
