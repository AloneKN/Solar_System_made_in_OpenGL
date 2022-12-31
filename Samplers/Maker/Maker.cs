using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyGame
{
    public class Maker
    {
        private ShaderProgram shader;
        private TextureProgram texture;
        public Maker(string texturePath)
        {

            shader = new ShaderProgram("Samplers/Maker/maker.vert", "Samplers/Maker/maker.frag");

            texture = new TextureProgram(texturePath);
        }
        public void RenderFrame(Vector3 position, System.Numerics.Vector2 Size, System.Numerics.Vector4 color)
        {
            shader.Use();

            shader.SetUniform("projection", Camera.ProjectionMatrix);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("CameraRight", Camera.Right);
            shader.SetUniform("CameraUp", Camera.Up);

            shader.SetUniform("Position", position);
            shader.SetUniform("Size", Size);

            shader.SetUniform("imagem", texture.Use);
            shader.SetUniform("color", color);


            GL.Enable(EnableCap.Blend);
            Quad.RenderQuad();
            GL.Disable(EnableCap.Blend);
        }
    }
}