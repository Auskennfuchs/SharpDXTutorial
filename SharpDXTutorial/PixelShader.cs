using SharpDX.D3DCompiler;
using PS = SharpDX.Direct3D11.PixelShader;
using ConstantBuffer = SharpDXTutorial.Resources.ConstantBuffer;

namespace SharpDXTutorial.Shader {
    class PixelShader : BaseShader {
        public PS Shader {
            get; private set;
        }

        public PixelShader(PS ps, ShaderBytecode byteCode, ConstantBuffer[] constantBuffers) : base(byteCode,constantBuffers) {
            Shader = ps;
        }

        protected override void OnDispose() {
            if (Shader != null) {
                Shader.Dispose();
            }
        }
    }
}
