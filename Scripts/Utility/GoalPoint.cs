using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GraduationProject
{
	public class GoalPoint : MonoBehaviour {

		private UnitMarker marker;
		private UnitMarker guideImage;
		private Image manualImage;

		// Use this for initialization
		void Start () {
		
			marker = new UnitMarker ("mpicon_04", new Vector2 (22, 22));
			marker.SetMarkerPosition (transform.position);
			guideImage = new UnitMarker ("mark", new Vector2 (100, 100));
			guideImage.SetMarkerPosition (transform.position);
			guideImage.GetMarkerImage ().color = new Color (1, 1, 1, 0f);

			GameObject obj = new GameObject ();
			obj.name = "manualImage";
			obj.transform.parent = BattleUI.canvas.transform;
			
			manualImage = obj.AddComponent <Image> ();
			manualImage.sprite = Resources.Load ("UserInterface/temp_box", typeof(Sprite)) as Sprite;
			manualImage.rectTransform.anchorMax = Vector2.zero;
			manualImage.rectTransform.anchorMin = Vector2.zero;
			manualImage.rectTransform.pivot = Vector2.zero;
			
			manualImage.rectTransform.position = new Vector3 (10*BattleUI.ratio_width, 460*BattleUI.ratio_height, 0f);
			manualImage.rectTransform.sizeDelta = new Vector2 (1020*BattleUI.ratio_width, 133*BattleUI.ratio_height);
			manualImage.color = new Color (1, 1, 1, 0f);

			InvokeRepeating ("StartManualAnimation", 2f, 60f);

		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void StartManualAnimation ()
		{
			StartCoroutine ("ManualAnimation");
		}

		IEnumerator ManualAnimation ()
		{
			int counter = 0;
			Image guide = guideImage.GetMarkerImage ();
			guide.color = new Color (1, 1, 1, 1);
			manualImage.color = new Color (1, 1, 1, 1);
			guide.rectTransform.pivot = Vector2.one / 2f;
			float current = 200*BattleUI.ratio_width;
			guide.rectTransform.sizeDelta = new Vector2 (current, current);

			while (counter++ < 60)
			{
				float size = Mathf.Lerp (current, 10f, (float)counter/60f);
				guide.rectTransform.sizeDelta = new Vector2 (size, size);

				yield return 0;
			}

			yield return new WaitForSeconds (1f);
			manualImage.color = new Color (1, 1, 1, 0f);
			guide.color = new Color (1, 1, 1, 0f);
		}

		void OnTriggerEnter (Collider other)
		{
			if (other.tag == "Player")
			{
				GameObject.Find ("GameManager").GetComponent<GameManager> ().Result (0);
				this.GetComponent<Collider>().enabled = false;
			}

		}
	}
}
