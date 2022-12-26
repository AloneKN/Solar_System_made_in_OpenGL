using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace MyGame
{
    public class Earth
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap, NightMap, SpecularMap, NormalMap;
        public Earth()
        {
            shader = new ShaderProgram("Samplers/Earth/shader.vert", "Samplers/Earth/shader.frag");

            DiffuseMap = TextureProgram.Load("Resources/solarSystem/2k_earth_diffuse.jpg");
            SpecularMap = TextureProgram.Load("Resources/solarSystem/2k_earth_specular_map.png", PixelInternalFormat.Rgba);
            NormalMap = TextureProgram.Load("Resources/solarSystem/2k_earth_normal_map.png", PixelInternalFormat.Rgba);

            // representa a terra de noite
            NightMap = TextureProgram.Load("Resources/solarSystem/2k_earth_nightmap.jpg");
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
            

            shader.SetUniform("maps.DiffuseMap", DiffuseMap.Use());

            shader.SetUniform("maps.SpecularMap", SpecularMap.Use(TextureUnit.Texture1));

            shader.SetUniform("maps.NormalMap", NormalMap.Use(TextureUnit.Texture2));

            SphereAssimp.RenderSphere();
        }
        public void UpdateFrame()
        {

        }
        public void Dispose()
        {
            shader.Dispose();
        }
    }
}