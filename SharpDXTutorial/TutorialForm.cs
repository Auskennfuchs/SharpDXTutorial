using SharpDX;
using Buffer = SharpDXTutorial.Resources.Buffer;
using SharpDX.Direct3D11;
using System;
using System.Windows.Forms;
using SharpDXTutorial.Resources;
using System.Diagnostics;

namespace SharpDXTutorial {

    class TutorialForm : RenderForm {

        private Buffer vertexBuffer;

        private Shader.VertexShader vs;
        private Shader.PixelShader ps;

        private Stopwatch timer;

        public TutorialForm(Renderer renderer) : base(renderer) {
            this.ClientSizeChanged += TutorialForm_ClientSizeChanged;
            timer = new Stopwatch();
        }

        private void TutorialForm_ClientSizeChanged(object sender, EventArgs e) {
            Renderer.Resize(ClientSize.Width, ClientSize.Height);
        }

        public override void InitScene() {
            var vertices = new Vector3[] {
                new Vector3(-1.0f,-1.0f,0.0f),
                new Vector3(0.0f,1.0f,0.0f),
                new Vector3(1.0f,-1.0f,0.0f)
            };

            var buf = SharpDX.Direct3D11.Buffer.Create(Renderer.Device, vertices, new SharpDX.Direct3D11.BufferDescription {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags= CpuAccessFlags.None,
                OptionFlags= ResourceOptionFlags.None,
                SizeInBytes=vertices.Length*Vector3.SizeInBytes,
                StructureByteStride=Vector3.SizeInBytes,
                Usage= ResourceUsage.Default
            });

            vertexBuffer = new Buffer(buf,Vector3.SizeInBytes);
            vertexBuffer.AddDescription("POSITION", SharpDX.DXGI.Format.R32G32B32_Float);

            LoadShaders();

            timer.Reset();
        }

        private void LoadShaders() {
            try {
                vs = Renderer.Shader.LoadVertexShader("shaders/simple.hlsl", "mainVS");
                ps = Renderer.Shader.LoadPixelShader("shaders/simple.hlsl", "mainPS");
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(-2);
            }

            Renderer.ImmContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

            Renderer.ImmPipeline.Parameters.SetWorldMatrix(Matrix.Translation(0, 0, 2.0f));
            Renderer.ImmPipeline.Parameters.SetViewMatrix(Matrix.LookAtLH(Vector3.Zero, Vector3.ForwardLH, Vector3.Up));
            Renderer.ImmPipeline.Parameters.SetProjectionMatrix(Matrix.PerspectiveFovLH((float)Math.PI / 2.0f, ClientSize.Width / ClientSize.Height, 0.1f, 1000.0f));
        }

        public override void RenderLoop() {
            timer.Stop();
            float elapsed = (float)timer.ElapsedTicks / (float)Stopwatch.Frequency;
            Renderer.ImmPipeline.VertexShader = vs;
            Renderer.ImmPipeline.PixelShader = ps;
            Renderer.ImmPipeline.ClearRenderTarget(Renderer.Instance.MainTarget, new Color4(0.0f, 0.0f, 1.0f, 1.0f));
            Renderer.ImmPipeline.SetVertexBuffer(0, vertexBuffer);
            Renderer.ImmPipeline.Draw(3, 0);
            Renderer.Present();
            timer.Restart();
        }

        public override void ClearResources() {
            if(vs!=null) {
                vs.Dispose();
            }
            if (ps != null) {
                ps.Dispose();
            }
            if(vertexBuffer!=null) {
                vertexBuffer.Dispose();
            }
        }

    }
}
