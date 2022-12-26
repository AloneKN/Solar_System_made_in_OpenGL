using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace MyGame
{
    public class Game : IDisposable
    {
        public CubeMap cubeMap;
        private Text text;
        private ViewPort crossHair;
        private Bloom bloom;
        public Sun sun; 
        public Earth earth;
        public LinesOrbits linesOrbits;
        public LinesOrbits lineTerra;
        public Dictionary<Astro, OuterPlanets> Planets = new Dictionary<Astro, OuterPlanets>();
        public Atmosphere VenusAtmosphere, terraAtmosphere;
        public List<RingPlanet> ringPlanets = new List<RingPlanet>();
        public Game()
        {
            text = new Text("Resources/Fonts/Wigners.otf");
            crossHair = new ViewPort("Resources/img/bokeh_circle.png");
            cubeMap = new CubeMap("Resources/Cubemap/Milkyway.hdr", false);
            bloom = new Bloom();

            sun = new Sun();

            earth = new Earth();
            lineTerra = new LinesOrbits();

            Planets.Add( SolarSystemValues.MERCURIO , new OuterPlanets("Resources/solarSystem/mercurio.jpg"));
            Planets.Add( SolarSystemValues.VENUS    , new OuterPlanets("Resources/solarSystem/venus.jpg"));
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
                // Planet Earth logic
                var details = SolarSystemValues.TERRA;
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size);
                model = model * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(details.Axis));
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                earth.RenderFrame(model);
                lineTerra.RenderFrame(SolarSystemValues.TERRA.Position);


            }
            
            {
                // atmosphere logic    
                var details = SolarSystemValues.TERRA;
                var model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size + 2.0f);
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                terraAtmosphere.RenderFrame(model, 0.5f);


                details = SolarSystemValues.VENUS;
                model = Matrix4.Identity;
                model = model * Matrix4.CreateScale(details.Size + 2.0f);
                model = model * Matrix4.CreateRotationY(CurretTimerSystem * details.RotationVel);
                model = model * Matrix4.CreateTranslation(details.Position);
                model = model * Matrix4.CreateRotationY(MathHelper.DegreesToRadians(CurretTimerSystem * details.OrbitVel));
                VenusAtmosphere.RenderFrame(model, 0.1f);

            }

            int cont = 0;
            int cont01 = 0;

            foreach(var item in Planets)
            {
                if(cont >= 3)
                {
                    // Ring logics
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

            bloom.RenderFrame(Values.DisableBloomScene);


            crossHair.RenderFrame(Vector2.Zero, 0.03f);
            var textPos = new Vector2(10.0f, Program.Size.Y - 50.0f);
            text.RenderText($"Frames: {Clock.FramesForSecond.ToString()}", textPos, 0.45f, Values.fpsColor);

        }
        public void ResizedFrame()
        {
            bloom.ResizedFrameBuffer(Values.DisableBloomScene);
        }
        public void UpdateFrame()
        {
            earth.UpdateFrame();
        }
        public void Dispose()
        {   

        }
    }
}

