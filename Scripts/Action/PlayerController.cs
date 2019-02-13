using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class PlayerController : MonoBehaviour {

		private InputManager inputManager;
		private PlayerInformation playerInfo;

		private ActionState nowState;
		
		public float nowSpeed {get; private set;}
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {
			
			CharacterController _characterController = GetComponent<CharacterController> ();
			
			nowState = nowState.nextState;
			playerInfo.SetCameraTargetPosition (transform);
			nowSpeed = playerInfo.speed;
			
			nowState.isGround = playerInfo.IsGrounded ();
			nowState.GraityController ();
			Vector3 movement = nowState.execution (inputManager);
			movement *= Time.deltaTime;
			_characterController.Move (movement);

		}
		
		
		void BackToMoveMotion ()
		{
			playerInfo.SetSwordActive (1);
			nowState = new MoveState ();
			nowState.InitNextState (playerInfo);
			playerInfo.animator.CrossFade ("Idle", 0.2f);
		}
		
		void NowActionEvent (int index)
		{
			nowState.ActionEvent (index);
		}
		
		
		public void Damage (int damage, Vector3 pos)
		{
			string[] motionNames = new string[3] {"Damage_Front", "Damage_Side", "Damage_Back"};
			Vector3 dir = transform.InverseTransformPoint (pos);

			int index = 0;
			if (dir.z > 0) index = Random.Range (0, 2);
			else index = Random.Range (1, 3);

			nowState.DamageEvent (damage, motionNames[index]);
		}

		public float GetNowHp ()
		{
			return (float)playerInfo.hp / (float)playerInfo.characterInfo.status.hp;
		}

		public void Prepare (GameManager gm, PlayerCamera ca, CharacterData data)
		{
			inputManager = gm.GetComponent<InputManager> ();
			
			WeaponObject[] swordOjects = new WeaponObject[2];
			swordOjects = GetComponentsInChildren<WeaponObject> ();

			playerInfo = new PlayerInformation (data, gm, ca, GetComponent<Animator> (), swordOjects, transform);
			nowState = new MoveState ();
			nowState.InitNextState (playerInfo);
			playerInfo.SetSwordActive (1);
			playerInfo.SetCameraTargetPosition (transform);
		}
	}
}