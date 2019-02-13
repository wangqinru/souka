using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class DamegeState : ActionState {

		private float damageTime = 0f;
		private float DAMAGE_LENGTH = 0.6f;
		
		public DamegeState ()
		{
			
		}
		
		public override Vector3 execution (InputManager inputManager)
		{
			if (Time.time - damageTime > DAMAGE_LENGTH)
			{
				if (playerInfo.hp > 0)
				{
					nextState = new MoveState ();
					nextState.InitNextState (playerInfo);
					playerInfo.animator.CrossFade ("Idle", 0.1f);
				}
				else
				{
					playerInfo.animator.CrossFade ("Dead", 0.1f);
					GameObject.Find ("GameManager").GetComponent<GameManager> ().Result (1);
					damageTime = Time.time;
				}
			}
			playerInfo.trackCamera.MoveCamera (inputManager, false);
			
			return Vector3.zero;
		}
		
		public override void InitNextState (PlayerInformation info)
		{
			base.InitNextState (info);
			playerInfo.speed = 0f;
			damageTime = Time.time;
		}

		public override void DamageEvent (int damage, string motion)
		{
			if (playerInfo.hp <= 0) return;
			playerInfo.hp -= damage;
			if (playerInfo.hp <= 0) playerInfo.hp = 0;
			playerInfo.animator.Play (motion);
			damageTime = Time.time;
		}
	}
}
