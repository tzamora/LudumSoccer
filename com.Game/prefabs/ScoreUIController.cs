using UnityEngine;
using System.Collections;

public class ScoreUIController : MonoBehaviour {

	public tk2dTextMesh LocalScore;

	public tk2dTextMesh VisitScore;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		LocalScore.text = "Local: " + GameContext.Get.Match.LocalScore;

		VisitScore.text = "Visit: " + GameContext.Get.Match.VisitScore;
	}
}
