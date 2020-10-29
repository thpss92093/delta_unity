using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegRender : MonoBehaviour {
	public Color[] colors;
	public GameObject[] objects;
	public Material seg;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < objects.Length; i++) {
			Material m = new Material (seg);
			m.color = colors [i];

			if (objects [i].transform.childCount > 0) {
				foreach (Transform child in objects[i].transform) {
					child.gameObject.GetComponent<Renderer> ().material = m;
				}
			} else {
				objects[i].GetComponent<Renderer> ().material = m;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
