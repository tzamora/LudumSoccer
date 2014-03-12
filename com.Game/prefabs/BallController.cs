using UnityEngine;
using System.Collections;
using System;

public class BallController : MonoBehaviour {

	/// <summary>
	/// The current player that has the ball
	/// null if nobody has the ball
	/// </summary>
	/// 
	private PlayerController _currentPlayer;

	/// <summary>
	/// The on parent change event.
	/// tells when it has a new parent and when
	/// it looses its parent
	/// </summary>
	public Action<PlayerController> OnCurrentPlayerChange;

	// Use this for initialization
	void Awake () 
	{
	
	}
	
	// Update is called once per frame
	void Update () {

		//
		// fixes the bug that makes the ball change of size
		//

		transform.localScale = new Vector3(1f,1f,1f);

        //
        // check if the parent of the ball
        // threw or lost the ball by asking
        // if it still has the ball
        //

        if(CurrentPlayer != null && CurrentPlayer.Ball == null)
        {
            CurrentPlayer = null;
        }
	}

	//
	// GETTERS AND SETTERS
	//

	public PlayerController CurrentPlayer {
		get {
			return _currentPlayer;
		}
		set 
		{
			_currentPlayer = value;

			if(OnCurrentPlayerChange!=null)
			{
				OnCurrentPlayerChange(_currentPlayer);
			}
			
		}
	}
}
