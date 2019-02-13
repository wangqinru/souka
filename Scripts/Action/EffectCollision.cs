using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class EffectCollision : MonoBehaviour {

		private float force_z = 3f;
		private float force_y = 0f;
		private int durationMax = 10;
		private int attack = 0;
		private int duration = 10;
		private Material effectMat;
		private ParticleSystem particle;

		private float startTime;
		// Use this for initialization
		void Start () {
		}
		
		// Update is called once per frame
		void Update () {

			if (particle == null)
			{
				Color color = effectMat.GetColor ("_TintColor");
				float alhpa = (float)duration/(float)durationMax / 3f;
				effectMat.SetColor ("_TintColor", new Color (color.r, color.g, color.b, alhpa));
				if (duration-- < 0)
				{
					Destroy (gameObject);
				}
			}
			else
			{
				if (!particle.IsAlive ())
					Destroy (gameObject);
			}

		}
		
		public void Prepare (int atk, float z, float y, int count)
		{
			force_z = z;
			force_y = y;
			attack = atk;
			duration = count;
			durationMax = count;
			effectMat = GetComponentInChildren<Renderer> ().material;
			particle = GetComponentInChildren<ParticleSystem> ();
			startTime = Time.time;
		}
		
		void OnTriggerEnter (Collider other)
		{
			if (other.tag == "Enemy")
			{
				//print ("hit");
				EnemyController enemy = other.GetComponent<EnemyController> ();
				enemy.Push (force_z, force_y, transform.TransformDirection (Vector3.forward));
				enemy.PlayDamageMotion (attack);
				transform.GetChild (1).gameObject.SetActive (true);
			}
		}

		void OnTriggerStay (Collider other)
		{
			if (other.tag == "Enemy")
			{
				if (particle != null && Time.time - startTime > 0.5f)
				{
					startTime = Time.time;
					EnemyController enemy = other.GetComponent<EnemyController> ();
					enemy.Push (force_z, force_y, transform.TransformDirection (Vector3.forward));
					enemy.PlayDamageMotion (attack);
					transform.GetChild (1).gameObject.SetActive (true);
				}
			}
		}
	}
}
