using OpenTK.Mathematics;

using Vector4 = System.Numerics.Vector4;

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
        crosshairColor = Vector4.One,
        LinesColor = Convert.ColorToVec4(Color4.AliceBlue);

        public static float gammaBackground = 1.5f; 
        public static float cameraVel = 50.0f;

        public static int PrimitiveType = 1;

        // bloom
        public static bool DisableBloomScene = false; 
        public static float new_bloom_exp = 0.761f;
        public static float new_bloom_streng = 0.532f;
        public static float new_bloom_gama = 0.364f;
        public static float filterRadius = 0.0082344f;
        public static float new_bloom_filmGrain = -0.1f;
        public static float nitidezStrengh = 0.0625021f;

        public static float outdoorSize = 0.0f;

        // solar system values
        public static bool pauseSystem = false;
        public static float UpdateVel = 0.0008412f;
        public static float VelWaves = 0.005f;

        // earth
        public static float LightAmbiente = 1.0f;
        public static float LightShininess = 2.0f;
        public static float LightSpecular = 1.0f;
        public static float LightDiffuse = 1.0f;

    }
}