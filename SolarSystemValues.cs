using OpenTK.Mathematics;

namespace MyGame
{
    public struct Astro
    {
        public Vector3 Position { get; private set; } 
        public float Size { get; private set; } 
        public float RotationVel { get; private set; } 
        public float OrbitVel { get; private set; } 
        public float Axis { get; private set; } 
        public Astro(Vector3 position, float size, float rotation_vel, float orbit_vel, float axis)
        {
            Position = position; 
            Size = size; 
            RotationVel = rotation_vel;
            OrbitVel = orbit_vel;
            Axis = axis;
        }
    }
    public class SolarSystemValues
    {
        public static readonly Vector3 Luz    = new Vector3(0.0f, 0.0f, 0.0f);
        private static readonly float SizeProproction = 10.0f;
        public static readonly Astro SOL      = new Astro( new Vector3( 0f, 0f, 0f),                    109.2f,                      1997f,          0.0f   , 0f );
        public static readonly Astro MERCURIO = new Astro( new Vector3( 57.91f  + SOL.Size  , 0f, 0f),  SizeProproction *  0.38f,    0.065846995f,   48.92f , 0f );
        public static readonly Astro VENUS    = new Astro( new Vector3( 108.21f + SOL.Size  , 0f, 0f),  SizeProproction *  0.95f,    0.16803278f,    35.02f , 178f);
        public static readonly Astro TERRA    = new Astro( new Vector3( 149.6f  + SOL.Size  , 0f, 0f),  SizeProproction *  1.0f,     0.27322403f,    29.78f ,  23.5f);
        public static readonly Astro MARTE    = new Astro( new Vector3( 227.92f + SOL.Size  , 0f, 0f),  SizeProproction *  0.53f,    0.51393443f,    24.07f ,  24f);
        public static readonly Astro JUPITER  = new Astro( new Vector3( 778.57f + SOL.Size  , 0f, 0f),  SizeProproction *  11.21f,   3.2409837f,     13.05f ,  3f);
        public static readonly Astro SATURNO  = new Astro( new Vector3( 1433.53f+ SOL.Size  , 0f, 0f),  SizeProproction *  9.45f,    8.046994f,      9.64f  ,  27f);
        public static readonly Astro URANO    = new Astro( new Vector3( 2872.46f+ SOL.Size  , 0f, 0f),  SizeProproction *  4.01f,    23.03907f,      6.81f  ,  98f);
        public static readonly Astro NETUNO   = new Astro( new Vector3( 4495.06f+ SOL.Size  , 0f, 0f),  SizeProproction *  3.88f,    45.04973f,      5.43f  ,  30f);
    }
}



