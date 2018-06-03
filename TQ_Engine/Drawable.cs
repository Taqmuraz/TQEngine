using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AnimalsSolution;
using System.Windows.Forms;
using System.Drawing;

public class Slot : IncBehaviour {

	public PictureBox picture { get; private set; }
	public RectangleTrans trans { get; private set; }
	public Bitmap[] pictures { get; private set; }

	public Slot (Vector position, Vector size, string drawableName) {
		trans = new RectangleTrans (position, size);
		picture = new PictureBox ();

		base.Init ();
	}
}

