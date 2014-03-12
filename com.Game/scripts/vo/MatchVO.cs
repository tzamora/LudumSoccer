using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[Serializable]
public class MatchVO {

	public int LocalScore = 0;

	public int VisitScore = 0;

	public List<PlayerController> LocalTeam;
	
	public List<PlayerController> VisitTeam;

	public void AddScore (TeamType teamType)
	{
		switch(teamType)
		{
		case TeamType.Local:
			LocalScore++;
			break;
		case TeamType.Visit:
			VisitScore++;
			break;
		}
	}
}
