using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D11;

namespace SharpDXTutorial {
    class InputAssemblerStage {
        public InputAssemblerState DesiredState {
            get; private set;
        }
        private InputAssemblerState currentState = new InputAssemblerState();

        public InputAssemblerStage() {
            DesiredState = new InputAssemblerState(); 
            DesiredState.SetSisters(currentState);
        }

        public void ApplyDesiredState(DeviceContext context) {
            if (DesiredState.VertexBuffers.NeedRefresh ||
                DesiredState.VertexBufferOffsets.NeedRefresh ||
                DesiredState.VertexBufferStrides.NeedRefresh) {
                var minSlot = Math.Min(Math.Min(DesiredState.VertexBuffers.StartSlot,
                                                DesiredState.VertexBufferOffsets.StartSlot),
                                                DesiredState.VertexBufferStrides.StartSlot);
                var maxSlot = Math.Max(Math.Max(DesiredState.VertexBuffers.EndSlot,
                                                DesiredState.VertexBufferOffsets.EndSlot),
                                                DesiredState.VertexBufferStrides.EndSlot);
                context.InputAssembler.SetVertexBuffers(minSlot,
                    DesiredState.VertexBuffers.GetRange(minSlot, maxSlot),
                    DesiredState.VertexBufferStrides.GetRange(minSlot, maxSlot),
                    DesiredState.VertexBufferOffsets.GetRange(minSlot, maxSlot));
            }

            if(DesiredState.Topology.NeedRefresh) {
                context.InputAssembler.PrimitiveTopology = DesiredState.Topology.State;
            }

            if(DesiredState.IndexBuffer.NeedRefresh) {
                context.InputAssembler.SetIndexBuffer(DesiredState.IndexBuffer.State, DesiredState.IndexBufferFormat.State, 0);
            }

            DesiredState.ResetTracking();
            currentState.SetStatesUntracked(DesiredState);
        }

    }
}
