using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDXTutorial.Resources;
using VertexShader = SharpDXTutorial.Shader.VertexShader;

namespace SharpDXTutorial.Shader {
    class InputLayoutManager : IDisposable{

        private Dictionary<int, Dictionary<int, InputLayout>> inputLayouts;

        Device device;

        public static InputLayoutManager Instance {
            get; private set;
        }

        public InputLayoutManager(Device dev) {
            device = dev;
            inputLayouts = new Dictionary<int, Dictionary<int, InputLayout>>();
            Instance = this;
        }

        public InputLayout GetInputLayout(VertexElementDescription bufDesc, VertexShader vShader) {
            if (vShader == null) {
                return null;
            }
            if(inputLayouts.ContainsKey(vShader.ID)) {
                if(inputLayouts[vShader.ID].ContainsKey(bufDesc.HashCode)) {
                    return inputLayouts[vShader.ID][bufDesc.HashCode];
                } 
            } else {
                inputLayouts.Add(vShader.ID, new Dictionary<int, InputLayout>());
            }

            var inputLayout = vShader.CreateInputLayout(device, bufDesc.Elements);
            inputLayouts[vShader.ID].Add(bufDesc.HashCode, inputLayout);

            return inputLayout;
        }

        public void Dispose() {
            foreach(var t in inputLayouts) {
                foreach(var ip in t.Value) {
                    ip.Value.Dispose();
                }
            }
            inputLayouts.Clear();
        }
    }
}
