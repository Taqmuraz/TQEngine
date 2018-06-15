using System;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using Rendering;
using Models;

namespace TQ_Engine
{
	class MainClass
	{
		[STAThread]
		public static void Main (string[] args)
		{
			Application.Run (new RenderForm());
		}
	}
}
