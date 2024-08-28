using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
	CharacterController cc;
	public Transform cam;
	Vector3 dir;
	Vector3 vel;
	
	[SerializeField] private float walks = 1.5f;
	[SerializeField] private float runs = 3f;
	[SerializeField] private float jumpf = 1.7f;
	[SerializeField] private float turntime = 0.2f;
	float trues;
	float h, ve;
	float turnvel; 
	float gravity = -9.81f;
	
	bool iswalk;
	bool isrun;
	bool isjump;
	bool isground;
	
	void Awake()
	{
		cc = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;
		isjump = false;
	}
	
	void Move()
	{
		isground = cc.isGrounded;
		
		h = Input.GetAxis("Horizontal");
		ve = Input.GetAxis("Vertical");
		dir = new Vector3(h, 0, ve).normalized;
		
		iswalk = h != 0 || ve != 0;
		
		if (iswalk)
		{
			trues = walks;
		}
		if (iswalk && !isrun && Input.GetKey(KeyCode.LeftShift))
		{
			trues = runs;
			isrun = true;
		}
		else
		{
			isrun = false;
		}
		
		if (iswalk)
		{
			float targetangle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnvel, turntime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);
			Vector3 movedir = Quaternion.Euler(0f, targetangle , 0f) * Vector3.forward;
			cc.Move(movedir.normalized * trues * Time.deltaTime);
		}
		
	}
	
	void Jump()
	{
		if (isground)
		vel.y -= 0.5f;
		if (isground && Input.GetKeyDown(KeyCode.Space) && !isjump)
		{
			isjump = true;
			vel.y = Mathf.Sqrt(jumpf * -2f * gravity);
		}
		else
		{
			isjump = false;
		}
		vel.y += gravity * Time.deltaTime;
		cc.Move(vel * Time.deltaTime);
	}
	
	void Update()
	{	
		Move();
		Jump();
	}
}
