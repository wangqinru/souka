using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class DashState : ActionState {

		public DashState ()
		{

		}

		public override Vector3 execution (InputManager inputManager)
		{
			playerInfo.trackCamera.MoveCamera (inputManager, false);

			if (playerInfo.horizontalSpeed > 0.1f)
			{
				Vector3 forward = playerInfo.transform.TransformDirection (Vector3.forward);
				forward.y = 0.0f;
				forward = forward.normalized;
				
				playerInfo.horizontalSpeed -= DASH_ACCELE;
				playerInfo.gravity = 0.0f;
				
				if (playerInfo.horizontalSpeed <= 1.0f)
				{
					//_animator.CrossFade ("Jump_descent", 0);
					playerInfo.verticalSpeed = -1.0f;
					playerInfo.horizontalSpeed = 0.0f;
					playerInfo.gravity = GRAITY;
				}
				
				return forward*playerInfo.horizontalSpeed;
			}
			else
			{
				nextState = new MoveState ();
				nextState.InitNextState (playerInfo);
			}

			return Vector3.zero;
		}

		public override void InitNextState (PlayerInformation info)
		{
			base.InitNextState (info);
			playerInfo.animator.CrossFade ("Jump_dush", 0.1f);
		}

		public override void DamageEvent (int damage, string motion)
		{
		}
	}
}