using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GraduationProject
{
	public class CreateEnemyPoint : MonoBehaviour {

		public PlayerController player {get; set;}
		private GameManager gameManager;
		private SoundManager sounderManager;
		private List<CaptainEnemy> captainList {get; set;}

		private float MOVE_RANGE = 10.0f;
		private int MAX_CAPTAIN = 3;
		private float MIN_DISTANCE = 2.0f;

		public bool moveFlag {get; set;}
		public float timeCounter {get; set;}

		void Awake ()
		{
			captainList = new List<CaptainEnemy> ();
		}

		// Use this for initialization
		void Start () {

			gameManager = GetComponentInParent <GameManager> ();
			sounderManager = gameManager.GetComponent<SoundManager> ();
			CreateEnemy (MAX_CAPTAIN);
		}
		
		// Update is called once per frame
		void Update () {

			foreach (CaptainEnemy captain in captainList)
				captain.PushAbeSan ();
		}

		private void CreateEnemy (int number)
		{
			for (int i=0; i<number; i++)
			{
				GameObject captain = new GameObject ("Captain");
				captain.name = "Captain00"+i.ToString ();
				captain.tag = "Captain";
				Vector3 pos = transform.TransformPoint (new Vector3 (Random.Range (1f, -1f), 0f, i*-2.0f +Random.Range (-1f, 0f)));
				captain.transform.position = pos;
				captain.transform.parent = transform;
				CaptainEnemy captainAbe = captain.AddComponent<CaptainEnemy> ();
				Rigidbody rigid = captain.AddComponent<Rigidbody> ();
				rigid.useGravity = false;

				int index = Random.Range (0, 2);
				captainAbe.Prepare (player, gameManager.dataManager.enemyData[index]);
				captainAbe.LookAtPlayer ();
				captainList.Add (captainAbe);
			}
		}

		public void SetCaptainTargetPosition ()
		{
			List<CaptainEnemy> tempList = new List<CaptainEnemy> ();
			
			for (int i=0; i<captainList.Count; i++)
			{
				if (captainList[i].GetDistance () < MOVE_RANGE)
				{
					tempList.Add (captainList[i]);
				}
			}

			List<CaptainEnemy> forward = new List<CaptainEnemy> ();
			List<CaptainEnemy> back = new List<CaptainEnemy> ();

			while (tempList.Count > 0)
			{
				int ind = 0;
				float tempDis = captainList[0].GetDistance ();
				for (int i=1; i<tempList.Count; i++)
				{
					if (tempList[i].GetDistance () < tempDis)
					{
						ind = i;
						tempDis = tempList[i].GetDistance ();
					}
				}

				float ey = tempList[ind].transform.eulerAngles.y;

				Vector3 dir = player.transform.position - tempList[ind].transform.position;
				dir.y = 0f;
				dir = dir.normalized;
				tempList[ind].transform.rotation = Quaternion.LookRotation (dir);

				float dif = Mathf.Abs (tempList[ind].transform.eulerAngles.y - ey);
				int rotateAngle = dif > 170f && dif < 270f ? -1 : 1;
				Vector3 ez = player.transform.InverseTransformPoint (tempList[ind].transform.position);
				
				Vector3 localPos;
				if (ez.z > 0)
				{
					if (forward.Count > 0)
					{
						localPos = forward[forward.Count-1].transform.InverseTransformPoint (tempList[ind].transform.position);
						localPos = localPos.normalized*1.5f;
						localPos = forward[forward.Count-1].transform.TransformPoint (localPos);
					}
					else
					{
						localPos = player.transform.InverseTransformPoint (tempList[ind].transform.position);
						localPos = localPos.normalized;
						localPos = player.transform.TransformPoint (localPos);
					}
					
					forward.Add (tempList[ind]);
				}
				else
				{
					if (back.Count > 0)
					{
						localPos = back[back.Count-1].transform.InverseTransformPoint (tempList[ind].transform.position);
						localPos = localPos.normalized*1.5f;
						localPos = back[back.Count-1].transform.TransformPoint (localPos);
					}
					else
					{
						localPos = player.transform.InverseTransformPoint (tempList[ind].transform.position);
						localPos = localPos.normalized;
						localPos = player.transform.TransformPoint (localPos);
					}
					
					back.Add (tempList[ind]);
				}
				
				if ((localPos - tempList[ind].transform.position).magnitude < MIN_DISTANCE) return;
				tempList[ind].transform.position = localPos;
				tempList[ind].SetAbeSanTargetPosition (rotateAngle);
				
				tempList.Remove (tempList[ind]);
			}
		}

		public void PlayHitSE ()
		{
			sounderManager.PlayHitSE ();
		}

		private float GetNearEnemyDistance ()
		{
			float tempDis = captainList[0].GetDistance ();
			for (int i=0; i<captainList.Count; i++)
			{
				if (captainList[i].GetDistance () < tempDis)
					tempDis = captainList[i].GetDistance ();
			}

			return tempDis;
		}

		public void RemoveEnemyCaptain (CaptainEnemy cab)
		{
			captainList.Remove (cab);

			if (captainList.Count <= 0)
			{
				CreateEnemy (MAX_CAPTAIN);
			}
		}

		public float GetCaptainDistance ()
		{
			if (captainList.Count > 0)
				return captainList[0].GetDistance ();

			return 100.0f;
		}

		public float GetCaptainDirection ()
		{
			transform.LookAt (player.transform.position);

			if (captainList.Count > 0)
				return captainList[0].transform.eulerAngles.y;

			return 180.0f;
		}

		public void SetKillNumber ()
		{
			gameManager.killerNumber ++;
			gameManager.battleUI.DrawKillNumber (gameManager.killerNumber);
		}

		public void SetEnemyEnable (bool flag)
		{
			foreach (CaptainEnemy captain in captainList)
				captain.SetEnemyEnable (flag);
		}
	}
}
