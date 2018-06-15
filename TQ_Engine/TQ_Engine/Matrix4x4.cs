using System.Collections;

[System.Serializable]
public class Matrix2x2
{
	private float angleGetter = 0;
	public float angle
	{
		get
		{
			return angleGetter;
		}
		set
		{
			Vector basisX = GetBasis(value);
			Vector basisY = GetBasis(value + 90);
			a = basisX.x;
			b = basisX.y;
			c = basisY.x;
			d = basisY.y;
		}
	}
	public float a = 1;
	public float b = 0;
	public float c = 0;
	public float d = 1;
	public float scale = 1;
	public static Vector GetBasis (float angle) {
		float x = Mathf.Cos (angle * Mathf.Deg2Rad);
		float y = Mathf.Sin (angle * Mathf.Deg2Rad);
		return new Vector (x, y);
	}
	public static Vector operator * (Vector b, Matrix2x2 m) {
		b *= m.scale;
		Vector x = new Vector (m.a, m.b) * b.x;
		Vector y = new Vector (m.c, m.d) * b.y;
		return x + y;
	}
}
[System.Serializable]
public class Matrix4x4
{
	private float a = 0;
	private float b = 0;
	private float c = 0;
	private float d = 0;
	private float e = 0;
	private float f = 0;
	private float g = 0;
	private float h = 0;
	private float i = 0;

	public Matrix4x4 () {
		eulerAngles = Vector.zero;
		position = Vector.zero;
		scale = Vector.one;
		forward = Vector.forward;
		right = Vector.right;
		up = Vector.up;
	}

	public Vector eulerAngles
	{
		get
		{
			return new Vector(eulerAnglesGetter.x % 360, eulerAnglesGetter.y % 360, eulerAnglesGetter.z % 360);
		}
		set
		{
			eulerAnglesGetter = value;
			SetBasis(eulerAnglesGetter);
		}
	}
	private Vector eulerAnglesGetter = Vector.zero;

	public Vector scale { get; set; }
	public Vector position { get; set; }

	public static Vector Cross (Vector a, Vector b) {
		a = a.normalized;
		b = b.normalized;
		return new Vector (a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x).normalized;
	}

	public Vector forward
	{
		get
		{
			return new Vector(a, d, g).normalized;
		}
		set
		{
			Vector v = value.normalized;
			a = v.x;
			d = v.y;
			g = v.z;
		}
	}
	public Vector up
	{
		get
		{
			return new Vector(b, e, h).normalized;
		}
		set
		{
			Vector v = value.normalized;
			b = v.x;
			e = v.y;
			h = v.z;
		}
	}
	public Vector right
	{
		get
		{
			return new Vector(c, f, i).normalized;
		}
		set
		{
			Vector v = value.normalized;
			c = v.x;
			f = v.y;
			i = v.z;
		}
	}

	public void SetBasis (Vector euler) {
		Matrix2x2 main = new Matrix2x2 ();
		main.angle = euler.y;
		float zUp = Mathf.Sin (euler.x * Mathf.Deg2Rad);
		float zCos = Mathf.Cos (euler.x * Mathf.Deg2Rad);
		Vector u = (Vector.up * main).normalized * zCos;
		Vector zAxis = new Vector (-u.x, -zUp, u.y).normalized;
		float xUp = Mathf.Sin (euler.z * Mathf.Deg2Rad);
		float xCos = Mathf.Cos (euler.z * Mathf.Deg2Rad);
		main.angle = euler.y + 90;
		u = (Vector.up * main).normalized * xCos;
		Vector xAxis = new Vector (-u.x, xUp, u.y).normalized;
		Vector yAxis = -Cross (xAxis, zAxis).normalized;
		xAxis = -Cross (zAxis, yAxis);
		forward = zAxis;
		up = yAxis;
		right = xAxis;
	}
	public static Vector operator * (Vector b, Matrix4x4 m) {
		b = new Vector (b.x * m.scale.x, b.y * m.scale.y, b.z * m.scale.z);
		Vector x = m.right * b.x;
		Vector y = m.up * b.y;
		Vector z = m.forward * b.z;
		return x + y + z + m.position;
	}
	public static Vector operator / (Vector v, Matrix4x4 m) {
		v -= m.position;
		Vector r = new Vector (v.x * m.right.x + v.y * m.right.y + v.z * m.right.z,
		                         v.x * m.up.x + v.y * m.up.y + v.z * m.up.z,
		                         v.x * m.forward.x + v.y * m.forward.y + v.z * m.forward.z);
		r = new Vector (r.x * m.scale.x, r.y * m.scale.y, r.z * m.scale.z);
		return r;
	}
}











