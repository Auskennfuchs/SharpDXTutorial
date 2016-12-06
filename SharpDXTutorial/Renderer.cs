using System;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using Resource = SharpDX.Direct3D11.Resource;
using SharpDX.Windows;
using SharpDXTutorial.Shader;

namespace SharpDXTutorial {
    class Renderer : IDisposable{

        public Device Device {
            get; private set;
        }

        public DeviceContext ImmContext {
            get; private set;
        }

        public RenderPipeline ImmPipeline {
            get; private set;
        }

        public ShaderLoader Shader {
            get; private set;
        }

        private SwapChain dxSwapChain;

        private RenderForm form;

        public RenderTargetView MainTarget {
            get; private set;
        }

        private InputLayoutManager inputLayoutManager;

        public static Renderer Instance {
            get; private set;
        }

        public Renderer() {
            Instance = this;
        }

        public void Init(RenderForm window) {
            form = window;
            Device = new Device(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.Debug);
            ImmContext = Device.ImmediateContext;

            ImmPipeline = new RenderPipeline(ImmContext);

            var width = form.ClientSize.Width;
            var height = form.ClientSize.Height;
            using(var factory = new Factory1()) {
                var swapChainDesciptor = new SwapChainDescription {
                    BufferCount = 1,
                    Flags = SwapChainFlags.None,
                    IsWindowed = true,
                    ModeDescription = new ModeDescription(width, height, new Rational(0, 1), Format.R8G8B8A8_UNorm),
                    OutputHandle = form.Handle,
                    SampleDescription = new SampleDescription(1, 0),
                    SwapEffect = SwapEffect.Discard,
                    Usage = Usage.RenderTargetOutput                  
                };
                dxSwapChain = new SwapChain(factory, Device, swapChainDesciptor);
                Resize(width, height);

                factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAltEnter);
            }

            Shader = new ShaderLoader(this);

            inputLayoutManager = new InputLayoutManager(Device);
        }

        public void Dispose() {
            if(inputLayoutManager!=null) {
                inputLayoutManager.Dispose();
            }
            if (MainTarget != null) {
                MainTarget.Dispose();
            }
            if(dxSwapChain!=null) {
                dxSwapChain.Dispose();
            }
            if(Device != null) {
                Device.Dispose();
            }
        }

        public void ClearRenderTarget(RenderTargetView target, Color4 col) {
            ImmContext.ClearRenderTargetView(target, col);
        }

        public void Present() {
            dxSwapChain.Present(0, PresentFlags.None);
        }

        public void Run() {
            form.InitScene();
            RenderLoop.Run(form,form.RenderLoop);
            form.ClearResources();
        }

        public void Resize(int newWidth, int newHeight) {
            if(MainTarget!=null) {
                MainTarget.Dispose();
            }
            if (dxSwapChain == null) {
                return;
            }
            dxSwapChain.ResizeBuffers(dxSwapChain.Description.BufferCount, newWidth, newHeight, dxSwapChain.Description.ModeDescription.Format, dxSwapChain.Description.Flags);
            using (var backBuffer = Resource.FromSwapChain<Texture2D>(dxSwapChain, 0)) {
                MainTarget = new RenderTargetView(Device, backBuffer);
                var viewPort = new Viewport(0, 0, newWidth, newHeight);
                ImmContext.Rasterizer.SetViewport(viewPort);
                ImmContext.OutputMerger.SetRenderTargets(MainTarget);
            }
        }

    }
}
