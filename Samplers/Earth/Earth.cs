using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Earth : Renderer
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap, SpecularMap, NormalMap;
        public Earth()
        {
            shader = new ShaderProgram("Samplers/Earth/shader.vert", "Samplers/Earth/shader.frag");

            DiffuseMap = new TextureProgram("Resources/solarSystem/2k_earth_diffuse.jpg");
            SpecularMap = new TextureProgram("Resources/solarSystem/2k_earth_specular_map.png", 
                PixelInternalFormat.Rgba, TextureUnit.Texture1);
            NormalMap = new TextureProgram("Resources/solarSystem/2k_earth_normal_map.png", 
                PixelInternalFormat.Rgba, TextureUnit.Texture2);

        }
        public void RenderFrame(Matrix4 model)
        {

            shader.Use();
            shader.SetUniform("model", model);
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);

            shader.SetUniform("viewPos", Camera.Position);
            shader.SetUniform("lightPos", SolarSystemValues.Luz);
            shader.SetUniform("light.Specular", Values.LightSpecular);
            shader.SetUniform("light.Shininess", Values.LightShininess);
            shader.SetUniform("light.Ambiente", Values.LightAmbiente);
            shader.SetUniform("light.Diffuse", Values.LightDiffuse);
            

            shader.SetUniform("maps.DiffuseMap", DiffuseMap.Use);

            shader.SetUniform("maps.SpecularMap", SpecularMap.Use);

            shader.SetUniform("maps.NormalMap", NormalMap.Use);

            SphereAssimp.RenderSphere();
        }
        public void Dispose()
        {
            shader.Dispose();
        }
    }
}