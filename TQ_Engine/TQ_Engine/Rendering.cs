using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Collections.Generic;
using System.Linq;
using d3d = Microsoft.DirectX.Direct3D;
using inp = Microsoft.DirectX.DirectInput;


namespace Rendering
{
	public class RenderForm : Form
    {

		public static List<Renderer> renderers { get; private set; }
		public static Camera camera { get; private set; }
		public static d3d.Font text = null;
        private Device device = null;

        public RenderForm()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);
            device = RenderDrawing.InitializeGraphics(this);

			renderers = new List<Renderer> ();

			int w = Screen.FromControl (this).Bounds.Width;
			int h = Screen.FromControl (this).Bounds.Height;
			SetBounds (0, 0, w, h);
			WindowState = FormWindowState.Maximized;


			Renderer r = new Renderer (new Matrix4x4(Vector.zero, Vector.zero, Vector.one), Models.ModelImporter.FromFile("Elf"));

			RenderForm.camera = new Camera (1000, -Vector.forward * 3 + Vector.up * 1.5f, Vector.forward, Vector.up);

			text = new d3d.Font (device, this.Font);

            device.DeviceReset += new EventHandler(OnReset);
            OnReset(this, new EventArgs());
			Input.SetInputTo (this);
        }

        private void OnReset(object sender, EventArgs e)
        {
            RenderDrawing.SetupCamera();
            RenderDrawing.SetupLights();
        }

		public static void CameraControl () {
			Vector i = Input.mouseInput * 0.1f;
			camera.localEulerAngles += new Vector (i.y, i.x, 0);
			camera.MoveAt (Input.arrows);
			UserControl ();
			RenderDrawing.SetupCamera ();
		}

		public static void UserControl () {
			Renderer casting = castedRenderer;
			if (casting != null) {
				Vector v = Input.arrows_right;
				if (Input.IsKeyPressed(inp.Key.R)) {
					casting.matrix.localEulerAngles += new Vector(v.z, v.x, v.y);
				}
				if (Input.IsKeyPressed(inp.Key.T)) {
					casting.matrix.globalPosition += v > camera;
				}
			}
		}

		private static Renderer castedRenderer
		{
			get {
				return renderers.FirstOrDefault ((Renderer rend) => rend.model.mesh.Intersect(camera.globalPosition - rend.matrix.globalPosition, camera.forward));
			}
		}

		private static void DrawingScene () {
			Renderer casted = castedRenderer;
			string add = casted != null ? casted.matrix.ToString() : "None";
			text.DrawText (null, "Casted : " + add + '\n' +  "Camera : " + camera.ToString(), new Point (10, 10), Color.White);
			foreach (var rend in renderers) {
				RenderDrawing.DrawModel(rend);
			}
		}

		protected override void OnPaint (PaintEventArgs e)
		{
			CameraControl ();
			base.OnPaint (e);
			device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.CornflowerBlue, 1.0f, 0);

			device.BeginScene();

			DrawingScene ();

			device.EndScene();

			device.Present();
			this.Invalidate();
		}

	}
	public class RenderDrawing
    {
		public static Device device = null;
		private static Control form = null; // ссылка на форму, в которой рисуем


        public static Device InitializeGraphics(Control frm)
        {
			
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.AutoDepthStencilFormat = DepthFormat.D24X8;// моя GMAx3100 не поддерживает 32 битную глубину (((
            presentParams.EnableAutoDepthStencil = true;


            device = new Device(0, DeviceType.Hardware, frm, 
                CreateFlags.HardwareVertexProcessing | CreateFlags.PureDevice, presentParams);
            form = frm;

            return device;
        }

		public static void DrawModel(Rendering.Renderer rend)
		{
			Rendering.Model mtm = rend.model;

			Vector euler = rend.matrix.globalEulerAngles;
			Quaternion q = Quaternion.RotationYawPitchRoll (euler.y * Mathf.Deg2Rad, euler.x * Mathf.Deg2Rad, euler.z * Mathf.Deg2Rad);

			Matrix matrix = new Matrix ();
			matrix.AffineTransformation (rend.matrix.scale.y, Vector.zero, q, rend.matrix.globalPosition); 

			for (int i = 0; i < mtm.materials.Length; i++)
			{
				device.SetTransform(TransformType.World, matrix);
				device.Material = mtm.materials[i].material;
				device.SetTexture(0, mtm.materials[i].texture);
				mtm.mesh.DrawSubset(i);
			}
		}

        public static void SetupCamera()
        {
            float w = form.ClientRectangle.Width, h = form.ClientRectangle.Height;
			device.Transform.Projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, w / h, 0.01f, 1000f);
			Camera cam = RenderForm.camera;
			device.Transform.View = Matrix.LookAtLH(cam.globalPosition, cam.globalPosition + cam.forward, cam.up);
            device.RenderState.Ambient = Color.White;
        }

        public static Vector3 lightDirection = new Vector3(0, 0, 1);

        public static void SetupLights()
        {
			device.RenderState.Lighting = true;    // включаем освещение
			device.Lights[0].Type = LightType.Directional; // направленный
            device.Lights[0].Direction = lightDirection; 
			device.Lights[0].Diffuse = System.Drawing.Color.White;
            device.Lights[0].Range = 1000.0f;
            device.Lights[0].Enabled = true;
        }
    }
}