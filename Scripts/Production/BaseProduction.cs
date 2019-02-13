using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;

//演出ベースクラス
namespace GraduationProject
{
	public class BaseProduction {

		protected float current = 0;
		protected float target = 0;
		protected float counter = 0;
		protected int time = 0;

		static protected float ratio_width = Screen.width / 1920f;
		static protected float ratio_height = Screen.height / 1080f;
		static protected bool skip = false;

		public BaseProduction ()
		{
		}

		public virtual void Execute (ProductionInfo information, IProductionObserver observer)
		{
			if (information.inputManager.StartButton == 1)
				skip = true;
		}

		public virtual void CreateProduction (Canvas canvas)
		{
		}

		public JsonData Json {get; set;}
	}

	public class ProductionInfo {

		public Camera camera {get; private set;}
		public Image fadeBackGroung {get; private set;}
		public InputManager inputManager {get; private set;}
		public DataManager dataManager {get; private set;}
		public SoundManager soundManager {get; private set;}

		public ProductionInfo (Camera main, InputManager inputMan, DataManager dataMan, SoundManager soundMan)
		{
			camera = main;
			inputManager = inputMan;
			dataManager = dataMan;
			soundManager = soundMan;
		}

		public void CreateFadeBackGround (Canvas canvas)
		{
			GameObject fadeObject = new GameObject ();
			fadeObject.name = "Fade";
			fadeObject.transform.parent = canvas.transform;
			fadeBackGroung = fadeObject.AddComponent<Image> ();
			fadeBackGroung.sprite = Resources.Load ("UserInterface/fade", typeof(Sprite)) as Sprite;

			RectTransform rectTransform = fadeBackGroung.GetComponent <RectTransform> ();
			rectTransform.anchorMax = Vector2.zero;
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.position = new Vector3 (Screen.width/2f, Screen.height/2f, 0f);
			rectTransform.sizeDelta = new Vector2 (Screen.width, Screen.height);

			fadeBackGroung.color = new Color (1f, 1f, 1f, 0f);
		}
	}
}