using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GraduationProject
{
	public class CaptainEnemy : MonoBehaviour {

		private EnemyData childEnemy;
		protected PlayerController player;
		protected Animator animator;
		private UnitMarker marker;
		private List<EnemyController> enemyList;
		private CreateEnemyPoint parentPoint;
		private Vector3[] memberPosition;
		
		private int ENEMY_MAX = 5;
		private float STOP_RANGE = 0.5f;
		
		protected float distance;
		protected float timer;
		
		public float WAIT_TIME = 3.0f;
		
		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {

			if (enemyList.Count > 0 && enemyList[0].enabled) marker.SetMarkerPosition (enemyList[0].transform.position);
		}
		
		private void CreateAbeSan ()
		{
			GameObject enemyObject = Resources.Load (childEnemy.modlePath, typeof (GameObject)) as GameObject;
			
			//(float)(-3+(i+1)*i-i*(i-1)) + Random.Range (0.1f, 1.5f), 0.0f, Random.Range (0.1f, 0.5f)
			int rand = Random.Range (0, 3);
			for (int i=0; i<ENEMY_MAX+rand; i++)
			{
				Vector3 pos = transform.TransformPoint (new Vector3 ((float)(-3.5f+(i%10)*1) + Random.Range (-0.2f, 0.2f), 0.0f, (i/10)* -1.5f + Random.Range (0.1f, 0.5f)));
				GameObject enemy = Instantiate (enemyObject, 
				                                pos,
				                                parentPoint.transform.rotation) as GameObject;
				enemy.transform.parent = transform.parent;
				EnemyController enemyCtrl = enemy.AddComponent<EnemyController> ();
				enemyCtrl.Prepare (player,this, childEnemy);
				enemyList.Add (enemyCtrl);

				Renderer faceRender = enemy.transform.Find ("head").GetComponent<Renderer> ();
				faceRender.material.SetTextureOffset ("_MainTex", new Vector2 (Random.Range (0, 2)/2f, Random.Range (0, 2)/2f));
			}
		}
		
		public void PushAbeSan ()
		{
			foreach (EnemyController abe in enemyList)
			{
				if (abe.GetDistance () <= STOP_RANGE)
				{
					Vector3 side = player.transform.InverseTransformPoint (abe.AbeTransform ().position);
					
					Vector3 dir = side.x > 0 ? player.transform.TransformDirection (Vector3.right) : player.transform.TransformDirection (Vector3.left);
					dir.y = 0;
					dir = dir.normalized;
					
					abe.Push (1.0f, 0f, dir);
				}
			}
		}
		
		public void SetAbeSanTargetPosition (int rotateAngle)
		{
			for (int i=0; i<enemyList.Count; i++)
			{
				Vector3 pos = transform.TransformPoint (new Vector3 ((float)(-3.5f*rotateAngle+(i%10)*1*rotateAngle)+Random.Range (-0.2f, 0.2f), 0.0f, (i/10)* -1.5f+Random.Range (0.1f, 0.6f)));
				enemyList[i].SetTargetPosition (pos);
			}
		}
		
		public float GetDistance ()
		{
			distance = (player.transform.position - transform.position).magnitude;
			return distance;
		}
		
		public void ArrangementAbeSan ()
		{
			Vector3 toPlayer = (player.transform.position - transform.position).normalized;
			toPlayer.y = 0f;
			toPlayer = toPlayer.normalized;
			
			transform.rotation = Quaternion.LookRotation (toPlayer);
			memberPosition = new Vector3[enemyList.Count];
			
			for (int i=0; i<memberPosition.Length; i++)
			{
				memberPosition[i] = transform.TransformPoint ((float)(-2.5f+i*1) + Random.Range (-0.5f, 0.5f), 0.0f, Random.Range (0.1f, 0.5f));
			}
		}
		
		public void RemoveAbeSan (EnemyController enemy)
		{
			enemyList.Remove (enemy);
			parentPoint.SetKillNumber ();
			if (Random.Range (0, ENEMY_MAX) > enemyList.Count)
			{
				for (int i=0; i<enemyList.Count; i++)
				{
					enemyList[i].Access (1.5f + (i/3)*1f);
				}
			}
			if (enemyList.Count <= 0)
			{
				parentPoint.RemoveEnemyCaptain (this);
				Destroy (gameObject);
				marker.Destory ();
			}
		}
		
		public void LookAtPlayer ()
		{
			Vector3 direction = player.transform.position - transform.position;
			direction.y = 0f;
			direction = direction.normalized;
			
			transform.rotation = Quaternion.LookRotation (direction);
		}

		public void PlayHitSE ()
		{
			parentPoint.PlayHitSE ();
		}

		public void SetEnemyEnable (bool flag)
		{
			foreach (EnemyController enemy in enemyList)
			{
				enemy.enabled = flag;
				enemy.GetComponent<Animator> ().enabled = flag;
			}
		}

		public virtual void Prepare (PlayerController son, EnemyData data)
		{
			player = son;
			enemyList = new List<EnemyController> ();
			animator = GetComponent<Animator> ();
			distance = (player.transform.position - transform.position).magnitude;
			parentPoint = GetComponentInParent <CreateEnemyPoint> ();
			childEnemy = data;
			CreateAbeSan ();
			marker = new UnitMarker ();
			timer = Time.time;
		}
	}
}
