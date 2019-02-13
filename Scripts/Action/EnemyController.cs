using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class EnemyController : MonoBehaviour {

		private CaptainEnemy captain;
		private CharacterController characterController_;
		private Animator animator;
		private PlayerController player;
		private EnemyInformation enemyInfo;
		//private AbeCollision collision;
		
		private ArtificialIntelligenceStage nowState;
		
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			
			nowState = nowState.nextState;
			Vector3 movement = nowState.Run ();
			
			movement *= Time.deltaTime;
			characterController_.Move (movement);
			nowState.isGrounded = characterController_.isGrounded;

			if (transform.position.y <= -20f)
				transform.position = captain.transform.position;
		}
		
		
		public void Access (float magn)
		{
			Vector3 localPos = player.transform.InverseTransformPoint (transform.position);
			localPos = localPos.normalized*magn;
			nowState.targetPosition = player.transform.TransformPoint (localPos);
		}
		
		public void PlayDamageMotion (int damage)
		{
			nowState.Damage (damage, captain, this);
			captain.PlayHitSE ();
		}
		
		public void Push (float forceZ, float forceY, Vector3 pushDir)
		{
			nowState.Push (forceZ, forceY, pushDir);
		}
		
		public void Prepare (PlayerController p, CaptainEnemy cap, EnemyData data)
		{
			animator = GetComponent<Animator> ();
			player = p;
			captain = cap;
			characterController_ = GetComponent<CharacterController> ();
			//collision = GetComponentInChildren<AbeCollision> ();
			enemyInfo = new EnemyInformation (animator, player, transform);
			enemyInfo.hp = data.Hp;
			enemyInfo.attack = data.attack;
			enemyInfo.moveSpeed = data.speed;
			enemyInfo.range = data.range;
			nowState = new NormalState (enemyInfo);
			nowState.targetPosition = transform.position;
		}
		
		public Transform AbeTransform ()
		{
			return transform;
		}
		
		private void AttackEvent ()
		{
			nowState.StateEvent ();
		}
		
		void FinishInDownAnimation ()
		{
			Destroy (gameObject);
		}
		
		public float GetDistance ()
		{
			return (player.transform.position - transform.position).magnitude;
		}
		
		public float GetDirection ()
		{
			return transform.eulerAngles.y;
		}
		
		public void SetTargetPosition (Vector3 targetPos)
		{
			nowState.targetPosition = targetPos;
		}
	}
}
