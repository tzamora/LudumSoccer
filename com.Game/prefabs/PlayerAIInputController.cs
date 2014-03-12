using UnityEngine;
using System.Collections;

public enum AIStates
{
    GoingToGoal,
    GoingToBall,
    AsistPartner
}

public class PlayerAIInputController : InputController
{
    public TeamType GoalToScore;
    private PlayerController playerController;
    public AIStates CurrentState = AIStates.GoingToBall;
    bool dispatchKick = false;
    Vector3 direction = Vector3.zero;
    public float DistanceToAttack = 0;
    public float DistanceToScoreGoal = 0;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        StartCoroutine(ThinkAI());
    }

    IEnumerator ThinkAI()
    {
        //
        // here the decisions are made
        //

        while (true)
        {
            if (playerController.Ball == null)
            {

                //
                // if somebody has the ball
                //

                PlayerController playerWithBall = GameContext.Get.MainGameBall.CurrentPlayer;

                if (playerWithBall!=null)
                {
                    if(playerController.IsPartner(playerWithBall))
                    {
                        AsistPartner();

                        //ok estamos listos con el pase ahora hay Que ajustar La distancia Que hay entre mi Partner 

                        //tenemos Que ver porque los enemigos se maman
                    }
                }
                else
                {
                    GoToBall();

                    TryToStealBall();
                }   
                                
            }
            else
            {
                //
                // if we have the ball the try to score
                //

                GoToScoreGoal();

                NearScoreGoal();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    public virtual void TryToStealBall()
    {
        //
        // when we are near a ball and the ball has a parent
        //

        if (GameContext.Get.MainGameBall.CurrentPlayer != null)
        {
            Vector3 ballPosition = GameContext.Get.MainGameBall.transform.position;

            float distance = Vector3.Distance(ballPosition, transform.position);

            Vector3 attackPoint = transform.position + new Vector3(0f, DistanceToAttack, 0f);

            Debug.DrawLine(transform.position, attackPoint, Color.red);

            if (distance < DistanceToAttack)
            {
                dispatchKick = true;
            }
        }
    }

    void GoToBall()
    {
        Vector3 ballPos = GameContext.Get.MainGameBall.transform.position;
        
        // Gets a vector that points from the player's position to the target's.
        Vector3 heading = ballPos - transform.position;
        
        float distance = heading.magnitude;
        
        direction = heading / distance;  // This is now the normalized direction.
    }

    void GoToScoreGoal()
    {
        Vector3 goalPos = GameContext.Get.LeftScore.position;

        if (GoalToScore == TeamType.Local)
        {
            goalPos = GameContext.Get.LeftScore.position;
        } else
        {
            goalPos = GameContext.Get.RightScore.position;
        }

        GoToDirection(goalPos);
    }

    void GoToDirection(Vector3 destination)
    {
        // Gets a vector that points from the player's position to the target's.
        Vector3 heading = destination - transform.position;
        
        float distance = heading.magnitude;
        
        direction = heading / distance;  // This is now the normalized direction.
    }

    void NearScoreGoal()
    {
        //
        // if we are near the goal score by a distance of minScoreGoal distance
        // then kick the ball
        //

        Vector3 goalPos = GameContext.Get.LeftScore.position;
        
        //  Debug.Log(GoalToScore);
        
        if (GoalToScore == TeamType.Local)
        {
            goalPos = GameContext.Get.LeftScore.position;
        } else
        {
            goalPos = GameContext.Get.RightScore.position;
        }

        float distance = Vector3.Distance(goalPos, transform.position);

        if (distance < DistanceToScoreGoal)
        {
            //
            // try to score a motherfucking goal!!
            //

            dispatchKick = true;
        }
    }

    void AsistPartner()
    {
        //
        // when the player have the ball, the AI will help running next to us
        // first choose if the AI partner will be running up or below me
        // make this selection based on the score position.
        //

        if (playerController.Teammates.Count > 0)
        {
            //
            // first get the point where we will be next to the player
            //

            Vector3 positionNextToPartner = playerController.Teammates[0].transform.position + new Vector3(0f,2f,0f);

            //Vector3 positionNextToPartner = playerController.Teammates[0].transform.position;

            //if(positionNextToPartner != transform.position)
            //{
            Vector3 teammateDirection = (positionNextToPartner-transform.position).normalized;
                
                //Vector3 scoreDirection = transform.InverseTransformPoint(GameContext.Get.RightScore.transform.position);
                
            GoToDirection(positionNextToPartner);
            //}

//            if (scoreDirection.y > 0)
//            {
//                //
//                // place the ai up to the player
//                // run towards a specified position behind the player
//                //
//
//                Debug.Log("El pase se esta realizando para la nueva noche");
//            }
//            
//            if (scoreDirection.y < 0)
//            {
//                
//            }
            
            //
            //
            //
        }
    }

    override public float GetHorizontalAxis()
    {
        return direction.x;
    }
    
    override public float GetVerticalAxis()
    {
        return direction.y;
    }
    
    override public bool Kick()
    {
        if (dispatchKick)
        {
            dispatchKick = false;

            return true;
        } else
        {
            return false;
        }
    }
}
