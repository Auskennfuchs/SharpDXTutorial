using System;
using SharpDX.D3DCompiler;
using VS = SharpDX.Direct3D11.VertexShader;
using PS = SharpDX.Direct3D11.PixelShader;
using CBuffer = SharpDX.D3DCompiler.ConstantBuffer;
using Buffer = SharpDX.Direct3D11.Buffer;
using System.Collections.Generic;
using SharpDXTutorial.Resources;
using SharpDX.Direct3D11;

namespace SharpDXTutorial.Shader {
    class ShaderLoader {

        private Renderer renderer;

        public ShaderLoader(Renderer renderer) {
            this.renderer = renderer;
        }

        public VertexShader LoadVertexShader(string file, string mainFunction) {
            using (var shaderResult = ShaderBytecode.CompileFromFile(file, mainFunction, "vs_5_0", ShaderFlags.PackMatrixRowMajor|ShaderFlags.Debug)) {
                var vs = new VS(renderer.Device, shaderResult.Bytecode);
                var constantBuffers = ReflectBytecode(shaderResult.Bytecode);
                return new VertexShader(vs, shaderResult.Bytecode, constantBuffers);
            }
        }

        public PixelShader LoadPixelShader(string file, string mainFunction) {
            using (var shaderResult = ShaderBytecode.CompileFromFile(file, mainFunction, "ps_5_0", ShaderFlags.PackMatrixRowMajor|ShaderFlags.Debug)) {
                var ps = new PS(renderer.Device, shaderResult.Bytecode);
                var constantBuffers = ReflectBytecode(shaderResult.Bytecode);
                return new PixelShader(ps, shaderResult.Bytecode,constantBuffers);
            }
        }

        protected Resources.ConstantBuffer[] ReflectBytecode(ShaderBytecode bytecode) {
            List<Resources.ConstantBuffer> constantBuffers = new List<Resources.ConstantBuffer>();
            using (var reflection = new ShaderReflection(bytecode)) {
                for (int cBufferIndex = 0; cBufferIndex < reflection.Description.ConstantBuffers; cBufferIndex++) {
                    CBuffer cb = reflection.GetConstantBuffer(cBufferIndex);
                    Buffer buf = new Buffer(Renderer.Instance.Device, cb.Description.Size, ResourceUsage.Dynamic, BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, sizeof(float));
                    Resources.ConstantBuffer constantBuffer = new Resources.ConstantBuffer(buf);
                    for (int i = 0; i < cb.Description.VariableCount; i++) {
                        var refVar = cb.GetVariable(i);
                        var type = refVar.GetVariableType();
                        switch (type.Description.Type) {
                            case ShaderVariableType.Float:
                                if (type.Description.RowCount == 4 && type.Description.ColumnCount == 4) {
                                    var matParam = new MatrixParameter();
                                    if (matParam.GetSize() != refVar.Description.Size) {
                                        throw new ArgumentOutOfRangeException("Error ConstantBufferParamtersize for "+refVar.Description.Name);
                                    }
                                    constantBuffer.AddParameter(refVar.Description.Name, refVar.Description.StartOffset, matParam);
                                }
                                break;
                        }
                    }
                    constantBuffers.Add(constantBuffer);
                }
            }
            return constantBuffers.ToArray();
        }

    }
}
