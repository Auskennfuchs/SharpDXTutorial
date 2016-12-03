using SharpDX;
using Buffer = SharpDX.Direct3D11.Buffer;
using SharpDX.Direct3D11;
using SharpDX.D3DCompiler;
using System;
using System.Windows.Forms;

namespace SharpDXTutorial {
    class TutorialForm : RenderForm {

        private Buffer vertexBuffer;

        private VertexShader vs;
        private PixelShader ps;
        private InputLayout inputLayout;

        public TutorialForm(Renderer renderer) : base(renderer) {
            this.ClientSizeChanged += TutorialForm_ClientSizeChanged;
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

            vertexBuffer = Buffer.Create(Renderer.Device, vertices, new BufferDescription {
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags= CpuAccessFlags.None,
                OptionFlags= ResourceOptionFlags.None,
                SizeInBytes=vertices.Length*Vector3.SizeInBytes,
                StructureByteStride=Vector3.SizeInBytes,
                Usage= ResourceUsage.Default
            });

            LoadShaders();
        }

        private void LoadShaders() {
            try {
                using (var shaderByteCode = ShaderBytecode.CompileFromFile("shaders/simple.hlsl", "mainVS", "vs_5_0", ShaderFlags.Debug)) {
                    vs = new VertexShader(Renderer.Device, shaderByteCode);
                    inputLayout = new InputLayout(Renderer.Device, shaderByteCode, new InputElement[] {
                        new InputElement("POSITION",0,SharpDX.DXGI.Format.R32G32B32_Float,0)
                    });
                    Renderer.ImmContext.InputAssembler.InputLayout = inputLayout;
                }
                using (var shaderByteCode = ShaderBytecode.CompileFromFile("shaders/simple.hlsl", "mainPS", "ps_5_0", ShaderFlags.Debug)) {
                    ps = new PixelShader(Renderer.Device, shaderByteCode);
                }
            }catch(Exception ex) {
                MessageBox.Show(ex.Message);
                Environment.Exit(-2);
            }

            Renderer.ImmContext.VertexShader.Set(vs);
            Renderer.ImmContext.PixelShader.Set(ps);
            Renderer.ImmContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
        }

        public override void RenderLoop() {
            Renderer.ClearRenderTarget(Renderer.Instance.MainTarget, new SharpDX.Color4(0.0f, 0.0f, 1.0f, 1.0f));
            Renderer.ImmContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Vector3.SizeInBytes, 0));
            Renderer.ImmContext.Draw(3, 0);
            Renderer.Present();
        }

        public override void ClearResources() {
            if(inputLayout!=null) {
                inputLayout.Dispose();
            }
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
