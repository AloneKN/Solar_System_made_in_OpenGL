using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace MyGame
{
    public struct BloomMip
    {
        public Vector2 size;
        public Vector2i intSize;
        public int texture;
   
    }
    public class BloomFBO : IDisposable
    {
        private Vector2i sizeWindow { get => Program.Size; }
        private int mFBO;
        public List<BloomMip> mMipChain { get; private set; }
        private readonly int mipCHainLenght; 
        public BloomFBO(int mipCHainLenght)
        {
            this.mipCHainLenght = mipCHainLenght;

            mFBO = GL.GenFramebuffer();

            mMipChain = new List<BloomMip>();
            for(int i = 0; i < mipCHainLenght; i++)
                mMipChain.Add(new BloomMip() { texture = GL.GenTexture() });

            ResizedFrameBuffer();
        }
        public void ResizedFrameBuffer()
        {

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFBO);
            Vector2 mipSize = sizeWindow;
            Vector2i mipIntSize = sizeWindow;

            for(int i = 0; i < mipCHainLenght; i++)
            {
                mipSize *= 0.5f;
                mipIntSize /= 2;
                
                int tex = mMipChain[i].texture;

                mMipChain[i] = new BloomMip()
                {
                    texture = tex,
                    size = mipSize,
                    intSize = mipIntSize,
                };

                GL.BindTexture(TextureTarget.Texture2D, mMipChain[i].texture);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R11fG11fB10f, 
                    mipIntSize.X, mipIntSize.Y, 0, PixelFormat.Rgb, PixelType.Float, IntPtr.Zero);
                
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            }

            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, 
                TextureTarget.Texture2D, mMipChain[0].texture, 0);

            DrawBuffersEnum[] attachments = { DrawBuffersEnum.ColorAttachment0}; 
            GL.DrawBuffers(1, attachments);

            if(GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                    Console.WriteLine("Framebuffer Not complete!");
                    
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

        }
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, mFBO);
        }
        public void Dispose()
        {
            GL.DeleteFramebuffer(mFBO);

            foreach(var item in mMipChain)
                GL.DeleteTexture(item.texture);
        }
    }
    public class RendererBloom
    {
        private BloomFBO mFBO;
	    private ShaderProgram mDownsampleShader;
	    private ShaderProgram mUpsampleShader;
        private Vector2i sizeWindow { get => Program.Size; }

        private bool mKarisAverageOnDownsample = true;
        public int BloomTexture { get => mFBO.mMipChain[0].texture; }
        private const int num_bloom_mips = 6;

        public RendererBloom()
        {
            mFBO = new BloomFBO(num_bloom_mips);

            mDownsampleShader = new ShaderProgram("FrameBuffers/new bloom/new_bloom.vert", "FrameBuffers/new bloom/downscale.frag");
            mUpsampleShader = new ShaderProgram("FrameBuffers/new bloom/new_bloom.vert", "FrameBuffers/new bloom/upscale.frag");

        }
        private void renderDownSamples(int srcTexture)
        {
            var mipChain = mFBO.mMipChain;
            mDownsampleShader.Use();
            mDownsampleShader.SetUniform("srcResolution", sizeWindow);
            if(mKarisAverageOnDownsample)
            {
                mDownsampleShader.SetUniform("mipLevel", 0);
            }

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, srcTexture);
            mDownsampleShader.SetUniform("srcTexture", 0);

            for(int i = 0; i < mipChain.Count; i++)
            {
                BloomMip mip = mipChain[i];
                GL.Viewport(0, 0, (int)mip.size.X, (int)mip.size.Y);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                    TextureTarget.Texture2D, mip.texture, 0);

                Quad.RenderQuad();

                mDownsampleShader.SetUniform("srcResolution", mip.size);
                GL.BindTexture(TextureTarget.Texture2D, mip.texture);

                if(i == 0)
                {
                    mDownsampleShader.SetUniform("mipLevel", 1);
                }
            }
        }
        private void RenderUpSamples()
        {
            var mipChain = mFBO.mMipChain;

            mUpsampleShader.Use();
            mUpsampleShader.SetUniform("filterRadius", Values.filterRadius);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.One, BlendingFactor.One);
            // GL.BlendEquation(BlendEquationMode.FuncAdd);

            for(int i = mipChain.Count - 1; i > 0; i--)
            {
                BloomMip mip = mipChain[i];
                BloomMip nextMip = mipChain[i - 1];

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, mip.texture);
                mUpsampleShader.SetUniform("srcTexture", 0);

                GL.Viewport(0, 0, (int)nextMip.size.X, (int)nextMip.size.Y);
                GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                    TextureTarget.Texture2D, nextMip.texture, 0);

                Quad.RenderQuad();
            }

            // GL.Disable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        }
        public void RenderBloomTexture(int srcTexture)
        {
            mFBO.Bind();

            renderDownSamples(srcTexture);
            RenderUpSamples();

            GL.Viewport(0, 0, sizeWindow.X, sizeWindow.Y);
        }
        public void ResizedFrameBuffer()
        {
            mFBO.ResizedFrameBuffer();
        }
        public void Dispose()
        {
            mFBO.Dispose();
            mDownsampleShader.Dispose();
            mUpsampleShader.Dispose();
        }
    }
}