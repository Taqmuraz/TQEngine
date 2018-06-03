using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace AnimalsSolution
{
	class MainClass
	{
		public static GameScreen screen = new GameScreen();
		public static Thread thread;
		public static void Main (string[] args)
		{
			
			thread = new Thread (GameScreen.ThreadUpdate);
			thread.Start ();
			Application.Run (screen);
		}
	}

	public class GameScreen : Form
	{
		

		public GameScreen () {
			Game_0.game_0 = new Game_0 ();

			TextBox text = new TextBox ();

			text.SetBounds (100, 100, 100, 50);
		}

		public static void ThreadUpdate () {

			Game_0.game_0.Start ();

			while (true) {
				MainClass.screen.Update ();
			}
		}

		public static int width
		{
			get
			{
				if (MainClass.screen != null) {
					return MainClass.screen.Width;
				} else {
					return 0;
				}
			}
		}
		public static int height
		{
			get
			{
				if (MainClass.screen != null) {
					return MainClass.screen.Height;
				} else {
					return 0;
				}
			}
		}
		public static Vector mousePosition
		{
			get {
				return (PointF)MainClass.screen.PointToClient (MousePosition);
			}
		}
	}
}
