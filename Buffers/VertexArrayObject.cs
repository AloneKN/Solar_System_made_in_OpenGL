using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

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
        public void LinkBufferObject(ref BufferObject<float> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<int> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<uint> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<ushort> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<Vector3> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<Vertex> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public void LinkBufferObject(ref BufferObject<Matrix4> bufferObject)
        {
            Bind();
            bufferObject.Bind();
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, int vertexSize_Bytes, int offSet_Bytes)
        {
            GL.VertexAttribPointer(index, count, type, false, vertexSize_Bytes, offSet_Bytes);
            GL.EnableVertexAttribArray(index);
        }
        public unsafe void VertexAttributePointer(uint index, int count, VertexAttribPointerType type, int vertexSize_Bytes, IntPtr offSet_Bytes)
        {
            GL.VertexAttribPointer(index, count, type, false, vertexSize_Bytes, (int)offSet_Bytes);
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