#version 460 core

out vec4 FragColor;

uniform sampler2D DiffuseMap;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoord;


void main()
{

    FragColor = texture(DiffuseMap, TexCoord);
    if(FragColor.a < 0.1)
        discard;

    
}