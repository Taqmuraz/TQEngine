using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Linq;
using d3d = Microsoft.DirectX.Direct3D;
using Rendering;

namespace TQ_Engine
{
	public class Animatable
	{
		public Bone rootBone { get; private set; }
	}
	public class Bone
	{
		public Renderer renderer { get; private set; }
		public Bone parent { get; private set; }
		public Bone[] childs { get; private set; }

		public Bone (Renderer _renderer, Bone _parent, params Bone[] _childs) {
			renderer = _renderer;
			parent = _parent;
			childs = _childs;
			foreach (var c in _childs) {
				c.parent = this;
			}
		}
	}
}

