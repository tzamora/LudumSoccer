using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player3DController : MonoBehaviour
{

    public CharacterController characterController;
    public float speed = 0f;
    private Vector3 move = Vector3.zero;
    public float pushPower = 2.0F;
	public Transform ball;
	public Transform ballPlaceholder;

	/// <summary>
	/// Amount of time the player stops when doing a kick
	/// </summary>
	public float kickStopTime = 0f;

	public List<Vector2> KickForces;

	public Vector2 CurrentKickForce;

	//public float kickPowerX;
	//public float kickPowerY;
	float horizontalAxis = 0f;
	float verticalAxis = 0f;
	public TriggerListener BallTrigger;
	private bool StopMove = false;
	public GameObject KickParticle; 

    // Use this for initialization
    void Awake()
    {
		BallTrigger.OnEnter += BallTriggerEnterHandler;

		CurrentKickForce = KickForces[0];
    }

	// Update is called once per frame
    void Update()
    {
		MovePlayer();

		//
		// while we have the ball keep it at the ball placeholder position
		//

		if(ball!=null)
		{
			ball.transform.localPosition = Vector3.zero;
		}
    }

	void MovePlayer()
	{
		if(StopMove)
		{
			return;
		}

		horizontalAxis = Input.GetAxis("Horizontal");
		
		verticalAxis = Input.GetAxis("Vertical");
		
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//StartCoroutine(Kick());

			StartCoroutine("ChargeKick");
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			//StartCoroutine(Kick());
			
			StopCoroutine("ChargeKick");

			StartCoroutine(Kick());
		}
		
		move = new Vector3(horizontalAxis, Physics.gravity.y, verticalAxis).normalized;
		
		Vector3 direction = new Vector3(move.x, 0f, move.z).normalized;
		
		if(direction != Vector3.zero)
		{
			transform.rotation = Quaternion.LookRotation(direction);
		}
		
		characterController.Move(move * speed * Time.deltaTime);
	}

	IEnumerator ChargeKick()
	{
		kickStopTime = 0.05f;

		yield return new WaitForSeconds(0.5f);

		//
		// set the force in level 1
		//

		CurrentKickForce = KickForces[1];

		kickStopTime = 0.2f;

		//this.renderer.material.color = Color.blue;

		yield return new WaitForSeconds(1.5f);
		
		//
		// set the force in level 1
		//

		kickStopTime = 0.4f;

		CurrentKickForce = KickForces[2];

		//this.renderer.material.color = Color.blue;
	}

	IEnumerator Kick()
	{
		if(ball!=null)
		{
			//
			// first disable the trigger
			//

			ToggleTrigger(false);

			ball.transform.parent = null;

			Vector3 direction = transform.rotation * Vector3.forward;

			print ("this is the direction " + direction);

			direction += new Vector3(0f, 1f, 0f);

			direction = new Vector3(direction.x * CurrentKickForce.x, direction.y * CurrentKickForce.y, direction.z * CurrentKickForce.x );	

			//Debug.Log ("Direction " + direction);

			ball.rigidbody.velocity = direction;

			//
			// remove the reference with the ball
			//

			ball = null;

			Invoke("EnableTheTrigger",0.2f);

			//
			// show the particle
			//

			Instantiate(KickParticle, ballPlaceholder.transform.position, Quaternion.identity);

			//
			// make the player stop for a few frames
			//

			StopMove = true;

			yield return new WaitForSeconds(kickStopTime);

			StopMove = false;
		}

		//
		// reset the kick forces
		//

		CurrentKickForce = KickForces[0];

		//this.renderer.material.color = Color.white;
	}

	void EnableTheTrigger()
	{
		ToggleTrigger(true);
	}

	void ToggleTrigger(bool toggle)
	{
		BallTrigger.gameObject.SetActive(toggle);
	}

	//void OnTriggerEnter(Collider other)
	void BallTriggerEnterHandler(Collider other)
	{
		SoccerBallController soccerBall = (SoccerBallController) other.gameObject.GetComponent<SoccerBallController>();

		if(soccerBall!=null)
		{
			other.transform.parent = ballPlaceholder;

			ball = other.transform;

			ball.transform.localPosition = Vector3.zero;
		}
	}

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
            return;
        
        if (hit.moveDirection.y < -0.3F)
            return;
        
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower;
    }
}
