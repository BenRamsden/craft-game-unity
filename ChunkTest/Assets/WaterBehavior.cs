using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBehavior : MonoBehaviour {
	public int _uvTieX = 1;
	public int _uvTieY = 1;
	public int _fps = 10;

	private Vector2 _size;
	private Renderer _myRenderer;
	private int _lastIndex = -1;

	// Use this for initialization
	void Start () {
		_size = new Vector2 (1.0f / _uvTieX , 1.0f / _uvTieY);
		_myRenderer = GetComponent<Renderer>();
		if(_myRenderer == null)
			enabled = false;
	}

	// Update is called once per frame
	void Update() {
	}
}
