using OpenTK.Mathematics;
using Assimp;

namespace MyGame
{
    public class AssimpModel
    {
        private static Scene ?_scene;
        private static List<Meshe> ?_meshes;
        public static List<Meshe> Load(string FilePath, bool FlipUVs = false)
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
            _meshes = new List<Meshe>();

            processNodes(_scene.RootNode);
            
            return _meshes!;
        }
        private static void processNodes(Node node)
        {
            for(int i = 0; i < node.MeshCount; i++)
            {
                var _meshesValues = processMesh(_scene!.Meshes[node.MeshIndices[i]]);
                _meshes!.Add(new Meshe(_meshesValues.Item1, _meshesValues.Item2));
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
                packed.Tangents = new Vector3(mesh.Tangents[i].X, mesh.Tangents[i].Y, mesh.Tangents[i].Z);
                packed.Bitangents = new Vector3(mesh.BiTangents[i].X, mesh.BiTangents[i].Y, mesh.BiTangents[i].Z);

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
    }
}