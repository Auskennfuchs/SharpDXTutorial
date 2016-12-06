namespace SharpDXTutorial {
    class InputAssemblerState {
        public StateMonitor<SharpDX.Direct3D11.Buffer> IndexBuffer {
            get; private set;
        }
        public StateMonitor<SharpDX.DXGI.Format> IndexBufferFormat {
            get; private set;
        }
        public StateArrayMonitor<SharpDX.Direct3D11.Buffer> VertexBuffers {
            get; private set;
        }
        public StateArrayMonitor<int> VertexBufferStrides {
            get; private set;
        }
        public StateArrayMonitor<int> VertexBufferOffsets {
            get; private set;
        }

        public StateMonitor<SharpDX.Direct3D.PrimitiveTopology> Topology {
            get; private set;
        }

        public InputAssemblerState() {
            IndexBuffer = new StateMonitor<SharpDX.Direct3D11.Buffer>(null);
            IndexBufferFormat = new StateMonitor<SharpDX.DXGI.Format>(SharpDX.DXGI.Format.Unknown);
            VertexBuffers = new StateArrayMonitor<SharpDX.Direct3D11.Buffer>(null, SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount);
            VertexBufferStrides = new StateArrayMonitor<int>(0, SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount);
            VertexBufferOffsets = new StateArrayMonitor<int>(0, SharpDX.Direct3D11.InputAssemblerStage.VertexInputResourceSlotCount);
            Topology = new StateMonitor<SharpDX.Direct3D.PrimitiveTopology>(SharpDX.Direct3D.PrimitiveTopology.Undefined);
        }

        public void SetSisters(InputAssemblerState sister) {
            IndexBuffer.Sister = sister.IndexBuffer;
            IndexBufferFormat.Sister = sister.IndexBufferFormat;
            VertexBuffers.Sister = sister.VertexBuffers;
            VertexBufferStrides.Sister = sister.VertexBufferStrides;
            VertexBufferOffsets.Sister = sister.VertexBufferOffsets;
            Topology.Sister = sister.Topology;
        }

        public void SetStatesUntracked(InputAssemblerState state) {
            IndexBuffer.ApplyValue(state.IndexBuffer);
            IndexBufferFormat.ApplyValue(state.IndexBufferFormat);
            VertexBuffers.ApplyValues(state.VertexBuffers);
            VertexBufferStrides.ApplyValues(state.VertexBufferStrides);
            VertexBufferOffsets.ApplyValues(state.VertexBufferOffsets);
            Topology.ApplyValue(state.Topology);
        }

        public void ResetTracking() {
            IndexBuffer.ResetTracking();
            IndexBufferFormat.ResetTracking();
            VertexBuffers.ResetTracking();
            VertexBufferStrides.ResetTracking();
            VertexBufferOffsets.ResetTracking();
            Topology.ResetTracking();
        }
    }
}
