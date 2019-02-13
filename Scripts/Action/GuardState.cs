using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class GuardState : ActionState {

		private string isPlaying = "none";

		public GuardState ()
		{
			
		}
		
		public override Vector3 execution (InputManager inputManager)
		{
			playerInfo.trackCamera.FixationCamera ();
			Vector3 direction = Mathf.Abs (inputManager.Vertical) > Mathf.Abs (inputManager.Horizontal) ? 
								(inputManager.Vertical > 0 ? Vector3.forward : Vector3.back) : 
								(inputManager.Horizontal > 0 ? Vector3.right : Vector3.left);

			bool isTransition = playerInfo.animator.IsInTransition (0);

			if (isTransition)
			{
			}
			else
			{
				if (inputManager.GuardButton == 0)
				{
					nextState = new MoveState ();
					nextState.InitNextState (playerInfo);
					playerInfo.animator.CrossFade ("Idle", 0.2f);
					return new Vector3 (0.0f, playerInfo.verticalSpeed, 0.0f);
				}

				if (Mathf.Abs (inputManager.Vertical) + Mathf.Abs (inputManager.Horizontal) < 0.1f)
				{
					if (isPlaying != "Guard_Pose")
					{
						isPlaying = "Guard_Pose";
						playerInfo.animator.CrossFade (isPlaying, 0.2f);
					}
					return new Vector3 (0.0f, playerInfo.verticalSpeed, 0.0f);
				}

				float v = 1.0f * direction.z;
				float h = 1.0f * direction.x;
				
				string motion = Mathf.Abs (v) > Mathf.Abs (h) ? (v > 0 ? "Front" : "Back") : (h > 0 ? "Right" : "Left");
				if (isPlaying != motion)
				{
					isPlaying = motion;
					playerInfo.animator.CrossFade (motion, 0.2f);
				}
			}

			direction = playerInfo.transform.TransformDirection (direction);
			direction.y = 0f;
			direction = direction.normalized;

			return direction + new Vector3 (0.0f, playerInfo.verticalSpeed, 0.0f);
		}
		
		public override void InitNextState (PlayerInformation info)
		{
			base.InitNextState (info);
			playerInfo.animator.CrossFade ("Guard_Pose", 0.01f);
		}
		
		public override void ActionEvent (int index)
		{
		}

		public override void DamageEvent (int damage, string motion)
		{
			playerInfo.animator.Play ("Guard_Damage");
		}
	}
}
