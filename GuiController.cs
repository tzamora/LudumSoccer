using UnityEngine;
using System.Collections;

public class GuiController : MonoBehaviour {

	public tk2dTextMesh PowerText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		PowerText.text = "[ " + GameContext3D.Get.MainPlayer.CurrentKickForce.x + " , " + GameContext3D.Get.MainPlayer.CurrentKickForce.y + " ]";
	}
}
