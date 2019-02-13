//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using UnityEngine;

namespace GraduationProject
{
	public class AnomalyState : ArtificialIntelligenceStage
	{
		private bool standup = false;
		private float WAIT_TIME = 0.5f;

		public AnomalyState (EnemyInformation Info) : base (Info)
		{
			nextState = this;
			timeCounter = Time.time;
			enemyInfo.animator.SetFloat ("speed", 0f);
			if (enemyInfo.pushForce_y > 0f) standup = true;
		}
		
		public override Vector3 Run ()
		{
			Vector3 moveDirection = Vector3.zero;
			
			if (enemyInfo.pushForce_z > 0.1f)
			{
				enemyInfo.pushForce_z -= 10f*Time.deltaTime;
				if (enemyInfo.pushForce_z <= 0.1f) enemyInfo.pushForce_z = 0f;
			}
			
			UseGravity ();
			
			moveDirection = enemyInfo.pushDirection*enemyInfo.pushForce_z + new Vector3 (0f, velocity, 0f);

			if (enemyInfo.pushForce_y + enemyInfo.pushForce_z < 0.1f)
			{
				if (Time.time - timeCounter > WAIT_TIME)
				{
					if (enemyInfo.hp <= 0)
					{
						enemyInfo.animator.Play ("Dead");
						nextState = new DeadState (enemyInfo);
						return Vector3.zero;
					}

					if (standup)
					{
						enemyInfo.animator.Play ("Standup");
						standup = false;
						timeCounter = Time.time;
					}
					else
						nextState = new NormalState (enemyInfo);
				}
			}
			else
				timeCounter = Time.time;

			return moveDirection;
		}
		
		public override void Push (float forceZ, float forceY, Vector3 pushDir)
		{
			base.Push (forceZ, forceY, pushDir);
		}
		
		public override void Damage (int damage, CaptainEnemy captain, EnemyController enemy)
		{
			if (enemyInfo.hp <= 0) return;
			enemyInfo.hp -= damage;
			if (enemyInfo.hp <= 0)
			{
				captain.RemoveAbeSan (enemy);
				enemyInfo.hp = 0;
			}

			if (enemyInfo.pushForce_y > 0)
				enemyInfo.animator.Play ("Down");
			else
			{
				AnimatorStateInfo nowState = enemyInfo.animator.GetCurrentAnimatorStateInfo (0);
				if (nowState.IsName ("Damage"))
					enemyInfo.animator.Play ("Damage_2");
				else
					enemyInfo.animator.Play ("Damage");
			}
		}
		
		protected override void UseGravity ()
		{
			if (isGrounded)
			{
				if (velocity < 0f)
				{
					velocity = 0f;
					gravity = GRAVITY_;
				}
			}
			else
			{
				/*pushForce_y -= GRAVITY_*Time.deltaTime;
			if (pushForce_y < 0f && pushForce_y > -1f) 
			{
				gravity = 0f;
			}

			velocity -= gravity*Time.deltaTime;
			gravity += ACCELE;*/
				
				//まず与えられたY方向の力を減る
				if (enemyInfo.pushForce_y > 0f)
				{
					enemyInfo.pushForce_y -= GRAVITY_*Time.deltaTime;
					velocity = enemyInfo.pushForce_y;
				}
				//Y方向の力がなくなったら下降し始める
				else   
				{
					velocity -= gravity*Time.deltaTime;
					gravity += ACCELE;
				}
			}
		}

		public override void StateEvent ()
		{
			//nextState = new NormalState (animator, player, transform);
		}
	}

	public class DeadState : ArtificialIntelligenceStage
	{
		private float WAIT_TIME = 2f;
		
		public DeadState (EnemyInformation Info) : base (Info)
		{
			nextState = this;
			timeCounter = Time.time;
		}
		
		public override Vector3 Run ()
		{

			if (Time.time - timeCounter > WAIT_TIME)
			{
				MonoBehaviour.Destroy (enemyInfo.transform.gameObject);
			}

			return Vector3.zero;
		}
		
		public override void Push (float forceZ, float forceY, Vector3 pushDir)
		{
		}
		
		public override void Damage (int damage, CaptainEnemy captain, EnemyController enemy)
		{
		}
		
		public override void StateEvent ()
		{
			//nextState = new NormalState (animator, player, transform);
		}
	}
}

