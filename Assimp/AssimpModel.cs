using OpenTK.Mathematics;
using Assimp;

namespace MyGame
{
    public class AssimpModel : IDisposable
    {
        private static List<Meshe> ?_meshes;
        public static List<Meshe> Load(string FilePath, bool FlipUVs = false)
        {
            _meshes = new List<Meshe>();
            
            Init(FilePath, FlipUVs);
            
            return _meshes!;
        }
        private static List<InstancedMeshe> ?_instancedMesh;
        private static List<Matrix4> ?_matricesModel;
        public static List<InstancedMeshe> Load(string FilePath, List<Matrix4> matricesModel, bool FlipUVs = false)
        {
            _instancedMesh = new List<InstancedMeshe>();
            _matricesModel = new List<Matrix4>(matricesModel);

            Init(FilePath, FlipUVs);
            
            return _instancedMesh!;
        }
        private static Scene ?_scene;
        private static void Init(string FilePath, bool FlipUVs)
        {
            if(!File.Exists(FilePath))
                throw new Exception($"ERROR::ASSIMP:: Arquivo nao encontrado: {FilePath}..");

            using(var importer = new AssimpContext())
            {
                if(FlipUVs)
                {
                    _scene = importer.ImportFile(FilePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.FlipUVs | PostProcessSteps.CalculateTangentSpace);
                } else
                {
                    _scene = importer.ImportFile(FilePath, PostProcessSteps.Triangulate | PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.CalculateTangentSpace);
                }
            }

            processNodes(_scene.RootNode);

        }
        private static void processNodes(Node node)
        {
            for(int i = 0; i < node.MeshCount; i++)
            {
                var _meshesValues = processMesh(_scene!.Meshes[node.MeshIndices[i]]);
                if(_meshes != null)
                {
                    _meshes!.Add(new Meshe(_meshesValues.Item1, _meshesValues.Item2));
                }
                else
                {
                    _instancedMesh!.Add( new InstancedMeshe(_meshesValues.Item1, _meshesValues.Item2, _matricesModel!));
                }
            }
            for(int i = 0; i < node.ChildCount; i++)
            {
                processNodes(node.Children[i]);
            }
        }
        private static Tuple< List<Vertex>, List<ushort> > processMesh(Mesh mesh)
        {
            var vertices = new List<Vertex>();
            for(int i = 0; i < mesh.VertexCount; i++)
            {
                var packed = new Vertex();

                packed.Positions = new Vector3(mesh.Vertices[i].X, mesh.Vertices[i].Y, mesh.Vertices[i].Z);
                packed.Normals = new Vector3(mesh.Normals[i].X, mesh.Normals[i].Y, mesh.Normals[i].Z);
                if(mesh.HasTextureCoords(0))
                {
                    packed.TexCoords = new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y);
                } else
                {
                    packed.TexCoords = new Vector2(0.0f, 0.0f);
                }

                vertices.Add(packed);
            }

            var indices = new List<ushort>();
            for(int i = 0; i < mesh.FaceCount; i++)
            {
                Face face = mesh.Faces[i];
                for(int j = 0; j < face.IndexCount; j++)
                {
                    indices.Add((ushort)face.Indices[j]);
                }
            }

            return new Tuple<List<Vertex>, List<ushort> >(vertices, indices);
        }
        public void Dispose()
        {
            _meshes = new List<Meshe>(null!);
            _instancedMesh = new List<InstancedMeshe>(null!);
            _matricesModel = new List<Matrix4>(null!);
        }
    }
}