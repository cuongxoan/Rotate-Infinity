using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagComponent : Singleton<TagComponent> {
	public static string TOMAHOK  = "tomahok";
	public static string TEAM1 = "team1";
	public static string TEAM2 = "team2";
	public static string OBTACLE = "obtacle";
	public static string BALL = "ball";
	public static string TARGET = "target";
	public LayerMask CIRCLELAYER; 
}
