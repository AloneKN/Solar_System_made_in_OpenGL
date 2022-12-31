using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace MyGame
{
    public class Particles : IDisposable
    {
        private ShaderProgram shader;
        private TextureProgram texture;
        private VertexArrayObject Vao;
        private BufferObject<float> vbo;
        private BufferObject<Matrix4> vboMatrices;
        private int amount = 30000;
        public unsafe Particles(string fileTexture)
        {
            shader = new ShaderProgram("Samplers/Particles/shader.vert", "Samplers/Particles/shader.frag");
            texture = new TextureProgram(fileTexture);

            float[] vertices = 
            {
                // positions        // texture Coords
                -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,
                -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f,  1.0f, 0.0f, 1.0f, 1.0f,
                 1.0f, -1.0f, 0.0f, 1.0f, 0.0f,       
            };

            var rand = new System.Random();

            var modelMatrices = new List<Matrix4>();
            for(int i = 0; i < amount; i++)
            {
                var position = new Vector3()
                {
                    X = rand.Next( -5000, 5000),
                    Y = rand.Next( -120, 400),
                    Z = rand.Next( -5000, 5000),
                };
                
                float scale = (float)(0.5f + rand.NextDouble() * 2.0);
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(scale);
                model = model * Matrix4.CreateTranslation(position);
                modelMatrices.Add(model);
            }


            Vao = new VertexArrayObject();
            vbo = new BufferObject<float>(vertices, BufferTarget.ArrayBuffer);
            Vao.LinkBufferObject(ref vbo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 5 * sizeof(float), 0 * sizeof(float));
            Vao.VertexAttributePointer(1, 2, VertexAttribPointerType.Float, 5 * sizeof(float), 3 * sizeof(float));

            vboMatrices = new BufferObject<Matrix4>(TransposeMatrix(modelMatrices), BufferTarget.ArrayBuffer);
            Vao.LinkBufferObject(ref vboMatrices);

            for(int i = 0; i < 4; i++)
            {
                int index = 3 + i;
                Vao.VertexAttributePointer((uint)index, 4, VertexAttribPointerType.Float, sizeof(Matrix4), i * sizeof(Vector4));
                GL.VertexAttribDivisor(index, 1);
            }

        }
        private Vector3 position = Vector3.One;
        private float vel = 0.01f;
        public void RenderFrame(System.Numerics.Vector4 color)
        {

            shader.Use();
            shader.SetUniform("imagem", texture.Use);

            shader.SetUniform("CameraRight", Camera.Right);
            shader.SetUniform("CameraUp", Camera.Up);

            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("color", color);

            position.X += vel * 0.9f;
            position.Y += vel * 0.2f;
            position.Z += vel * 0.7f;

            if(position.X > 50f || position.X < -50f)
            {
                vel = vel * -1f;
            }

            var model = Matrix4.Identity;
            model = model * Matrix4.CreateTranslation(position);
            shader.SetUniform("model", model);

            GL.Enable(EnableCap.Blend);
            Vao.Bind();
            GL.DrawArraysInstanced(PrimitiveType.TriangleStrip, 0, 4, amount);
            GL.Disable(EnableCap.Blend);
        }
        public void Dispose()
        {

            shader.Dispose();
            texture.Dispose();
        }
        private Matrix4[] TransposeMatrix(List<Matrix4> inputModel)
        {
            var outputModel = new Matrix4[inputModel.Count];

            for(int i = 0; i < inputModel.Count; i++)
            {
                outputModel[i] = new Matrix4(inputModel[i].Column0, inputModel[i].Column1, inputModel[i].Column2, inputModel[i].Column3);
            }
            return outputModel;
        }
    }
}