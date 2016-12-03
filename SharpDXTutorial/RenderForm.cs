using System.Windows.Forms;

namespace SharpDXTutorial {
    abstract class RenderForm : Form{

        public Renderer Renderer {
            get; private set;
        }

        public RenderForm(Renderer renderer) {
            Renderer = renderer;
        }

        public abstract void RenderLoop();

        public abstract void ClearResources();

        public virtual void InitScene() { }
    }
}
