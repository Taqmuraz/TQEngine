using System;
using System.IO;
using Tools;
using System.Linq;
using System.Collections.Generic;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Rendering;

namespace Models
{
	public struct MeshPoint
	{
		public Vector point;
		public ColorS color;

		public MeshPoint (Vector p, ColorS c) {
			point = p;
			color = c;
		}
	}

	public struct MeshModel
	{
		public MeshPoint[] verticles;
		public int facesCount;

		public MeshModel (MeshPoint[] _v, int _faces) {
			verticles = _v;
			facesCount = _faces;
		}
	}

	public class ModelImporter
	{
		public static string modelsPath
		{
			get {
				return Resources.appPath + "Models/";
			}
		}
		public static Model FromFile (string name) {
			string path = modelsPath + name + '/';
			ExtendedMaterial[] materials;
			Mesh mesh = Mesh.FromFile (path + name + ".x", MeshFlags.Managed, RenderDrawing.device, out materials);
			TexturedMaterial[] textured = new TexturedMaterial[materials.Length];
			for (int i = 0; i < materials.Length; i++) {
				Texture tex = null;
				if (!string.IsNullOrEmpty(materials[i].TextureFilename)) {
					tex = TextureLoader.FromFile (RenderDrawing.device, path + materials [i].TextureFilename);
				}
				textured [i] = new TexturedMaterial (materials [i].Material3D, tex);
			}
			return new Model (mesh, textured);
		}
	}
}

