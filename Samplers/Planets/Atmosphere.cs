using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyGame
{
    public class Atmosphere
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap;
        public Matrix4 model;
        
        public Atmosphere(string pathTexture)
        {
            shader = new ShaderProgram("Samplers/Planets/shader.vert", "Samplers/Planets/shaderAtmosphere.frag");
            DiffuseMap = new TextureProgram(pathTexture);
        }
        public void RenderFrame(Matrix4 model, float alpha)
        {

            shader.Use();
            shader.SetUniform("model", model);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);
            shader.SetUniform("Timer", Clock.Time);
            shader.SetUniform("alpha", alpha);

            shader.SetUniform("DiffuseMap", DiffuseMap.Use);

            // como queremos configurar o componente alpha da nossa textura devemos ativar o blend
            GL.Enable(EnableCap.Blend);
            SphereAssimp.RenderSphere();
            GL.Disable(EnableCap.Blend);

        }
    }
}