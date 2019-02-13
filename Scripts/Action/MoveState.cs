using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class MoveState : ActionState {

		private int moveCounter = 0;

		public MoveState ()
		{
			
		}
		
		public override Vector3 execution (InputManager inputManager)
		{
			moveDirection = GetMoveDirection (inputManager.Vertical, inputManager.Horizontal).normalized;
			StartJumpMotion (inputManager);
			
			float axis = Mathf.Abs (inputManager.Vertical)+Mathf.Abs (inputManager.Horizontal);
			float nowSpeed = playerInfo.speed*axis;
			nowSpeed = Mathf.Clamp (nowSpeed, WALK_SPEED, RUN_SPEED);
			
			Vector3 movement = moveDirection*nowSpeed + new Vector3 (0.0f, playerInfo.verticalSpeed, 0.0f);
			playerInfo.animator.SetFloat ("speed", Mathf.Abs (inputManager.Horizontal) + Mathf.Abs (inputManager.Vertical));

			bool isTransition = playerInfo.animator.IsInTransition (0);

			if (isTransition)
			{
			}
			else
			{
				AnimatorStateInfo stateInfo = playerInfo.animator.GetCurrentAnimatorStateInfo (0);

				if (moveDirection == Vector3.zero)
				{
					//if (playerInfo.pushCollider.enabled) playerInfo.pushCollider.enabled = false;
					moveCounter = 0;
					playerInfo.gameManager.signalCounter ++;
				}
				else
				{
					moveCounter ++;
					playerInfo.gameManager.signalCounter = 0;
					//if (!playerInfo.pushCollider.enabled) playerInfo.pushCollider.enabled = true;
					if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Free"))
						playerInfo.animator.CrossFade ("Move", 0);
					playerInfo.soundManager.PlaySEOnce (0);
					playerInfo.transform.rotation = Quaternion.LookRotation (moveDirection);
				}

				TransitionAttackState (inputManager);
				TransitionGuardState (inputManager.GuardButton == 1);
			}

			playerInfo.trackCamera.MoveCamera (inputManager, moveCounter > 60);

			return movement;
		}

		private void StartJumpMotion (InputManager inputManager)
		{
			if (inputManager.Jump > 0 && isGround)
			{
				playerInfo.verticalSpeed = JUMP_POWER;
				playerInfo.gravity = GRAITY;
				playerInfo.animator.CrossFade ("Jump_start", 0.1f);
				inputManager.Jump = 2;
				playerInfo.jumpNumber ++;
			}
			
			if (playerInfo.jumpNumber == 1 && inputManager.Jump == 1)
			{
				playerInfo.horizontalSpeed = DASH_SPEED;
				playerInfo.jumpNumber ++;
				nextState = new DashState ();
				nextState.InitNextState (playerInfo);
			}
		}

		private void TransitionAttackState (InputManager inputManager)
		{
			if (inputManager.AttackButton == 1)
			{
				AttackInformation info = playerInfo.characterInfo.attackInfo[0];

				if (inputManager.SkillButton >= 1)
					info = playerInfo.characterInfo.attackInfo[2];

				playerInfo.SetSwordActive (0);
				nextState = new AttackState (info);
				nextState.InitNextState (playerInfo);
			}
			else if (inputManager.ChargeButton == 1)
			{
				AttackInformation info = playerInfo.characterInfo.attackInfo[1];

				if (inputManager.SkillButton >= 1)
					info = playerInfo.characterInfo.attackInfo[3];

				playerInfo.SetSwordActive (0);
				nextState = new AttackState (info);
				nextState.InitNextState (playerInfo);
			}
		}

		private void TransitionGuardState (bool flag)
		{
			if (flag)
			{
				playerInfo.SetSwordActive (1);
				nextState = new GuardState ();
				nextState.InitNextState (playerInfo);
			}
		}

		public override void InitNextState (PlayerInformation info)
		{
			base.InitNextState (info);
			if (isGround) playerInfo.animator.CrossFade ("Idle", 0.2f);

			playerInfo.speed = RUN_SPEED;
			playerInfo.gameManager.signalCounter = 0;
			playerInfo.SetSwordActive (1);
		}

		public override void DamageEvent (int damage, string motion)
		{
			playerInfo.hp -= damage;
			if (playerInfo.hp <= 0) playerInfo.hp = 0;
			nextState = new DamegeState ();
			nextState.InitNextState (playerInfo);
			playerInfo.animator.Play (motion);
		}
	}
}
