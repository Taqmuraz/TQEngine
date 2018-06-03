using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using AnimalsSolution;

public class Game_0 {

	public static Game_0 game_0;

	public static PictureBox box;

	public void Start () {
		box = new PictureBox ();
		box.Image = Resources.Load ("Human.png");
		MainClass.screen.Controls.Add (box);
	}
}

