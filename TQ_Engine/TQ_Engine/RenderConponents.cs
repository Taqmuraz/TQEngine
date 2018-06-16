using System;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using System.Drawing;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace Rendering
{

	public struct TexturedMaterial
	{
		public Material material;
		public Texture texture;

		public TexturedMaterial (Material _mat, Texture _tex)
		{
			material = _mat;
			texture = _tex;
		}
	}

	public struct Model
	{
		public Mesh mesh;
		public TexturedMaterial[] materials;

		public Model (Mesh _mesh, params TexturedMaterial[] _mats) {
			mesh = _mesh;
			materials = _mats;
		}
	}

	public class Renderer
	{
		public Matrix4x4 matrix { get; private set; }
		public Model model { get; private set; }

		public Renderer (Matrix4x4 _matrix, Model _model) {
			matrix = _matrix;
			model = _model;
			RenderForm.renderers.Add (this);
		}

		public void Destroy () {
			RenderForm.renderers.Remove (this);
			foreach (var m in model.materials) {
				m.texture.Dispose ();
			}
			model.mesh.Dispose ();
		}
	}

	public class Camera : Matrix4x4
	{
		public float drawDistance { get; private set; }

		public void MoveAt (Vector3 at) {
			globalPosition = at * this;
		}

		public Camera (float dist, Vector _position, Vector _fwd, Vector _up) {
			drawDistance = dist;
			globalPosition = _position;
			forward = _fwd;
			up = _up;
			scale = Vector.one;
		}
	}
}

