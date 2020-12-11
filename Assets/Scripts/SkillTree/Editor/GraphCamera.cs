using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphCamera
{
	private bool _debug = false;

	public Vector2 _offset; // Total offset tracked by the camera
	public bool _move; // Is the camera moving?
	private Vector2 _prevPos; // Used to track position changes

	public float VIEWPORT_SIZE = 6000f;

	public void BeginMove(Vector2 startPos)
	{
		_move = true;
		_prevPos = startPos;
		if (_debug) Debug.Log("Begin camera move");
	}

	public void EndMove()
	{
		_move = false;
		if (_debug) Debug.Log("End camera move");
	}

	public bool PollCamera(Vector2 newPos)
	{
		if (!_move) return false;

		_offset += _prevPos - newPos;
		_prevPos = newPos;

		//	if (debug) Debug.LogFormat("Polled offset: {0}, {1}", offset.x.ToString(), offset.y.ToString());

		return true;
	}

	// Get the mouse position considering the current camera offset
	public Vector2 GetMouseGlobal(Vector2 mouse)
	{
		return new Vector2(mouse.x + _offset.x - (VIEWPORT_SIZE / 2f), mouse.y + _offset.y - (VIEWPORT_SIZE / 2f));
	}

	public Vector2 GetOffsetGlobal()
	{
		return new Vector2(_offset.x - (VIEWPORT_SIZE / 2f), _offset.y - (VIEWPORT_SIZE / 2f));
	}

	public void Reset()
	{
		_move = false;
		_offset = new Vector2(VIEWPORT_SIZE / 2f, VIEWPORT_SIZE / 2f);
		if (_debug) Debug.Log("Camera reset");
	}
}
