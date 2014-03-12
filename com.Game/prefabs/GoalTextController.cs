using UnityEngine;
using System.Collections;

public class GoalTextController : MonoBehaviour {

	public Vector3 PointA;

	public Vector3 PointB;

	public float Duration;

	void Awake()
	{

		GameContext.Get.GameEvent.ShowGoalAlert += ShowGoalAlertHandler;
	}

	void ShowGoalAlertHandler()
	{
		//Debug.Log("@!#@#%#$%#$% ahora si vamos con todo");

		StartCoroutine(AnimateText(PointA, PointB, Duration));
	}

	IEnumerator AnimateText(Vector3 pointA, Vector3 pointB, float time)
	{
		float i = 0.0f;
		
		float rate = 1.0f / time;
		
		while (i < 1.0f)
		{
			i += Time.deltaTime * rate;
			
			this.transform.localPosition = Vector3.Lerp(pointA, pointB, i);

			this.gameObject.GetComponent<tk2dTextMesh>().color = Color.Lerp( new Color(1f,1f,1f,1f), new Color(1f,1f,1f,0f), i);

			yield return null;

			//Debug.Log("@!#@#%#$%#$% ahora si vamos con todo");
		}
	}

}
