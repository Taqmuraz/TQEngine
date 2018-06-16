using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System;
using System.IO;
using System.Windows.Forms;
using TQ_Engine;
using Tools;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using inp = Microsoft.DirectX.DirectInput;

public class IncBehaviour
{
	public static void Destroy (IncBehaviour behaviour) {
		incBehaviours.Remove (behaviour);
	}

	public virtual void Init () {
		incBehaviours.Add (this);
	}

	public static List<IncBehaviour> incBehaviours
	{
		get {
			return incBehavioursGetter;
		}
		set {
			incBehavioursGetter = value;
		}
	}

	private static List<IncBehaviour> incBehavioursGetter = new List<IncBehaviour> ();

	public static T[] FindObjectsOfType<T> () {
		return incBehaviours.Where ((IncBehaviour arg) => arg is T).Cast<T> ().ToArray ();
	}
}
public struct ColorS
{
	public int r;
	public int g;
	public int b;
	public int a;

	public ColorS (int _r, int _g, int _b) {
		r = _r;
		g = _g;
		b = _b;
		a = 255;
	}
	public ColorS (int _r, int _g, int _b, int _a) {
		r = _r;
		g = _g;
		b = _b;
		a = _a;
	}

	public static implicit operator ColorS (Color a) {
		return new ColorS (a.R, a.G, a.G, a.A);
	}
	public static implicit operator Color (ColorS a) {
		return Color.FromArgb (a.a, a.r, a.g, a.b);
	}
	public static implicit operator int (ColorS a) {
		return ((Color)a).ToArgb ();
	}
	public static implicit operator ColorS (int a) {
		return Color.FromArgb (a);
	}
}
public struct Vector
{
	public float x;
	public float y;
	public float z;

	public Vector (float _x, float _y, float _z) {
		x = _x;
		y = _y;
		z = _z;
	}
	public Vector (float _x, float _y) {
		x = _x;
		y = _y;
		z = 0;
	}
	public static implicit operator Vector (string s) {
		string[] parts = TextParser.ToWords (s);
		float[] fs = new float[3];
		for (int i = 1; i < parts.Length; i++) {
			fs [i - 1] = TextParser.ParceFloat (parts[i]);
		}
		return new Vector (fs [0], fs [1], fs [2]);
	}
	public static implicit operator Vector (SizeF s) {
		return new Vector (s.Width, s.Height);
	}
	public static implicit operator SizeF (Vector v) {
		return new SizeF (v.x, v.y);
	}
	public static implicit operator Vector (PointF s) {
		return new Vector (s.X, s.Y);
	}
	public static implicit operator PointF (Vector v) {
		return new PointF (v.x, v.y);
	}

	public static implicit operator Vector (Point s) {
		return new Vector (s.X, s.Y);
	}
	public static implicit operator Point (Vector v) {
		return new Point ((int)v.x, (int)v.y);
	}

	public static implicit operator Vector4 (Vector v) {
		return new Vector4 (v.x, v.y, v.z, 1);
	}
	public static implicit operator Vector (Vector4 v) {
		return new Vector (v.X, v.Y, v.Z);
	}

	public static implicit operator Vector3 (Vector v) {
		return new Vector3 (v.x, v.y, v.z);
	}
	public static implicit operator Vector (Vector3 v) {
		return new Vector (v.X, v.Y, v.Z);
	}

	public static Vector operator * (Vector a, float b) {
		return new Vector (a.x * b, a.y * b, a.z * b);
	}
	public static Vector operator / (Vector a, float b) {
		return new Vector (a.x / b, a.y / b, a.z / b);
	}
	public static Vector operator + (Vector a, Vector b) {
		return new Vector (a.x + b.x, a.y + b.y, a.z + b.z);
	}
	public static Vector operator - (Vector a, Vector b) {
		return new Vector (a.x - b.x, a.y - b.y, a.z - b.z);
	}
	public static Vector operator - (Vector a) {
		return new Vector (-a.x, -a.y, -a.z);
	}
	public static Vector right
	{
		get {
			return new Vector (1, 0);
		}
	}
	public static Vector left
	{
		get {
			return new Vector (-1, 0);
		}
	}
	public static Vector up
	{
		get {
			return new Vector (0, 1);
		}
	}
	public static Vector one
	{
		get {
			return new Vector (1, 1, 1);
		}
	}
	public static Vector down
	{
		get {
			return new Vector (0, -1);
		}
	}
	public static Vector forward
	{
		get {
			return new Vector (0, 0, 1);
		}
	}
	public static Vector back
	{
		get {
			return new Vector (0, 0, -1);
		}
	}
	public static Vector zero
	{
		get {
			return new Vector (0, 0, 0);
		}
	}
	public float magnitude
	{
		get {
			return (float)Math.Sqrt ((double)(x * x + y * y + z * z));
		}
	}
	public Vector normalized
	{
		get {
			return magnitude > 0 ? new Vector (x, y, z) / magnitude : Vector.zero;
		}
	}
	public override string ToString () {
		return "Vector : [" + x + ", " + y + ", " + z + "]";
	}
}
public class Mathf
{
	public static float Sin (float a) {
		return (float)Math.Sin (a);
	}
	public static float Cos (float a) {
		return (float)Math.Cos (a);
	}
	public const float Deg2Rad = (float)Math.PI / 180f;
	public const float Rad2Deg = 180f / (float)Math.PI;
}
public struct Rect
{

	public Rect (Vector pos, Vector sz) {
		position = pos;
		size = sz;
	}

	public Vector position;
	public Vector size;

	public Vector max
	{
		get {
			return position + size / 2;
		}
	}
	public Vector min
	{
		get {
			return position - size / 2;
		}
	}
	public bool Containts (Vector point) {
		return (point.x < max.x && point.y < max.y && point.x >= min.x && point.y >= min.y);
	}
	public bool Containts (Vector point, float radius) {return (position + size / 2 - point).magnitude < radius;
	}
	public static implicit operator RectangleF (Rect r) {
		return new RectangleF (r.position, r.size);
	}
}
public class Input
{
	public static Vector arrows
	{
		get {
			Vector v = Vector.zero;
			inp.Key[] keys = keyboard.GetPressedKeys();
			foreach (var k in keys) {
				switch (k) {
				case inp.Key.A:
					v += Vector.left;
					break;
				case inp.Key.W:
					v += Vector.forward;
					break;
				case inp.Key.S:
					v += Vector.back;
					break;
				case inp.Key.D:
					v += Vector.right;
					break;
				case inp.Key.E:
					v += Vector.up;
					break;
				case inp.Key.Q:
					v += Vector.down;
					break;
				}
			}
			return v.normalized;
		}
	}

	public static Vector arrows_right
	{
		get {
			Vector v = Vector.zero;
			inp.Key[] keys = keyboard.GetPressedKeys();
			foreach (var k in keys) {
				switch (k) {
				case inp.Key.LeftArrow:
					v += Vector.left;
					break;
				case inp.Key.UpArrow:
					v += Vector.forward;
					break;
				case inp.Key.DownArrow:
					v += Vector.back;
					break;
				case inp.Key.RightArrow:
					v += Vector.right;
					break;
				case inp.Key.PageUp:
					v += Vector.up;
					break;
				case inp.Key.PageDown:
					v += Vector.down;
					break;
				}
			}
			return v.normalized;
		}
	}


	public static bool IsKeyPressed (inp.Key key) {
		inp.Key[] input = keyboard.GetPressedKeys ();
		bool has = false;
		foreach (var k in input) {
			if (k == key) {
				has = true;
				break;
			}
		}
		return has;
	}

	public static inp.Device keyboard { get; private set; }
	public static inp.Device mouse { get; private set; }

	public static void SetInputTo (Rendering.RenderForm form) {
		keyboard = new inp.Device (inp.SystemGuid.Keyboard);
		keyboard.SetCooperativeLevel (form, inp.CooperativeLevelFlags.NonExclusive | inp.CooperativeLevelFlags.Background);
		keyboard.Acquire ();
		mouse = new inp.Device (inp.SystemGuid.Mouse);
		mouse.SetCooperativeLevel(form, inp.CooperativeLevelFlags.NonExclusive | inp.CooperativeLevelFlags.Background);
		mouse.Acquire ();
	}
	public static Vector mouseInput
	{
		get {
			inp.MouseState m = mouse.CurrentMouseState;
			return new Vector(m.X, m.Y, m.Z);
		}
	}
}
public class Resources
{
	public static Bitmap Load (string sourceName) {
		return new Bitmap (appPath + sourceName);
	}

	public static Bitmap[] LoadFromFolder (string folderName) {
		string[] names = Directory.GetFiles (appPath + folderName);
		return names.Select ((string n) => new Bitmap (n)).ToArray ();
	}

	public static string appPath
	{
		get {
			string p = Application.StartupPath + "/Resources/";
			return p;
		}
	}
}