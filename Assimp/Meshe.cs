using OpenTK.Graphics.OpenGL4;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace MyGame
{
    public struct Vertex
    {
        public Vector3 Positions;
        public Vector3 Normals;
        public Vector2 TexCoords;
    }
    
    public class Meshe : IDisposable
    {
        private int indicesCount;
        public VertexArrayObject Vao { get; private set; }
        private BufferObject<Vertex> Vbo;
        private BufferObject<ushort> Ebo;
        public unsafe Meshe(List<Vertex> Vertices, List<ushort> Indices)
        {
            indicesCount = Indices.Count;

            Vao = new VertexArrayObject();
            Vbo = new BufferObject<Vertex>(Vertices.ToArray(), BufferTarget.ArrayBuffer);
            Vao.LinkBufferObject(ref Vbo);

            Vao.VertexAttributePointer(0, 3, VertexAttribPointerType.Float, sizeof(Vertex), IntPtr.Zero);
            Vao.VertexAttributePointer(1, 3, VertexAttribPointerType.Float, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), "Normals"));
            Vao.VertexAttributePointer(2, 2, VertexAttribPointerType.Float, sizeof(Vertex), Marshal.OffsetOf(typeof(Vertex), "TexCoords"));

            Ebo = new BufferObject<ushort>(Indices.ToArray(), BufferTarget.ElementArrayBuffer);
            Vao.LinkBufferObject(ref Ebo);

            Vertices.Clear();
            Indices.Clear();

        }
        public void RenderFrame()
        {
            Vao.Bind();
            GL.DrawElements(PrimitiveType.Triangles, indicesCount, DrawElementsType.UnsignedShort, 0);
        }
        public void Dispose()
        {
            Vao.Dispose();
            Vbo.Dispose();
            Ebo.Dispose();
            
        }
    }
}