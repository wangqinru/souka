using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class waterfall : MonoBehaviour {

		private Material mainTexture;
		public float speed_x = 0f;
		public float speed_y = 0f;

		// Use this for initialization
		void Start () {
		
			mainTexture = GetComponent<Renderer> ().material;
		}
		
		// Update is called once per frame
		void Update () {

			float offset_x = 0f;
			float offset_y = 0f;

			if (speed_x > 0f) offset_x = Mathf.Repeat (Time.time, speed_x) / speed_x;
			if (speed_y > 0f) offset_y = Mathf.Repeat (Time.time, speed_y) / speed_y;

			mainTexture.SetTextureOffset ("_MainTex", new Vector2 (offset_x, offset_y));
		}
	}
}
