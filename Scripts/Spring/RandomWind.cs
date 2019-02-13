using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class RandomWind : MonoBehaviour
	{
		private SpringBone[] springBones;
		public bool isWindActive = false;
		
		// Use this for initialization
		void Start ()
		{
			springBones = GetComponent<SpringManager> ().springBones;
		}
		
		// Update is called once per frame
		void Update ()
		{
			Vector3 force = Vector3.zero;
			if (isWindActive) {
				force = new Vector3 (0, 0, Mathf.PerlinNoise (Time.time, 0.0f) * 0.005f);
			}
			
			for (int i = 0; i < springBones.Length; i++) {
				springBones [i].springForce = force;
			}
		}		
	}
}