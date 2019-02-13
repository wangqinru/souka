using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class PlayerInformation {

		public PlayerCamera trackCamera {get; private set;}
		public Animator animator {get; private set;}
		public WeaponObject[] sword {get; private set;}
		public Transform transform {get; private set;}
		public GameManager gameManager {get; private set;}
		public SoundManager soundManager {get; private set;}
		public CharacterData characterInfo {get; set;}

		public int hp {get; set;}
		public float speed {get; set;}
		public float verticalSpeed {get; set;}
		public float horizontalSpeed {get; set;}
		public float gravity {get; set;}
		public float accele {get; set;}
		public int jumpNumber {get; set;}
		
		public PlayerInformation ()
		{
			
		}
		
		public PlayerInformation (CharacterData data, GameManager gm, PlayerCamera ca, Animator anim, WeaponObject[] sw, Transform me)
		{
			gameManager = gm;
			soundManager = gameManager.GetComponent<SoundManager> ();
			trackCamera = ca;
			animator = anim;
			sword = sw;
			transform = me;
			characterInfo = data;

			hp = characterInfo.status.hp;
			speed = characterInfo.status.agile/100f;
			verticalSpeed = 0.0f;
			horizontalSpeed = 0.0f;
			gravity = 10.0f;
			accele = 5.0f;
			jumpNumber = 0;
		}
		
		public void SetSwordActive (int index)
		{
			for (int i=0; i<sword.Length; i++)
			{
				if (i == index)sword[i].gameObject.SetActive (false);
				else sword[i].gameObject.SetActive (true);
			}
		}
		
		public void SetCameraTargetPosition (Transform son)
		{
			trackCamera.targetPosition = son.transform.position + new Vector3 (0.0f, 0.9f, 0.0f);
			trackCamera.originAngle = new Vector2 (son.transform.eulerAngles.y, 110.0f);
		}
		
		public bool IsGrounded ()
		{
			Vector3 groundPosition = transform.position - new Vector3 (0f, 10f, 0f);
			RaycastHit hit;
			
			if (Physics.Linecast (transform.position + new Vector3 (0, 0.2f, 0f), groundPosition, out hit))
			{
				groundPosition = hit.point;
			}

			CharacterController characterController_ = transform.GetComponent<CharacterController> ();
			
			return characterController_.isGrounded || ((transform.position.y - groundPosition.y) < 0.1f);
		}
	}
}
