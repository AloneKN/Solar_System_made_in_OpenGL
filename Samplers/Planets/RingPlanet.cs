using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyGame
{
    public class RingPlanet
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap;
        public Matrix4 model;
        
        public RingPlanet(string pathTexture)
        {
            shader = new ShaderProgram("Samplers/Planets/shader.vert", "Samplers/Planets/ShaderRing.frag");
            DiffuseMap = TextureProgram.Load(pathTexture);
        }
        public void RenderFrame(Matrix4 model)
        {

            shader.Use();
            shader.SetUniform("model", model);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("DiffuseMap", DiffuseMap.Use());

            // como queremos configurar o componente alpha da nossa textura devemos ativar o blend
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            Ring.RenderRing();
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.CullFace);

        }
    }
}