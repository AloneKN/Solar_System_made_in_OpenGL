#version 460 core

layout ( location = 0 ) in vec3 aPos;
layout ( location = 1 ) in vec3 aNormals;
layout ( location = 2 ) in vec2 aTexCoords;
layout ( location = 4 ) in mat4 aModel;


uniform mat4 projection;
uniform mat4 view;
uniform mat4 model;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoord;

void main()
{
    mat4 modelMatrix = model * aModel;

    FragPos = vec3(vec4(aPos, 1.0) * modelMatrix);
    Normal = aNormals * mat3(transpose(inverse(modelMatrix)));
    TexCoord = aTexCoords;
    gl_Position = vec4(aPos, 1.0) * modelMatrix  * view * projection;


    // gl_Position = vec4(aPos, 1.0) * aModel * view * projection ;
    
}