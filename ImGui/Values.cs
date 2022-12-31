using Color4 = OpenTK.Mathematics.Color4;

using System.Numerics;

namespace MyGame
{
    public struct Convert
    {
        public static Vector4 ColorToVec4(Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }
        public static Color4 Vec4ToColor4(Vector4 color)
        {
            return new Color4(color.X, color.Y, color.Z, color.Z);
        }
    }
    // the values 
    public struct Values
    {
        public static Vector4 lightColor = Vector4.One,
        fpsColor = Vector4.One,
        crosshairColor = Convert.ColorToVec4(Color4.SkyBlue),
        LinesColor = Convert.ColorToVec4(Color4.SkyBlue),
        MakerColor = Convert.ColorToVec4(Color4.SkyBlue),
        particlesColor = Vector4.One,
        asteoridesColor = Vector4.One;

        public static float gammaBackground = 1.5f; 
        public static float cameraVel = 50.0f;

        public static int PrimitiveType = 1;
        public static float markerScale = 10.0f;

        // bloom
        public static bool DisableBloomScene = false; 
        public static float new_bloom_exp = 1.0f;
        public static float new_bloom_streng = 0.532f;
        public static float new_bloom_gama = 0.364f;
        public static float filterRadius = 0.0082344f;
        public static float new_bloom_filmGrain = -0.1f;
        public static float nitidezStrengh = 0.0625021f;

        // solar system values
        public static bool pauseSystem = false;
        public static float UpdateVel = 0.0008412f;

        // earth
        public static float LightAmbiente = 1.0f;
        public static float LightShininess = 2.0f;
        public static float LightSpecular = 1.0f;
        public static float LightDiffuse = 1.0f;


    }
}