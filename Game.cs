using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using NAudio.Wave;

namespace MyGame
{
    public class Game : IDisposable
    {
        public CubeMap cubeMap;
        private Text text;
        private ViewPort crossHair;
        private Bloom bloom;
        public Sun sun; 
        // public Earth earth;
        public LinesOrbits linesOrbits;
        public LinesOrbits lineTerra;
        private Dictionary<Astro, Renderer> Planets = new Dictionary<Astro, Renderer>();
        public Atmosphere VenusAtmosphere, terraAtmosphere;
        public List<RingPlanet> ringPlanets = new List<RingPlanet>();
        public Asteroids asteroids;
        public Particles particles;
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;
        private Maker marcador;
        public Game()
        {
            text = new Text("Resources/Fonts/Wigners.otf");
            crossHair = new ViewPort("Resources/img/bokeh_circle.png");
            cubeMap = new CubeMap("Resources/Cubemap/Milkyway.hdr", false);
            bloom = new Bloom();

            sun = new Sun();

            // earth = new Earth();
            lineTerra = new LinesOrbits();

            Planets.Add( SolarSystemValues.MERCURIO , new OuterPlanets("Resources/solarSystem/mercurio.jpg"));
            Planets.Add( SolarSystemValues.VENUS    , new OuterPlanets("Resources/solarSystem/venus.jpg"));
            Planets.Add( SolarSystemValues.TERRA    , new Earth());
            Planets.Add( SolarSystemValues.MARTE    , new OuterPlanets("Resources/solarSystem/marte.jpg"));
            Planets.Add( SolarSystemValues.JUPITER  , new OuterPlanets("Resources/solarSystem/jupiter.jpg"));
            Planets.Add( SolarSystemValues.SATURNO  , new OuterPlanets("Resources/solarSystem/saturno.jpg"));
            Planets.Add( SolarSystemValues.URANO    , new OuterPlanets("Resources/solarSystem/urano.jpg"));
            Planets.Add( SolarSystemValues.NETUNO   , new OuterPlanets("Resources/solarSystem/netuno.jpg"));

            linesOrbits = new LinesOrbits();

            VenusAtmosphere = new Atmosphere("Resources/solarSystem/venus-atmosfera.jpg");
            terraAtmosphere = new Atmosphere("Resources/solarSystem/2k_earth_clouds.jpg");

            ringPlanets.Add(new RingPlanet("Resources/solarSystem/ringJupiter.png"));
            ringPlanets.Add(new RingPlanet("Resources/solarSystem/ringSaturn.png"));
            ringPlanets.Add(new RingPlanet("Resources/solarSystem/ringUrano.png"));
            ringPlanets.Add(new RingPlanet("Resources/solarSystem/ringNetuno.png"));

            particles = new Particles("Resources/solarSystem/particle.png");
            
            asteroids = new Asteroids();

            // marcador = new Maker("Resources/solarSystem/arrow.png");
            marcador = new Maker("Resources/solarSystem/marcador.png");


            audioFile = new AudioFileReader("Resources/sound/abandoned_land.mp3");
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();

        }
        private float CurretTimerSystem = 0.0f; 
        public void RenderFrame()
        {
            bloom.Active(Values.DisableBloomScene);

            cubeMap.RenderFrame();

            if(!Values.pauseSystem)
                CurretTimerSystem += Clock.ElapsedTime * Values.UpdateVel;

            {
                var details = SolarSystemValues.SOL;

                Matrix4 model_Sun = Matrix4.Identity;
                model_Sun = model_Sun * Matrix4.CreateScale(details.Size * 4.0f);
                model_Sun = model_Sun * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model_Sun = model_Sun * Matrix4.CreateTranslation(details.Position);


                sun.RenderFrame(model_Sun);

            }

            foreach(var item in Planets)
            {
                var details = item.Key;
                Matrix4 model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size);
                model = model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(details.Axis));
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));


                item.Value.RenderFrame(model);
                linesOrbits.RenderFrame(details.Position);

            }

            {
                // atmosphere logic    
                var details = SolarSystemValues.TERRA;
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size + 0.25f);
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                terraAtmosphere.RenderFrame(model, 0.25f);


                details = SolarSystemValues.VENUS;
                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size + 0.5f);
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                VenusAtmosphere.RenderFrame(model, 0.1f);

            }

            // Ring logic
            int cont = 0;
            int cont01 = 0;
            foreach(var item in Planets)
            {
                if(cont >= 4)
                {
                    var details = item.Key;
                    var model = Matrix4.Identity;
                    model = model * Matrix4.CreateScale(details.Size * 0.0073748f);
                    model = model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(details.Axis));
                    model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                    model = model * Matrix4.CreateTranslation(details.Position);
                    model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                    ringPlanets[cont01].RenderFrame(model);

                    cont01++;
                }

                cont++;
            }
            {
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(( CurretTimerSystem + 2.0f) * 10.0f));
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(( CurretTimerSystem + 2.0f) * 10.0f));
                model = model * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(( CurretTimerSystem + 2.0f) * 10.0f));
                asteroids.RenderFrame(model);

            }

            // marcadores
            foreach(var item in Planets)
            {
                var details = item.Key;
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));

                var pos = GetPositionForMatrix(model);
                var move = MathF.Sin(Clock.Time) / 0.05f + 30f;
                pos.Y += details.Size * 2.0f;
                pos.Y += move;

                marcador.RenderFrame(pos, new System.Numerics.Vector2(Values.markerScale), Values.MakerColor);
            }

            particles.RenderFrame(Values.particlesColor);


            bloom.RenderFrame(Values.DisableBloomScene);

            crossHair.RenderFrame(Vector2.Zero, 0.03f);
            var textPos = new Vector2(10.0f, Program.Size.Y - 50.0f);
            text.RenderText($"Frames: {Clock.FramesForSecond.ToString()}", textPos, 0.45f, Convert.ColorToVec4(Color4.SkyBlue));


        }
        public void ResizedFrame()
        {
            bloom.ResizedFrameBuffer(Values.DisableBloomScene);
        }
        public void UpdateFrame()
        {
        }
        public void Dispose()
        {   

        }
        private Vector3 GetPositionForMatrix(Matrix4 model)
        {
            return new Vector3(model.Row3.Xyz);
        }
    }
}

