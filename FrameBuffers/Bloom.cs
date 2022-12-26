using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace MyGame
{
    class Bloom : IDisposable
    {
        private Vector2i sizeWindow { get => Program.Size; }
        private ShaderProgram shaderNewBloomFinal;

        private int rboDepth;
        private int hrdFBO;

        public int[] colorBuffer = new int[2];
        private int[] pingpongFBO = new int[2];
        private int[] pingpongColorbuffers = new int[2];

        // new bloom
        private RendererBloom rendererBloom;

        public Bloom()
        {
            rendererBloom = new RendererBloom();

            shaderNewBloomFinal = new ShaderProgram("FrameBuffers/new bloom/new_bloom.vert", "FrameBuffers/new bloom/newbloomfinal.frag");

            GL.GenTextures(2, colorBuffer);
            rboDepth = GL.GenRenderbuffer();
            hrdFBO  = GL.GenFramebuffer();

            GL.GenFramebuffers(2, pingpongFBO);
            GL.GenTextures(2, pingpongColorbuffers);

            ResizedFrame();
        }
        private void ResizedFrame()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, hrdFBO);

            for(int i = 0; i < 2; i++)
            {
                GL.BindTexture(TextureTarget.Texture2D, colorBuffer[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f,
                    sizeWindow.X, sizeWindow.Y, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0 + i, 
                    TextureTarget.Texture2D, colorBuffer[i], 0);
            }
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer , rboDepth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, sizeWindow.X, sizeWindow.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, rboDepth);   

            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0, DrawBuffersEnum.ColorAttachment1 }; 
            GL.DrawBuffers(2, attachments);

            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    Console.WriteLine("Framebuffer Not complete!");


            for(int i = 0; i < 2; i++)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, pingpongFBO[i]);
                GL.BindTexture(TextureTarget.Texture2D, pingpongColorbuffers[i]);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba16f,
                    sizeWindow.X, sizeWindow.Y, 0, PixelFormat.Rgba, PixelType.Float, IntPtr.Zero);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
                    TextureTarget.Texture2D, pingpongColorbuffers[i], 0);

                if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    Console.WriteLine("Framebuffer Not complete!");
            }

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }
        public void Active(bool Enable)
        {
            if(Enable == false)
            {
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, hrdFBO);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            }

        }
        public void ResizedFrameBuffer(bool Enable)
        {
            if(Enable == false)
            {
                ResizedFrame();
                rendererBloom.ResizedFrameBuffer();
            }
        }
        public void RenderFrame(bool Enable)
        {
            if(Enable == false)
            {
                rendererBloom.RenderBloomTexture(colorBuffer[1]);

                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

                shaderNewBloomFinal.Use();
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, colorBuffer[0]);
                shaderNewBloomFinal.SetUniform("scene", 0);

                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, rendererBloom.BloomTexture);
                shaderNewBloomFinal.SetUniform("bloomBlur", 1);

                shaderNewBloomFinal.SetUniform("exposure", Values.new_bloom_exp);
                shaderNewBloomFinal.SetUniform("bloomStrength", Values.new_bloom_streng);
                shaderNewBloomFinal.SetUniform("gamma", Values.new_bloom_gama);
                shaderNewBloomFinal.SetUniform("film_grain", Values.new_bloom_filmGrain);
                shaderNewBloomFinal.SetUniform("elapsedTime", Clock.ElapsedTime);

                shaderNewBloomFinal.SetUniform("nitidezStrength", Values.nitidezStrengh);

                Quad.RenderQuad();

            }
        }
        public void Dispose()
        {
            rendererBloom.Dispose();
            shaderNewBloomFinal.Dispose();

            GL.DeleteTextures(2, colorBuffer);
            GL.DeleteRenderbuffer(rboDepth);
            GL.DeleteFramebuffer(hrdFBO);

            GL.DeleteFramebuffers(2, pingpongFBO);
            GL.DeleteTextures(2, pingpongColorbuffers);
        }
    }
}