
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Sun : IDisposable
    {
        private ShaderProgram Shader;
        private TextureProgram DiffuseMap;
        private string solarMaps = "Resources/solarSystem/";
        public Sun()
        {
            Shader = new ShaderProgram("Samplers/Sun/shader.vert", "Samplers/Sun/shader.frag");
            DiffuseMap = TextureProgram.Load(solarMaps + "sol.jpg", PixelInternalFormat.SrgbAlpha);
        }
        public void RenderFrame(Matrix4 model)
        {
            
            Shader.Use();
            Shader.SetUniform("model", model);
            Shader.SetUniform("view", Camera.ViewMatrix);
            Shader.SetUniform("projection", Camera.ProjectionMatrix);

            Shader.SetUniform("Timer", Clock.Time);

            Shader.SetUniform("velWaves", Values.VelWaves);

            Shader.SetUniform("DiffuseMap", DiffuseMap.Use());

            SphereAssimp.RenderSphere();
            
        }
        public void UpdateFrame()
        {
            var input = Program.window.IsKeyDown;

        }
        public void Dispose()
        {
            Shader.Dispose();
        }
    }
}