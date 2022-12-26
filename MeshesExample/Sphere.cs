using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace MyGame
{
    public class SphereAssimp : IDisposable
    {
        private static Meshe ?meshe;
        public static void RenderSphere()
        {
            if(meshe == null)
            {
                var _meshes = AssimpModel.Load("Resources/Model3D/globos.obj");
                meshe = _meshes[0];
            }
            
            meshe.RenderFrame();
        }
        public void Dispose()
        {
            meshe!.Dispose();
        }
    }
}