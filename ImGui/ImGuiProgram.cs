using ImGuiNET;

using Vector4 = System.Numerics.Vector4;
using Vector2 = System.Numerics.Vector2;

using static ImGuiNET.ImGui;

namespace MyGame
{
    public class ImGuiProgram
    {
        private static Vector4 _lightColor = Vector4.One;
        public static void RenderFrame()
        {
            ImGui.StyleColorsClassic();

            ImGui.Begin("Scene Details");
            ImGui.NewLine();
            ImGui.ColorEdit4("Color Text Display", ref Values.fpsColor);
            
            ImGui.NewLine();
            ImGui.ColorEdit4("Color CrossHair", ref Values.crosshairColor);

            float hdr = 255f;
            ImGui.NewLine();
            ImGui.ColorEdit4("Color Light", ref _lightColor);
            Values.lightColor  = new Vector4(_lightColor.X * hdr, _lightColor.Y * hdr, 
                                            _lightColor.Z * hdr, _lightColor.W * hdr);

            ImGui.NewLine();
            ImGui.SliderFloat("Galaxy Gamma", ref Values.gammaBackground, 0.5f, 2.0f);
            ImGui.SliderFloat("Camera speed", ref Values.cameraVel, 50.0f, 200.0f);

            ImGui.NewLine();
            if( ImGui.Button($"Bloom {(Values.DisableBloomScene ? "Enable" : "Disable")}") )
            {
                Values.DisableBloomScene = !Values.DisableBloomScene;
            }
            if(Values.DisableBloomScene == false)
            {
                ImGui.SliderFloat("Bloom Exposure", ref Values.new_bloom_exp, 0.0f, 1.0f);
                ImGui.SliderFloat("Bloom Strength", ref Values.new_bloom_streng, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Gamma", ref Values.new_bloom_gama, 0.0f, 1.0f, "%.7f");
                ImGui.SliderFloat("Bloom Spacing filter", ref Values.filterRadius, 0.0f, 0.1f, "%.7f");
                ImGui.SliderFloat("Bloom Film Grain", ref Values.new_bloom_filmGrain, -0.1f, 0.1f, "%.7f");
                ImGui.SliderFloat("Bloom Nitidez Strength", ref Values.nitidezStrengh, 0.0f, 0.2f, "%.7f");
            }

            ImGui.NewLine();
            ImGui.RadioButton("Line Strip", ref Values.PrimitiveType, 1);
            ImGui.SameLine();
            ImGui.RadioButton("Line", ref Values.PrimitiveType, 2);
            ImGui.SameLine();
            ImGui.RadioButton("Points", ref Values.PrimitiveType, 3);
            ImGui.ColorEdit4("Lines Orbit Color", ref Values.LinesColor);

            ImGui.NewLine();
            ImGui.ColorEdit4("Color Marker", ref Values.MakerColor);
            ImGui.SliderFloat("Scale Marker", ref Values.markerScale, 10.0f, 20.0f);

            ImGui.NewLine();
            ImGui.ColorEdit4("Color Particles", ref Values.particlesColor);
            ImGui.ColorEdit4("Color Asteroides", ref Values.asteoridesColor);


            ImGui.Checkbox("Pause Solar System", ref Values.pauseSystem);
            if(!Values.pauseSystem)
                ImGui.SliderFloat("Update Vel", ref Values.UpdateVel, 0.0008412f, 5.0f, "%.7f");

            ImGui.NewLine();
            ImGui.Text("Planets Graphics Details");
            ImGui.SliderFloat("Light Ambiente", ref Values.LightAmbiente, 0.0f, 1.0f);
            ImGui.InputFloat($"shininess Intensity", ref Values.LightShininess, Values.LightShininess);
            if(Values.LightShininess < 2) { Values.LightShininess = 2f; }
            else if(Values.LightShininess > 256) { Values.LightShininess = 256f; }

            ImGui.SliderFloat("Light Specular", ref Values.LightSpecular, 0.0f, 5.0f);
            ImGui.SliderFloat("Light Diffuse", ref Values.LightDiffuse, 0.0f, 5.0f);
            
            ImGui.End();



        }
    }
}