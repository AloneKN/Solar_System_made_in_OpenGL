using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
// using System.Numerics;

namespace MyGame
{
    public class Asteroids
    {
        private ShaderProgram shader;
        private TextureProgram DiffuseMap;
        private List<InstancedMeshe> meshe;
        public unsafe Asteroids()
        {
            shader = new ShaderProgram("Samplers/Asteroids/shader.vert", "Samplers/Asteroids/shader.frag");

            DiffuseMap = new TextureProgram("Resources/solarSystem/rocha.png");

            var rand = new System.Random().NextDouble;
            
            var inputModel = new List<Matrix4>();

            float radius = SolarSystemValues.JUPITER.Position.X - SolarSystemValues.MARTE.Position.X;
            float offset = 120f;
            int amount = 20000;

            for( int i = 0; i < amount; i++)
            {
                var model = Matrix4.Identity;
                var position = new Vector3();

                float angle = (float)i / (float)amount * 360f;

                float displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
                position.X = MathF.Sin(angle) * radius + displacement;

                displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
                position.Y = displacement * 0.3f;

                displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
                position.Z = MathF.Cos(angle) * radius + displacement;

                float scale = ((float)rand() * 2f);
                model = model * Matrix4.CreateScale(scale);
                

                float rotAngleX = (float)rand() * 360f;
                float rotAngleY = (float)rand() * 360f;
                float rotAngleZ = (float)rand() * 360f;
                model = model * Matrix4.CreateRotationX(rotAngleX);
                model = model * Matrix4.CreateRotationY(rotAngleY);
                model = model * Matrix4.CreateRotationZ(rotAngleZ);
                

                model = model * Matrix4.CreateTranslation(position);

                inputModel.Add(model);

            }
            
            meshe = AssimpModel.Load("Resources/Model3D/rock.obj", inputModel);

        }
        public unsafe void RenderFrame(Matrix4 model)
        {

            shader.Use();
            shader.SetUniform("view", Camera.ViewMatrix);
            shader.SetUniform("projection", Camera.ProjectionMatrix);
            shader.SetUniform("viewPos", Camera.Position);
            shader.SetUniform("lightPos", SolarSystemValues.Luz);
            shader.SetUniform("model", model);

            // shader.SetUniform("light.Specular", Values.LightSpecular);
            // shader.SetUniform("light.Shininess", Values.LightShininess);
            // shader.SetUniform("light.Ambient", Values.LightAmbiente);
            // shader.SetUniform("light.Diffuse", Values.LightDiffuse);

            shader.SetUniform("light.Ambient", 1.0f);
            shader.SetUniform("light.Shininess", 16f);
            shader.SetUniform("light.Specular", 2.123f);
            shader.SetUniform("light.Diffuse", 4.548f);

            shader.SetUniform("color", Values.asteoridesColor);

            shader.SetUniform("DiffuseMap", DiffuseMap.Use);
            
            foreach(var item in meshe)
            {
                item.RenderFrame();
            }

        }
    }
}


// for( int i = 0; i < amount; i++)
// {
//     float 2f = 2f;
//     float 100f = 100f;

    
//     float angle = (float)i / (float)amount * 360f;

//     float displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
//     float x = MathF.Sin(angle) * radius + displacement;

//     // displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
//     float y = displacement * 0.4f;

//     displacement = (float)rand() * (2f * offset * 100f) / 100f - offset;
//     float z = MathF.Cos(angle) * radius + displacement;

//     var model = Matrix4.Identity;
//     float scale = ((float)rand() * 50f) / 100f + 0.05f;
//     model = model * Matrix4.CreateScale(scale);

//     float rotAngleX = (float)rand() * 360f;
//     float rotAngleY = (float)rand() * 360f;
//     float rotAngleZ = (float)rand() * 360f;
//     model = model * Matrix4.CreateRotationX(rotAngleX);
//     model = model * Matrix4.CreateRotationY(rotAngleY);
//     model = model * Matrix4.CreateRotationZ(rotAngleZ);

//     model = model * Matrix4.CreateTranslation(new Vector3(x, y, z));

//     inputModel.Add(model);

// }