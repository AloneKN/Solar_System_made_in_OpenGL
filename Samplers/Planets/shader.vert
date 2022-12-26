#version 460 core

layout ( location = 0 ) in vec3 aPos;
layout ( location = 1 ) in vec3 aNormals;
layout ( location = 2 ) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoord;

void main()
{
    FragPos = vec3(vec4(aPos, 1.0) * model);
    Normal = aNormals * mat3(transpose(inverse(model)));
    TexCoord = aTexCoords;
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    
}