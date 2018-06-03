using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System;
using System.IO;
using System.Windows.Forms;
using AnimalsSolution;

public class IncBehaviour
{
	public void Destroy (IncBehaviour behaviour) {
		if (this is Slot) {
			MainClass.screen.Controls.Remove (((Slot)this).picture);
		}
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
	public static implicit operator Vector (SizeF s) {
		return new Vector (s.Width, s.Height);
	}
	public static implicit operator Vector (PointF s) {
		return new Vector (s.X, s.Y);
	}
	public static implicit operator SizeF (Vector v) {
		return new SizeF (v.x, v.y);
	}
	public static implicit operator PointF (Vector v) {
		return new PointF (v.x, v.y);
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
	public static Vector down
	{
		get {
			return new Vector (0, -1);
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
	public Vector normallized
	{
		get {
			return new Vector (x, y, z) / magnitude;
		}
	}
	public override string ToString () {
		return "Vector : [" + x + ", " + y + "," + z + "]";
	}
}
public class RectangleTrans : IncBehaviour
{
	public Rect rect;

	public RectangleTrans (Vector position, Vector size) {
		rect = new Rect (position, size);
		//base.Init ();
	}

	public Vector position
	{
		get {
			return rect.position;
		}
		set {
			rect.position = value;
		}
	}
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
public class Collider : IncBehaviour
{
	public RectangleTrans trans { get; private set; }

	public Collider (RectangleTrans t) {
		trans = t;
		base.Init ();
	}

	public static Collider[] OverlapArea (Vector point) {
		Collider[] colls = FindObjectsOfType<Collider> ();
		return colls.Where ((Collider arg) => arg.trans.rect.Containts (point)).ToArray ();
	}
	public static Collider[] OverlapCircle (Vector point, float radius) {
		Collider[] colls = FindObjectsOfType<Collider> ();
		return colls.Where ((Collider arg) => arg.trans.rect.Containts (point, radius)).ToArray ();
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