using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buf = SharpDX.Direct3D11.Buffer;

namespace SharpDXTutorial.Resources {
    class Buffer : IDisposable {
        public VertexElementDescription Description {
            get; private set;
        }

        public Buf D3DBuffer {
            get; private set;
        }

        public int ElementSize {
            get; private set;
        }

        public Buffer(Buf buffer, int elementSize) {
            D3DBuffer = buffer;
            Description = new VertexElementDescription();
            if (buffer != null) {
                ElementSize = elementSize;
            }
        }

        public void AddDescription(string name,Format format, InputClassification usage = InputClassification.PerVertexData, int offset=0) {
            var inputElement = new InputElement {
                SemanticName = name,
                Classification = usage,
                Format = format
            };
            inputElement.SemanticIndex = FindSemanticCount(name);
            if(offset!=0) {
                inputElement.AlignedByteOffset = offset;
            } else {
                inputElement.AlignedByteOffset = Description.Elements.Length==0 ? 0 : InputElement.AppendAligned;
            }
            Description.AddDescription(inputElement);
        }

        private int FindSemanticCount(string name) {
            int count = 0;
            foreach(var d in Description.Elements) {
                if(d.SemanticName.Equals(name)) {
                    count++;
                }
            }
            return count;
        }

        public void Dispose() {
            if(D3DBuffer!=null) {
                D3DBuffer.Dispose();
            }
        }
    }
}
