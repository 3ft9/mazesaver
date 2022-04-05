using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Drawing;

namespace ReactionDiffusionSaver
{
    public partial class ScreenSaverWindow : GameWindow
    {
        private Simulation simulation;

        public ScreenSaverWindow(int width = 1280, int height = 720)
            : base(width, height, new GraphicsMode(new ColorFormat(32), 1, 0, 4, new ColorFormat(32), 2), "Screen Saver")
        {
            simulation = new Simulation(width, height);
        }

        protected override void OnLoad(EventArgs e)
        {
            VSync = VSyncMode.Adaptive;

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);

            GL.Enable(EnableCap.Texture2D);

            GL.ClearColor(Color.Black);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            simulation = new Simulation(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (Focused)
            {
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            simulation.Update();

            GL.GetPixelMap()
            SwapBuffers();
        }

        protected override void OnKeyUp(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Exit();
                    break;

                case Key.Enter:
                    if (e.Modifiers.HasFlag(KeyModifiers.Alt))
                    {
                        if (WindowState != WindowState.Normal)
                        {
                            //DisplayDevice.GetDisplay(DisplayIndex.Default).RestoreResolution();
                            WindowState = WindowState.Normal;
                        }
                        else
                        {
                            //DisplayDevice.GetDisplay(DisplayIndex.Default).ChangeResolution(1280, 720, 32, DisplayDevice.GetDisplay(DisplayIndex.Default).RefreshRate);
                            WindowState = WindowState.Fullscreen;
                        }
                    }
                    break;
            }
        }
    }
}
