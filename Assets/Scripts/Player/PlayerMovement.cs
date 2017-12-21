﻿// Created by Timo Heijne
// camera relative move by Antonio Bottelier

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PlayerAnimation _playerAnimation;

	public bool CanMove { get; private set; }

	// Use this for initialization
	void Start () {
	    if (_rb == null)
	        Debug.LogError("PlayerMovement :: Rigidbody not found on player");

        if (_playerAnimation == null)
            Debug.LogError("PlayerMovement :: _playerAnimation not found");

		CanMove = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!CanMove)
		{
			_rb.velocity = Vector3.zero;
			return;
		}
		
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		
		// Get left vector of character
		var lockon = TargetLock.Targets[1];
		var vector = lockon.transform.position - transform.position;
		var cross = Vector3.Cross(vector, Vector3.up);

		cross.y = vector.y = 0;
		
		Vector3 movement = vector.normalized * moveVertical + -cross.normalized * moveHorizontal;
        movement = movement.normalized * _movementSpeed;
		_rb.velocity = new Vector3(movement.x, _rb.velocity.y, movement.z);
		
        if (_rb.velocity.x == 0 && _rb.velocity.z == 0) {
            _playerAnimation.ChangePlayerStatus(PlayerAnimation.Status.Idle);
        } else {
            _playerAnimation.ChangePlayerStatus(PlayerAnimation.Status.Walking);
            transform.rotation = Quaternion.Slerp(transform.rotation, 
	            Quaternion.LookRotation(_rb.velocity, Vector3.up), Time.deltaTime * 10);
        }          
    }

	public void ToggleCanMove(bool canmove)
	{
		CanMove = canmove;
	}
}
