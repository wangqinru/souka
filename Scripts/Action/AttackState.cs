using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class AttackState : ActionState {

		private AttackInformation attackInfo;
		private int nowStateIndex;
		private float horizontalSpeed = 0f;
		private float acceleration = 0f;
		private float verticalSpeed = 0f;
		private float gravity = 0f;

		private string[] effectPath = new string[6]
		{
			"Prefabs/AttackEffect",
			"Prefabs/AttackEffect2",
			"Prefabs/AttackEffect3",
			"Prefabs/AttackEffect4",
			"Prefabs/AttackEffect5",
			"Prefabs/AttackEffect6",
		};

		public AttackState (AttackInformation ainfo) : base ()
		{
			attackInfo = ainfo;
		}
		
		public override Vector3 execution (InputManager inputManager)
		{
			playerInfo.trackCamera.MoveCamera (inputManager, false);
			moveDirection = GetMoveDirection (inputManager.Vertical, inputManager.Horizontal).normalized;
			Vector3 forward = playerInfo.transform.TransformDirection (Vector3.forward);
			forward.y = 0.0f;
			forward = forward.normalized;

			bool isTransition = playerInfo.animator.IsInTransition (0);

			if (isTransition)
			{
				AnimatorStateInfo nextStateInfo = playerInfo.animator.GetNextAnimatorStateInfo (0);

				if (nextStateInfo.IsName (attackInfo.stateName[nowStateIndex]) && nowStateIndex < attackInfo.stateName.Length-1)
				{
					//if (playerInfo.animator.GetBool ("Key"))
					{
						playerInfo.animator.SetBool ("Key", false);
						//playerInfo.swordCollision.SetAttackForce (attackInfo.ForceZ[nowStateIndex], attackInfo.ForceY[nowStateIndex]);
						horizontalSpeed = attackInfo.moveSpeed[nowStateIndex];
						acceleration = attackInfo.acceleration[nowStateIndex]/100f;
						verticalSpeed = attackInfo.jumpPower[nowStateIndex];
						gravity = attackInfo.gravity[nowStateIndex];
						if (moveDirection != Vector3.zero) playerInfo.transform.rotation = Quaternion.LookRotation (moveDirection);
						nowStateIndex ++;
					}
				}

				if (playerInfo.verticalSpeed != 0f)
				{
					playerInfo.verticalSpeed = 0f;
					playerInfo.horizontalSpeed = 0.0f;
					playerInfo.gravity = GRAITY;
				}

				return new Vector3 (0.0f, verticalSpeed, 0.0f);
				//return Vector3.zero;
			}
			else
			{
				if (horizontalSpeed > 0f) 
				{
					horizontalSpeed -= acceleration;
					if (horizontalSpeed <= 0f) horizontalSpeed = 0f;
				}

				verticalSpeed -= gravity*Time.time;
				if (inputManager.AttackButton > 0)
				{
					playerInfo.animator.SetBool ("Key", true);
				}
			}
			
			return forward*horizontalSpeed + new Vector3 (0.0f, verticalSpeed, 0.0f);
		}
		
		public override void InitNextState (PlayerInformation info)
		{
			base.InitNextState (info);
			playerInfo.animator.SetBool ("Key", false);
			playerInfo.speed = 0f;
			nowStateIndex = 0;
			//playerInfo.swordCollision.SetAttackForce (attackInfo.ForceZ[nowStateIndex], attackInfo.ForceY[nowStateIndex]);
			playerInfo.animator.CrossFade (attackInfo.stateName[nowStateIndex], 0.1f);
		}
		
		public override void ActionEvent (int index)
		{
			playerInfo.soundManager.PlaySE (attackInfo.se[nowStateIndex-1]);
			Vector3 epos = playerInfo.transform.TransformPoint (Vector3.forward*1.5f);
			Vector3 eEul = playerInfo.transform.eulerAngles;
			if (index < effectPath.Length)
			{
				GameObject effect = MonoBehaviour.Instantiate (Resources.Load (effectPath[index], typeof (GameObject)), 
									                           new Vector3 (epos.x, playerInfo.sword[1].transform.position.y, epos.z), 
									                           Quaternion.Euler (new Vector3 (eEul.x, eEul.y, attackInfo.eulerZ[nowStateIndex-1]))) as GameObject;

				effect.GetComponent<Collider>().enabled = false;
				EffectCollision collision = effect.AddComponent<EffectCollision> ();
				collision.Prepare (playerInfo.characterInfo.status.attack ,attackInfo.ForceZ[nowStateIndex-1], attackInfo.ForceY[nowStateIndex-1], 10);
				collision.GetComponent<Collider>().enabled = true;
			}
		}

		public override void DamageEvent (int damage, string motion)
		{
			if (damage > (float)playerInfo.characterInfo.status.hp/10f)
			{
				playerInfo.animator.Play (motion);
				nextState = new DamegeState ();
				nextState.InitNextState (playerInfo);	
			}

			playerInfo.hp -= damage;
			if (playerInfo.hp <= 0) playerInfo.hp = 0;
		}
	}
}
