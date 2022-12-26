
namespace MyGame
{
    public class Ring : IDisposable
    {
        private static List<Meshe>? meshes;
        public static void RenderRing()
        {
            if(meshes == null)
            {
                meshes = AssimpModel.Load("Resources/Model3D/ring.obj");

                
            }
            foreach(var item in meshes)
                item.RenderFrame();            
        }
        public void Dispose()
        {
            foreach(var item in meshes!)
                item.Dispose();
        }
    }
}