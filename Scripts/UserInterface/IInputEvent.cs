using System;

namespace GraduationProject
{
	public interface IInputEvent
	{
		void UpdateAnimation ();
		void ButtonEvent (int input, IGameManager igameManager);
		void MoveCursor (int input);
		void CreateCanvas (DataManager dataManager);
		IInputEvent NextMenu ();
	}
}

