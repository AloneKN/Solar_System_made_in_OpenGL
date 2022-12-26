using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyGame
{
    public class OuterPlanets : IDisposable
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap;
        public OuterPlanets(string pathTexture)
        {
            shader = new ShaderProgram("Samplers/Planets/shader.vert", "Samplers/Planets/shader.frag");
            DiffuseMap = TextureProgram.Load(pathTexture);

        }
        public void RenderFrame(Matrix4 model)
        {

            shader.Use();
            shader.SetUniform("model", model);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("viewPos", Camera.Position);
            shader.SetUniform("lightPos", SolarSystemValues.Luz);
            shader.SetUniform("light.Specular", Values.LightSpecular);
            shader.SetUniform("light.Shininess", Values.LightShininess);
            shader.SetUniform("light.Ambient", Values.LightAmbiente);
            shader.SetUniform("light.Diffuse", Values.LightDiffuse);


            DiffuseMap.Use();
            shader.SetUniform("DiffuseMap", 0);

            SphereAssimp.RenderSphere();
        }
        public void Dispose()
        {
            DiffuseMap.Dispose();
            shader.Dispose();
        }
    }
}