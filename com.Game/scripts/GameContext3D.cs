using UnityEngine;
using System.Collections;

public class GameContext3D : MonoSingleton<GameContext3D> {

	public SoccerBallController MainGameBall;

	public Player3DController MainPlayer;

	public CameraController MainCamera;

	public Transform LeftScore;

	public Transform RightScore;

	void Awake()
	{
		//
		// init the game here
		//


	}
}
