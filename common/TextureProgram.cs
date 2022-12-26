using OpenTK.Graphics.OpenGL4;

using StbImageSharp;

namespace MyGame
{
    // Uma classe auxiliar, muito parecida com Shader, destinada a simplificar o carregamento de texturas.
    public class TextureProgram : IDisposable 
    {
        private readonly int Handle;
        private int Unit = 0;
        public static TextureProgram Load(string path, PixelInternalFormat pixelFormat = PixelInternalFormat.SrgbAlpha)
        {
            if(!File.Exists(path))
                throw new Exception($"Não foi possivel para encontrar a Textura: {path}");

            int handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, handle);

            StbImage.stbi_set_flip_vertically_on_load(1);
            using(Stream stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                    
                GL.TexImage2D(TextureTarget.Texture2D, 0, pixelFormat, 
                    image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }


            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return new TextureProgram(handle);
        }
        public TextureProgram(int glHandle)
        {
            Handle = glHandle;
        }
        public int Use(TextureUnit unit = TextureUnit.Texture0)
        {
            if(Unit == 0)
                Unit = numUnit(unit);

            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
            
            return Unit;
            
        }
        private int numUnit(TextureUnit unit)
        {
            string unitNum = $"{unit}";
            switch(unitNum.Length)
            {
                case 8:
                    return int.Parse(unitNum[(unitNum.Length - 1)..]);
                
                case 9:
                    return int.Parse(unitNum[(unitNum.Length - 2)..]);

                default:
                    return 0;
            }
        }
        public void Dispose()
        {
            GL.DeleteTexture(Handle);
        }
    }
}


