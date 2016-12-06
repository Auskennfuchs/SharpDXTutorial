using SharpDX.D3DCompiler;
using VS = SharpDX.Direct3D11.VertexShader;
using ConstantBuffer = SharpDXTutorial.Resources.ConstantBuffer;
using SharpDX.Direct3D11;

namespace SharpDXTutorial.Shader {
    class VertexShader : BaseShader{
        public VS Shader {
            get; private set;
        }

        public VertexShader(VS vs, ShaderBytecode byteCode, ConstantBuffer[] constantBuffers) : base(byteCode,constantBuffers) {
            Shader = vs;
        }

        protected override void OnDispose() {
            if (Shader != null) {
                Shader.Dispose();
            }
        }

        internal InputLayout CreateInputLayout(Device dev, InputElement[] inputElements) {
            return new InputLayout(dev, ByteCode, inputElements);
        }
    }
}
