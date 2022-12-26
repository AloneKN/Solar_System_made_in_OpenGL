using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    // A abstração do objeto array de vértices.
    public class VertexArrayObject : IDisposable
    {
        private int Handle;
        public VertexArrayObject()
        {
            Handle = GL.GenVertexArray();
        }
        public void BindBuffer(ref BufferObject<float> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void BindBuffer(ref BufferObject<int> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void BindBuffer(ref BufferObject<uint> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void BindBuffer(ref BufferObject<ushort> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void BindBuffer(ref BufferObject<Vertex> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, int vertexSize, int offSet)
        {
            GL.VertexAttribPointer(index, count, type, false, vertexSize * sizeof(float), offSet * sizeof(float));
            GL.EnableVertexAttribArray(index);
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, int vertexSize, IntPtr offSet)
        {
            GL.VertexAttribPointer(index, count, type, false, vertexSize, offSet);
            GL.EnableVertexAttribArray(index);
        }
        public void Bind()
        {
            GL.BindVertexArray(Handle);
        }
        public void Dispose()
        {
            GL.DeleteVertexArray(Handle);
        }
    }
}