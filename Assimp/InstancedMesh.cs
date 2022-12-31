using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace MyGame
{
    public class InstancedMeshe : IDisposable
    {
        private int indicesCount;
        private int Amount;
        public VertexArrayObject Vao { get; private set; }
        private BufferObject<Vertex> Vbo;
        private BufferObject<ushort> Ebo;
        private BufferObject<Matrix4>? VboMatrices;
        public unsafe InstancedMeshe(List<Vertex> Vertices, List<ushort> Indices, List<Matrix4> matricesModels)
        {
            indicesCount = Indices.Count;
            Amount = matricesModels.Count;

            Vao = new VertexArrayObject();

            Ebo = new BufferObject<ushort>(Indices.ToArray(), BufferTarget.ElementArrayBuffer);
            Vao.LinkBufferObject(ref Ebo);

            Vbo = new BufferObject<Vertex>(Vertices.ToArray(), BufferTarget.ArrayBuffer);
            Vao.LinkBufferObject(ref Vbo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, sizeof(Vertex), IntPtr.Zero);
            Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), "Normals"));
            Vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), "TexCoords"));

            VboMatrices = new BufferObject<Matrix4>(TransposeMatrix(matricesModels), BufferTarget.ArrayBuffer);

            Vao.LinkBufferObject(ref VboMatrices);

            for(int i = 0; i < 4; i++)
            {
                int index = 4 + i;
                Vao.VertexAttributePointer((uint)index, 4, VertexAttribPointerType.Float, sizeof(Matrix4), i * sizeof(Vector4));
                GL.VertexAttribDivisor(index, 1);
            }

            Vertices.Clear();
            Indices.Clear();
            matricesModels.Clear();

        }
        public void RenderFrame()
        {
            Vao.Bind();
            GL.DrawElementsInstanced(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedShort, IntPtr.Zero, Amount);
        }
        public void Dispose()
        {
            Vao.Dispose();
            Vbo.Dispose();
            Ebo.Dispose();
            VboMatrices!.Dispose();
            
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