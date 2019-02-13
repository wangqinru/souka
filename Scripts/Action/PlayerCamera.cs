﻿using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class PlayerCamera : MonoBehaviour {

		public Vector3 targetPosition {get; set;}
		public Vector2 originAngle {get; set;}
		private Vector2 angle = new Vector2 (0.0f, 110.0f);
		private float distance = 3.0f;
		private float rotateSpeed = 2.5f;
		
		private float autoRotateSpeed_x = 3.0f;
		private float autoRotateSpeed_y = 3.0f;
		private float distanceMoveSpeed = 1.0f; 
		
		private float ROTATE_TIME = 1.0f;
		private float MOVE_TIME = 0.1f;
		private float DISTANCE_NORMAL = 3.0f;
		private float DISTANCE_NEAR = 2.0f;
		private bool snap = false;
		private LayerMask lineofsightMask = 0;
		
		// Use this for initialization
		void Start () {
			
			transform.position = targetPosition - new Vector3 (Mathf.Sin(Mathf.Deg2Rad * angle.x)*distance, 0.0f, Mathf.Cos(Mathf.Deg2Rad * angle.x)*distance);
			
			lineofsightMask = ~((1 << LayerMask.NameToLayer ("nonPlayer")) | (1 << LayerMask.NameToLayer ("Enemy")));
		}
		
		// Update is called once per frame
		void Update () {
		}
		
		Vector3 SetCameraPosition (float h, float v)
		{
			angle += (Mathf.Abs (h) > Mathf.Abs (v)) ? new Vector2 (0.0f, h*rotateSpeed) : new Vector2 (v*rotateSpeed, 0.0f);
			
			if (angle.x > 360.0f) angle.x = 0.0f; if (angle.x < 0.0f) angle.x = 360.0f;
			
			angle.y = Mathf.Clamp (angle.y, 60.0f, 135.0f);
			
			return new Vector3 (Mathf.Sin (Mathf.Deg2Rad * angle.y) * distance * Mathf.Sin (Mathf.Deg2Rad * angle.x), 
			                    Mathf.Cos (Mathf.Deg2Rad * angle.y) * distance, 
			                    Mathf.Sin (Mathf.Deg2Rad * angle.y) * distance * Mathf.Cos (Mathf.Deg2Rad * angle.x));
		}
		
		Vector3 AdjustLineOfSight (Vector3 cameraPosition, Vector3 targetPosition)
		{
			RaycastHit hit;
			
			if (Physics.SphereCast (targetPosition, 0.2f, (cameraPosition-targetPosition).normalized, out hit, distance, lineofsightMask))
			{
				return new Vector3 (hit.point.x, hit.point.y + 0.2f, hit.point.z);
			}
			
			return cameraPosition;
		}
		
		void StartSnap (bool flag)
		{
			if (flag & !snap)
			{
				snap = true;
				StartCoroutine (Snaping (angle));
			}
		}
		
		public void ChangeDistance (float var, Vector3 tp)
		{
			StartCoroutine (SnapingPosition (distance + var, tp));
		}
		
		public void ChangeDistance (float var)
		{
			StartCoroutine (SnapingPosition (distance + var));
		}
		
		public void MoveCamera (InputManager inputManager, bool flag)
		{
			StartSnap (inputManager.SelectButton == 1);
			
			//入力情報を取得
			float v = snap ? 0.0f : inputManager.CameraVertical;
			float h = snap ? 0.0f : inputManager.CameraHorizontal;
			
			if (flag && (Mathf.Abs (v) + Mathf.Abs (h) < 0.01f))
			{
				if (originAngle.x - angle.x > 180.0f) originAngle -= new Vector2 (360.0f, 0.0f);
				if (originAngle.x - angle.x < -180.0f) originAngle += new Vector2 (360.0f, 0.0f);
				
				if (Mathf.Abs(originAngle.x - angle.x) < 100f)
				{
					float nextX = Mathf.SmoothDampAngle (angle.x, originAngle.x, ref autoRotateSpeed_x, ROTATE_TIME);
					float nextY = Mathf.SmoothDampAngle (angle.y, originAngle.y, ref autoRotateSpeed_y, ROTATE_TIME);
					
					angle = new Vector2 (nextX, nextY);
				}
			}
			
			distance = Mathf.SmoothDamp (distance, DISTANCE_NORMAL, ref distanceMoveSpeed, MOVE_TIME); 
			
			transform.position = AdjustLineOfSight (targetPosition - SetCameraPosition (h, v), targetPosition);		
			transform.LookAt (targetPosition);
		}
		
		public void FixationCamera ()
		{
			float backAngle = 100.0f;
			
			if (originAngle.x - angle.x > 180.0f) originAngle -= new Vector2 (360.0f, 0.0f);
			if (originAngle.x - angle.x < -180.0f) originAngle += new Vector2 (360.0f, 0.0f);
			
			float nextX = Mathf.SmoothDampAngle (angle.x, originAngle.x, ref autoRotateSpeed_x, MOVE_TIME);
			float nextY = Mathf.SmoothDampAngle (angle.y, backAngle, ref autoRotateSpeed_y, MOVE_TIME);
			
			angle = new Vector2 (nextX, nextY);
			distance = Mathf.SmoothDamp (distance, DISTANCE_NEAR, ref distanceMoveSpeed, MOVE_TIME);
			
			transform.position = AdjustLineOfSight (targetPosition - SetCameraPosition (0f, 0f), targetPosition);
			transform.LookAt (targetPosition);
		}
		
		IEnumerator Snaping (Vector3 nowAngle)
		{
			float nTime = 0.0f;
			float t = 0.1f;
			
			if (originAngle.x - nowAngle.x > 180.0f) originAngle -= new Vector2 (360.0f, 0.0f);
			if (originAngle.x - nowAngle.x < -180.0f) originAngle += new Vector2 (360.0f, 0.0f);
			
			while (nTime != 1.0f)
			{
				nTime += t; t -= (t>= 0.02f) ? 0.001f : 0;
				
				angle.x = Mathf.Lerp (nowAngle.x, originAngle.x, nTime);
				angle.y = Mathf.Lerp (nowAngle.y, originAngle.y, nTime);
				
				if (nTime >= 1) {nTime = 1; break;}
				
				yield return 0;
			}
			
			snap = false;
		}
		
		IEnumerator SnapingPosition (float td)
		{	
			float nTime = 0.0f;
			float t = 0.05f;
			
			float nd = distance;
			
			while (nTime != 1.0f)
			{
				nTime += t;
				
				nTime += t; t -= (t>= 0.02f) ? 0.001f : 0;
				
				distance = Mathf.Lerp (nd, td, nTime);
				
				if (nTime >= 1) {nTime = 1; break;}
				
				yield return 0;
			}
			
		}
		
		IEnumerator SnapingPosition (float td, Vector3 tpos)
		{
			Vector3 npos = targetPosition;
			
			float nTime = 0.0f;
			float t = 0.05f;
			
			float nd = distance;
			
			while (nTime != 1.0f)
			{
				nTime += t;
				
				nTime += t; t -= (t>= 0.02f) ? 0.001f : 0;
				
				distance = Mathf.Lerp (nd, td, nTime);
				
				float nposx = Mathf.Lerp (npos.x, tpos.x, nTime);
				float nposy = Mathf.Lerp (npos.y, tpos.y, nTime);
				float nposz = Mathf.Lerp (npos.z, tpos.z, nTime);
				
				targetPosition = new Vector3 (nposx, nposy, nposz);
				
				if (nTime >= 1) {nTime = 1; break;}
				
				yield return 0;
			}			
		}
	}
}
