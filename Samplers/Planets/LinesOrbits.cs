using OpenTK.Mathematics;

namespace MyGame
{
    public class LinesOrbits
    {
        private ShaderProgram shader;
        public Matrix4 model;
        
        public LinesOrbits()
        {
            shader = new ShaderProgram("Samplers/Planets/shader.vert", "Samplers/Planets/shaderLines.frag");
        }
        public void RenderFrame(Vector3 pos)
        {
            model = Matrix4.Identity;
            model = model * Matrix4.CreateScale(pos.X);

            shader.Use();
            shader.SetUniform("model", model);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("color", Values.LinesColor);

            Arco.RenderArco();

        }
    }
}