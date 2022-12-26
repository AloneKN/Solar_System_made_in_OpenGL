using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MyGame
{
    public class Outdoor : IDisposable
    {
        private ShaderProgram shader;
        private TextureProgram texture; 
        public Outdoor(string fileTexture)
        {
            shader = new ShaderProgram("Samplers/Outdoor/shader.vert", "Samplers/Outdoor/shader.frag");
            texture = TextureProgram.Load(fileTexture);
        }
        public void RenderFrame(Matrix4 model, Color4 color)
        {

            shader.Use();
            texture.Use(TextureUnit.Texture0);
            shader.SetUniform("imagem", 0);

            shader.SetUniform("CameraRight", Camera.Right);
            shader.SetUniform("CameraUp", Camera.Up);

            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);
            shader.SetUniform("model", model);

            shader.SetUniform("color", color);

            GL.Enable(EnableCap.Blend);
            Quad.RenderQuad();
            GL.Disable(EnableCap.Blend);
        }
        public void Dispose()
        {

            shader.Dispose();
            texture.Dispose();
        }
    }
}