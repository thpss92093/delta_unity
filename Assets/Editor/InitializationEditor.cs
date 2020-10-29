using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Initialization))]
public class InitializationEditor : Editor {

	Initialization tar;


	void OnEnable () {
		tar = (Initialization)target;

	}

	public override void OnInspectorGUI()
	{


		if (GUILayout.Button("Run Set Up"))
		{
			tar.Initialize();
		}
	

	
		base.DrawDefaultInspector ();
	}

}
