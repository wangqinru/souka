using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GraduationProject
{
	public class Title : MonoBehaviour, IProductionObserver {

		private List<BaseProduction> productionList {get; set;}
		private ProductionInfo information;
		private Canvas canvas;
		private BaseMenu menu;

		private int currentScript = 0;

		// Use this for initialization
		void Start () {

			GameObject gameObject = new GameObject ();
			canvas = gameObject.AddComponent<Canvas> ();
			canvas.name = "Canvas";
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			
			InputManager inputManager = GetComponent<InputManager> ();
			SoundManager soundManager = inputManager.GetComponent<SoundManager> ();
			DataManager dataManager = DataManager.GetInstance ();

			productionList = new List<BaseProduction> ();
			menu = new TitleMenu (canvas, soundManager);
			productionList = menu.GetProductionList ();
			productionList[currentScript].CreateProduction (canvas);

			information = new ProductionInfo (GetComponent<Camera> (), inputManager, dataManager, inputManager.GetComponent<SoundManager> ());
			information.CreateFadeBackGround (canvas);
		}
		
		// Update is called once per frame
		void Update () {
		
			if (currentScript < productionList.Count) 
			{
				productionList[currentScript].Execute (information, this);
			}
			else
			{
				menu.Execute (information.inputManager, this);
			}
		}

		void IProductionObserver.NextCommand ()
		{
			currentScript ++;
			if (currentScript < productionList.Count)
				productionList[currentScript].CreateProduction (canvas);
		}

		void IProductionObserver.NextMenu ()
		{
			menu = menu.nextMenu;
			productionList = menu.GetProductionList ();
			currentScript = 0;
			productionList[currentScript].CreateProduction (canvas);
		}

		void IProductionObserver.CreateMenu ()
		{
			menu.CreateBackGround ();
		}

		void IProductionObserver.Jump (int scriptCounter)
		{
			currentScript = scriptCounter;
		}

		void IProductionObserver.Rewind (int scriptCounter)
		{
			information.fadeBackGroung.color = new Color (1,1,1,0);

			for (int i=1; i<canvas.gameObject.transform.childCount; i++)
				Destroy (canvas.gameObject.transform.GetChild (i).gameObject);
			
			menu = menu.nextMenu;
			productionList = menu.GetProductionList ();
			currentScript = scriptCounter;
			productionList[currentScript].CreateProduction (canvas);
		}
	}
}