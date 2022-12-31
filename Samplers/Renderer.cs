using OpenTK.Mathematics;

namespace MyGame
{
    interface Renderer : IDisposable
    {
        void RenderFrame(Matrix4 model);
        new void Dispose();
    }
}