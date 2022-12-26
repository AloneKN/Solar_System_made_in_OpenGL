using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    public struct Arco : IDisposable
    {
        private static VertexArrayObject ?Vao;
        private static BufferObject<float> ?Vbo;
        private static int count;
        public static void RenderArco()
        {
            if(Vao == null)
            {
                List<float> vertices = new List<float>();

                for(int i = 0; i < 1100; i++)
                {
                    double resul = Math.Cos(2 * 3.14159 * i / 1000.0);
                    double resul1 = Math.Sin(2 * 3.14159 * i / 1000.0);

                    vertices.Add((float)resul1);
                    vertices.Add(0.0f);
                    vertices.Add((float)resul);
                }
                count = vertices.Count / 3;

                Vao = new VertexArrayObject();
                Vbo = new BufferObject<float>(vertices.ToArray(), BufferTarget.ArrayBuffer);

                Vao.BindBuffer(ref Vbo);
                Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, 3, 0);

            }

            Vao.Bind();
            switch(Values.PrimitiveType)
            {
                case 1:
                    GL.DrawArrays(PrimitiveType.LineStrip, 0, count);
                    break;
                case 2:
                    GL.DrawArrays(PrimitiveType.Lines, 0, count);
                    break;
                case 3:
                    GL.DrawArrays(PrimitiveType.Points, 0, count);
                    break;
                default:
                    break;
            }
        }
        public void Dispose()
        {
            Vao!.Dispose();
            Vbo!.Dispose();
        }
    }
}