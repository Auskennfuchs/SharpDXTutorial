using SharpDX.Direct3D11;
using D3DVertexShader = SharpDX.Direct3D11.VertexShader;
using D3DPixelShader = SharpDX.Direct3D11.PixelShader;
using VertexShader = SharpDXTutorial.Shader.VertexShader;
using PixelShader = SharpDXTutorial.Shader.PixelShader;
using Buffer = SharpDXTutorial.Resources.Buffer;
using SharpDX;
using SharpDXTutorial.Shader;
using SharpDXTutorial.Resources;

namespace SharpDXTutorial {
    class RenderPipeline {
        public DeviceContext Context {
            get;
        }

        public VertexShader VertexShader {
            get; set;
        }
        private D3DVertexShader currentVertexShader;

        public PixelShader PixelShader {
            get; set;
        }
        private D3DPixelShader currentPixelShader;   

        public ParameterManager Parameters {
            get; private set;
        }

        private Buffer[] vBuffers = new Buffer[SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount];

        private InputAssemblerStage inputAssemblerStage = new InputAssemblerStage();

        public RenderPipeline(DeviceContext context) {
            Context = context;
            Parameters = new ParameterManager();
        }

        public void ClearRenderTarget(RenderTargetView target, Color4 col) {
            Context.ClearRenderTargetView(target, col);
        }

        public void Draw(int vertexCount, int startVertexLocation) {
            UpdateStages();
            Context.Draw(vertexCount, startVertexLocation);
        }

        public void SetVertexBuffer(int slot, Resources.Buffer buf) {
            if(buf==null) {
                vBuffers[slot] = null;
                inputAssemblerStage.DesiredState.VertexBuffers.SetState(slot, null);
                inputAssemblerStage.DesiredState.VertexBufferOffsets.SetState(slot, 0);
                inputAssemblerStage.DesiredState.VertexBufferStrides.SetState(slot, 0);
            } else {
                inputAssemblerStage.DesiredState.VertexBuffers.SetState(slot, buf.D3DBuffer);
                //                desiredInputAssemblerState.VertexBufferOffsets.SetState(slot, bu);
                inputAssemblerStage.DesiredState.VertexBufferStrides.SetState(slot, buf.ElementSize);
                if (buf != vBuffers[slot]) {
                    vBuffers[slot] = buf;
                }
            }
        }

        private void UpdateStages() {
            if (VertexShader == null && currentVertexShader != null) {
                currentVertexShader = null;
                Context.VertexShader.Set(null);
            } else {
                if (currentVertexShader != VertexShader.Shader) {
                    currentVertexShader = VertexShader.Shader;
                    Context.VertexShader.Set(currentVertexShader);
                    Context.VertexShader.SetConstantBuffers(0,VertexShader.GetConstantBuffers());
                }
                foreach (var cb in VertexShader.ConstantBuffers) {
                    cb.UpdateBuffer(Context, Parameters);
                }

                inputAssemblerStage.DesiredState.Topology.SetState(SharpDX.Direct3D.PrimitiveTopology.TriangleList);

                inputAssemblerStage.ApplyDesiredState(Context);
                VertexElementDescription vDesc = null;
                foreach(var vb in vBuffers) {
                    if(vb!=null) {
                        vDesc = vb.Description;
                        break;
                    }
                }
                Context.InputAssembler.InputLayout = InputLayoutManager.Instance.GetInputLayout(vDesc, VertexShader);
            }

            if (PixelShader == null && currentPixelShader != null) {
                currentPixelShader = null;
                Context.PixelShader.Set(null);
            } else {
                if (currentPixelShader != PixelShader.Shader) {
                    currentPixelShader = PixelShader.Shader;
                    Context.PixelShader.Set(currentPixelShader);
                    Context.PixelShader.SetConstantBuffers(0, PixelShader.GetConstantBuffers());
                }
                foreach (var cb in PixelShader.ConstantBuffers) {
                    cb.UpdateBuffer(Context, Parameters);
                }
            }
        }
    }
}
