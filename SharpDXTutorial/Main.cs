using System;

namespace SharpDXTutorial {
    public static class MainProgram {

        [STAThread]
        static void Main() {
            var renderer = new Renderer();
            var form = new TutorialForm(renderer);
            form.Text = "SharpDXTutorial";
            form.ClientSize = new System.Drawing.Size(1280, 720);

            renderer.Init(form);
            renderer.Run();
            renderer.Dispose();
        }
    }
}
