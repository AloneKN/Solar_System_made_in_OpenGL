#version 460 core

layout (location = 0) in vec4 aPos;

out vec2 TexCoords;

uniform mat4 projection;


void main()
{
    gl_Position = vec4(aPos.xy, 0.0, 1.0) * projection;
    TexCoords = aPos.zw;
}