using UnityEngine;
using System.Collections;

public class GameContext : MonoSingleton<GameContext> {

	public BallController MainGameBall;

	public Transform LeftScore;

	public Transform RightScore;

	private GameEvent _gameEvent;

	public MatchVO Match;

	public FSM FSM;

	void Awake()
	{
		FSM = new FSM(this);
	}

	public GameEvent GameEvent {
		get {

			if(_gameEvent == null)
			{
				_gameEvent = new GameEvent();
			}

			return _gameEvent;
		}
	}
}
