using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class ActionState {

		public ActionState nextState {get; protected set;}
		public bool isGround {get; set;}

		protected float JUMP_POWER = 15.0f;
		protected float GRAITY = -20.0f;
		protected float RUN_SPEED = 6.0f;
		protected float WALK_SPEED = 1.5f;
		protected float DASH_SPEED = 60.0f;
		protected float DASH_ACCELE = 3.0f;
		protected float ADVANCE_SPEED = 0.5f;

		protected Vector3 moveDirection = Vector3.zero;
		protected PlayerInformation playerInfo;

		public ActionState ()
		{
		}

		protected Vector3 GetMoveDirection (float v, float h)
		{
			Transform cameraTransform = Camera.main.transform;
			Vector3 forward = cameraTransform.TransformDirection (Vector3.forward);
			forward.y = 0.0f;
			forward = forward.normalized;
			
			Vector3 right = new Vector3 (forward.z, 0.0f, -forward.x);
			Vector3 moveDir = v*forward + h*right;

			return moveDir;
		}

		public void GraityController ()
		{
			if (isGround)
			{
				if (playerInfo.verticalSpeed < 0.0f)
				{
					playerInfo.verticalSpeed = 0.0f;
					playerInfo.gravity = 10.0f;
					playerInfo.jumpNumber = 0;
				}
			}
			else
			{
				playerInfo.verticalSpeed -= playerInfo.gravity*Time.deltaTime;
				playerInfo.gravity += playerInfo.accele;

				playerInfo.gravity = Mathf.Clamp (playerInfo.gravity, -100f, 200f);
				playerInfo.animator.SetFloat ("verticalSpeed", playerInfo.verticalSpeed);
			}
			playerInfo.animator.SetBool ("isGrounded", isGround);
		}

		public virtual Vector3 execution (InputManager inputManager)
		{
			return Vector3.zero;
		}

		public virtual void InitNextState (PlayerInformation info)
		{
			nextState = this;
			playerInfo = info;
		}

		public virtual void ActionEvent (int index)
		{
		}

		public virtual void DamageEvent (int damage, string motion)
		{

		}
	}
}
