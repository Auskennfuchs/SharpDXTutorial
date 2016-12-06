using System;
using SharpDX.D3DCompiler;
using ConstantBuffer = SharpDXTutorial.Resources.ConstantBuffer;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDXTutorial.Shader {
    abstract class BaseShader : IDisposable{
        public ShaderBytecode ByteCode {
            get; private set;
        }
        public ConstantBuffer[] ConstantBuffers {
            get; private set;
        }
        private Buffer[] cBuffers;

        public int ID {
            get; private set;
        }

        private int g_ShaderId = 0;

        public BaseShader(ShaderBytecode byteCode, ConstantBuffer[] constantBuffers) {
            ByteCode = byteCode;
            ConstantBuffers = constantBuffers;
            cBuffers = new Buffer[ConstantBuffers.Length];
            var i = 0;
            foreach(var cb in ConstantBuffers) {
                cBuffers[i++] = cb.Buffer;
            }
            ID = g_ShaderId;
            System.Threading.Interlocked.Increment(ref g_ShaderId);
        }

        public void Dispose() {
            OnDispose();
            foreach(var cb in ConstantBuffers) {
                cb.Dispose();
            }
        }

        public Buffer[] GetConstantBuffers() {
            return cBuffers;
        }

        protected abstract void OnDispose();
    }
}
