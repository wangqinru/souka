using UnityEngine;
using System.Collections;

namespace GraduationProject
{
	public class InputManager : MonoBehaviour {

		public float Vertical {get; private set;}
		public float Horizontal {get; private set;}

		public float CameraVertical {get; private set;}
		public float CameraHorizontal {get; private set;}

		public int SelectButton {get; private set;}
		public int Jump {get; set;}

		public int StartButton {get; private set;}
		public int AttackButton {get; private set;}
		public int GuardButton {get; private set;}
		public int DecisionButton {get; private set;} 
		public int CancelButton {get; private set;}
		public int SkillButton {get; private set;}
		public int ChargeButton {get; private set;}

		public int StickLR { get; private set;}
		public int StickUD { get; private set;}
		
		public float DpadX { get; private set;}
		public float DpadY { get; private set;}

		public int speed { get; set;}
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
			Vertical = Input.GetAxisRaw ("Vertical");
			Horizontal = Input.GetAxisRaw ("Horizontal");

			DpadX = Input.GetAxisRaw ("DpadX");
			DpadY = Input.GetAxisRaw ("DpadY");

			CameraVertical = Input.GetAxisRaw ("CameraVertical");
			CameraHorizontal = Input.GetAxisRaw ("CameraHorizontal");

			SelectButton = Input.GetButton ("Select") ? SelectButton+1 : 0;
			Jump = Input.GetButton ("Jump") ? Jump+1 : 0;
			StartButton = Input.GetButton ("Start") ? StartButton+1 : 0;
			AttackButton = Input.GetButton ("Attack") ? AttackButton+1 : 0;
			GuardButton = Input.GetButton ("Guard") ? GuardButton+1 : 0; 
			CancelButton = Input.GetButton ("Jump") ? CancelButton+1 : 0;
			DecisionButton = Input.GetButton ("Decision") ? DecisionButton+1 : 0;
			SkillButton = Input.GetButton ("Skill") ? SkillButton+1 : 0;
			ChargeButton = Input.GetButton ("Charge") ? ChargeButton+1 : 0;

			if (Mathf.Abs (Horizontal) > Mathf.Abs (Vertical) || Mathf.Abs (DpadX) > Mathf.Abs (DpadY))
			{	
				if (Horizontal > 0.01f || DpadX > 0.01f)
				{
					if (StickLR < 0) StickLR = 0;
					StickLR ++;
					if (StickLR > 30+speed) StickLR = 0;
				}

				if (Horizontal < -0.01f || DpadX < -0.01f)
				{
					if (StickLR > 0) StickLR = 0;
					StickLR --;
					if (StickLR < -30-speed) StickLR = 0;
				}
			}
			else
				StickLR = 0;
			
			if (Mathf.Abs (Vertical) > Mathf.Abs (Horizontal) || Mathf.Abs (DpadY) > Mathf.Abs (DpadX))
			{	
				if (Vertical > 0.01f || DpadY > 0.01f)
				{
					if (StickUD > 0) StickUD = 0;
					StickUD --;
					if (StickUD < -30-speed) StickUD = 0;
				}
				
				if (Vertical < -0.01f || DpadY < -0.01f)
				{
					if (StickUD < 0) StickUD = 0;
					StickUD ++;
					if (StickUD > 30+speed) StickUD = 0;
				}
			}
			else
				StickUD = 0;
		}

		public void ClearAxis ()
		{
			Vertical = 0.0f;
			Horizontal = 0.0f;
		}
	}
}
