using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerController : MonoBehaviour 
{
	public float moveForce = 1f;			// Amount of force added to move the player left and right.

	public float moveSpeed = 5f;

	public float maxSpeed = 5f;

	public bool facingRight = true;

	public Transform BallFeetPivot;

	public BallController Ball;

	public Transform arrowIndicator;

	public float ColliderRadius = 0f;

	public int BallCollisionLayer = 8;

	public float BetweenKicksDelay = 0.3f;

	private bool enableBallCollision = true;

	private bool enableMovement = true;

	public Transform AlignmentPoint;

	public float SlideDistance = 2f;

	tk2dSpriteAnimator spriteAnimator;

	InputController input;

	public bool sliding = false;

	public int LayerToAttack = 0;

	//
	// this factor indicates how much movement having the ball has 
	//

	public float ballPossesionMovementPenalty = 1f;

	public List<PlayerController> Teammates;

	public virtual void Awake()
	{
		GameContext.Get.GameEvent.ToggleCharactersMovement += ToggleCharactersMovementHandler;

		GameContext.Get.GameEvent.ToggleJumpAnimation += ToggleJumpAnimationHandler;

		GameContext.Get.GameEvent.OnRestartPosition += OnRestartPositionHandler;

		//
		// listen when the ball is taken
		//

		GameContext.Get.MainGameBall.OnCurrentPlayerChange += OnCurrentPlayerChangeHandler;

		spriteAnimator = GetComponent<tk2dSpriteAnimator>();

		input = GetComponent<InputController>();

		//
		// fill all my teammates list
		//

		ObtainTeammates();
	}

	private bool toggleJumpAnimation = false;

	void ObtainTeammates()
	{
		//
		// fill all my teammates list
		//

		Teammates = new List<PlayerController>( GameContext.Get.Match.LocalTeam );

		//
		// but remove me from the list
		//

		Teammates.Remove(this);
	}

	void ToggleJumpAnimationHandler(bool toggle)
	{
		toggleJumpAnimation = toggle;

		//
		// only start the coroutine if the toggle is on
		//

		if(toggleJumpAnimation)
		{
			//StartCoroutine(StartJumpAnimation());
		}
	}

	IEnumerator StartJumpAnimation()
	{
		//
		// do a funny jump animation with itween
		// using the punch position feature
		//

		enableMovement = false;

		while(toggleJumpAnimation)
		{
			//iTween.PunchPosition(gameObject,iTween.Hash("y",1,"time",0.3f));
			
			yield return new WaitForSeconds(0.3f);
		}

		enableMovement = true;
	}

	void ToggleCharactersMovementHandler(bool toggle)
	{
		// Debug.Log("Activando el movimiento de los personajes en" + toggle);

		//
		// stop the player from moving when we receive a goal
		//

		enableMovement = toggle;

		enableBallCollision = toggle;

		//
		// if we have the ball remove it
		//

		if(Ball != null)
		{
			Ball.gameObject.transform.parent = null;

			Ball = null;
		}
	}

	void OnCurrentPlayerChangeHandler(PlayerController playerController)
	{
		if(Teammates.Count > 0)
		{
			if(playerController == Teammates[0])
			{
				//
				// get away from partner
				//
			}
		}
	}


	/// <summary>
	/// realigns the player to the starting point
	/// </summary>
	/// <param name="FinishRestartHandler">Finish restart handler.</param>
	void OnRestartPositionHandler(Action FinishRestartHandler)
	{
		//
		// re enable the movement
		//

		enableMovement = true;

		//
		// restart alignment position
		//

		StartCoroutine(GoBackToAligmentPosition(FinishRestartHandler));
	}

	IEnumerator GoBackToAligmentPosition(Action FinishRestartHandler)
	{
		Vector3 destination = new Vector3(AlignmentPoint.position.x, AlignmentPoint.position.y, transform.position.z);

		yield return StartCoroutine(Move(transform.position, destination, 1.5f));

		FinishRestartHandler();
	}

	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void Update()
	{
	}

	void StickBallToPivotPoint()
	{
		if(Ball!=null)
		{
			Ball.transform.localPosition = Vector3.zero;

			//Ball.transform.localScale
		}
	}

	void FixedUpdate ()
	{
		if(!enableMovement)
		{
			return;
		}

		//
		// extract wathever input controller it has
		//

		if(input!= null)
		{
			// Cache the inputs.
			float h = input.GetHorizontalAxis();

			float v = input.GetVerticalAxis();

			bool kickButtonPressed = input.Kick();

			bool passButtonPressed = input.Pass();

			MovePlayer(h, v);

			//
			// Do the kick actions
			//

			ExecuteAction(kickButtonPressed, h, v);

			//
			// Do the pass actions
			//
			
			ExecutePass(passButtonPressed, h, v);
	
			//
			// rotate indicator
			//

			float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;

			arrowIndicator.rotation = Quaternion.AngleAxis(angle,Vector3.forward);

			//
			// check if we are touching the ball
			//
			
			CheckBallCollider();

			//
			// check if we are colliding with other character
			//
			
			CheckOtherPlayerCollider();
			
			//
			// keep the ball stick to the pivot point
			//
			
			StickBallToPivotPoint();
		}
	}

	void CheckBallCollider()
	{
		if(!enableBallCollision)
		{
			return;
		}

		Debug.DrawLine(transform.position,transform.position + new Vector3(ColliderRadius,0f,0f));

		Collider2D collider = Physics2D.OverlapCircle(transform.position,ColliderRadius,1 << BallCollisionLayer);

		if(collider != null)
		{
			BallController ball = collider.gameObject.GetComponent<BallController>();

			// 
			// if we get ball the ball and nobody has it
			//

			if(ball != null && ball.transform.parent == null)
			{
				TakeBallWithYou(ball);

				enableBallCollision = false;
			}
		}
	}

	public virtual void CheckOtherPlayerCollider()
	{
		Debug.DrawLine(transform.position,transform.position + new Vector3(ColliderRadius,0f,0f));
		
		//
		// check against the players layers
		//
		
		Collider2D collider = Physics2D.OverlapCircle(transform.position,ColliderRadius,1 << LayerToAttack);
		
		if(collider != null)
		{
			//iTween.PunchPosition(gameObject,iTween.Hash("y",1,"time",0.3f));
			
			if(collider == this.gameObject.collider2D)
			{
				return;
			}
			
			PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
			
			// 
			// if we get ball and nobody has it
			//
			
			if(playerController != null)
			{
				if(playerController.sliding)
				{
					//
					//
					//
					
					StartCoroutine(DamagePlayer());
				}
			}
		}
	}

	IEnumerator DamagePlayer()
	{
		enableMovement = false;

		//
		// now push a little the ball
		// use 30% of the full force
		//
		
		Kick(-1f,-1f,moveForce*0.3f);

		//
		// first player the damage animation
		//
			
		yield return StartCoroutine(PlayDamageAnimation());

		yield return new WaitForSeconds(0.3f);

		yield return StartCoroutine(PlayStandUpAnimation());

		PlayRunningAnimation();

		enableMovement = true;
	}

	void MovePlayer(float horizontalAxis, float verticalAxis)
	{
		transform.Translate(horizontalAxis * (moveSpeed * ballPossesionMovementPenalty) * Time.deltaTime,
		                    verticalAxis * (moveSpeed * ballPossesionMovementPenalty) * Time.deltaTime,
		                    0);

		if(horizontalAxis > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(horizontalAxis < 0 && facingRight)
			// ... flip the player.
			Flip();
	}

	void ExecuteAction(bool doit, float horizontalAxis, float verticalAxis)
	{
		if(doit)
		{
			//
			// if we have the ball kick it
			// if not slide
			//
			
			if(Ball == null)
			{
				StartCoroutine(Slide(horizontalAxis, verticalAxis));
			}
			else
			{
				Kick(horizontalAxis, verticalAxis, moveForce);
			}
		}
	}

	void ExecutePass(bool doit, float horizontalAxis, float verticalAxis)
	{
		if(doit)
		{
			//
			// if we have the ball pass it
			// if not ask for the ball
			//
			
			if(Ball == null)
			{
				//StartCoroutine(Slide(horizontalAxis, verticalAxis));

				//
				// make the partner pass the ball to you
				//

				AskForBall();
			}
			else
			{
				//
				// throw the ball towards my partner
				//

				PassBall();
			}
		}
	}

	void AskForBall()
	{
		//
		// get to see if my teammate has the ball
		//

		if(GameContext.Get.MainGameBall.CurrentPlayer != null)
		{
			if(GameContext.Get.MainGameBall.CurrentPlayer is TeamPlayerController)
			{
				//
				// make him kick the ball toward me
				//

				GameContext.Get.MainGameBall.CurrentPlayer.PassBall();
			}
		}
	}

	void PassBall()
	{
		//
		// shot it to my nearest partener [TODO]
		// for the moment just shot it to the only partner
		//

		if(this.Teammates.Count > 0)
		{
			//
			// try to fill the gap between the player and the enemy
			//
			
			Vector3 direction = (this.Teammates[0].transform.position - this.transform.position);
			
			Kick(direction.x, direction.y,moveForce);
		}
	}

	/// <summary>
	/// Determines whether this instance is partner the specified other.
	/// </summary>
	/// <returns><c>true</c> if this instance is partner the specified other; otherwise, <c>false</c>.</returns>
	/// <param name="other">Other.</param>
	public bool IsPartner(PlayerController other)
	{
		return Teammates.Contains(other);
	}

	void TakeBallWithYou(BallController ball)
	{
		Ball = ball; // set local var

		Ball.CurrentPlayer = this;

		//
		// take the ball to the pivot point while we have it
		//

		ball.gameObject.transform.parent = BallFeetPivot;

		ball.gameObject.transform.localPosition = Vector3.zero;

		//
		// remove torque
		//

		ball.rigidbody2D.AddTorque(0.0f); 

		ballPossesionMovementPenalty = 0.8f;
	}

	void Kick(float horizontalAxis, float verticalAxis, float force)
	{
		//
		// free the ball
		//

		if(Ball == null)
		{
			return;
		}

		Ball.gameObject.transform.parent = null;

		//
		// shoot the ball
		//

		Rigidbody2D ballRigidBody = Ball.GetComponent<Rigidbody2D>();

		if(horizontalAxis == 0)
		{
			horizontalAxis = facingRight ? 1 : -1;
		}

		Vector2 direction = new Vector2(horizontalAxis, verticalAxis);

		direction = direction.normalized;
		
		ballRigidBody.AddForce(direction /** h*/ * force);

		ballRigidBody.AddTorque(-0.5f);
		
		Ball = null;

		//
		// restart the ballPossesionMovementPenalty
		//

		ballPossesionMovementPenalty = 1.0f;

		Invoke("ReEnableBallCollision", BetweenKicksDelay);
	}

	IEnumerator Slide(float horizontalAxis, float verticalAxis)
	{
		//
		// first start the animation
		//

		enableMovement = false;

		StartCoroutine(PlaySlideAnimation());

		//
		// do a little dash in front of you using a slerp
		//

		int side = facingRight ? 1 : -1;

		//
		// first get the direction
		//

		float h = input.GetHorizontalAxis();

		float v = input.GetVerticalAxis();
		//
		// lots of math that is fun! 
		//

		/*var newDir = Vector3.RotateTowards(transform.right, GameContext.Get.MainGameBall.transform.position, step, 0.0);

		Debug.DrawRay(transform.position, newDir, Color.red);
		// Move our position a step closer to the target.
		transform.rotation = Quaternion.LookRotation(newDir);*/

		//float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
		
		//transform.rotation = Quaternion.AngleAxis(angle,Vector3.forward);

		//
		//
		//

		Vector3 direction = new Vector3(h,v,0f).normalized;

		Vector3 endPoint = transform.position + (direction.normalized * SlideDistance);

		yield return StartCoroutine(Move(transform.position, endPoint, 0.7f));

		yield return StartCoroutine(PlayStandUpAnimation());

		PlayRunningAnimation();

		enableMovement = true;
	}

	IEnumerator PlaySlideAnimation()
	{
		sliding = true;

		if(!spriteAnimator.IsPlaying("slide"))
		{
			spriteAnimator.Play( "slide" );
			
			spriteAnimator.AnimationCompleted = null;

			yield return new WaitForSeconds(GetClipDuration(spriteAnimator.CurrentClip));
		}

		sliding = false;
	}

	IEnumerator PlayStandUpAnimation()
	{
		if(!spriteAnimator.IsPlaying("standup") )
		{
			spriteAnimator.Play( "standup" );

			spriteAnimator.AnimationCompleted = null;

			yield return new WaitForSeconds(GetClipDuration(spriteAnimator.CurrentClip));
		}
	}

	void PlayRunningAnimation()
	{
		if(!spriteAnimator.IsPlaying("run") )
		{
			//
			// re enable movement
			//

			spriteAnimator.Play( "run" );
			
			spriteAnimator.AnimationCompleted = null;
		}
	}

	IEnumerator PlayDamageAnimation()
	{
		if(!spriteAnimator.IsPlaying("fall"))
		{
			spriteAnimator.Play( "fall" );

			spriteAnimator.AnimationCompleted = null;//PlayDamageAnimationCompleted;

			yield return new WaitForSeconds(GetClipDuration(spriteAnimator.CurrentClip));
		}
	}

	float GetClipDuration(tk2dSpriteAnimationClip clip)
	{
		return clip.frames.Length / clip.fps;
	}

	void ReEnableBallCollision()
	{
		enableBallCollision = true;
	}

	void DropBall()
	{

	}

	void MoveTowardsBall()
	{

	}

	IEnumerator Move(Vector3 pointA, Vector3 pointB, float time)
	{
		float i = 0.0f;
		
		float rate = 1.0f / time;
		
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			
			this.transform.position = Vector3.Lerp(pointA, pointB, i);

			yield return null;
		}
	}

	IEnumerator RotateMovement(Vector3 pointA, Vector3 pointB, float time)
	{
		float i = 0.0f;
		
		float rate = 1.0f / time;
		
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			
			this.transform.position = Vector3.Lerp(pointA, pointB, i);
			
			yield return null;
		}
	}

	IEnumerator MoveSlerp(Vector3 pointA, Vector3 pointB, float time)
	{
		float i = 0.0f;
		
		float rate = 1.0f / time;
		
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			
			this.transform.position = Vector3.Slerp(pointA, pointB, i);
			
			yield return null;
		}
	}
}
