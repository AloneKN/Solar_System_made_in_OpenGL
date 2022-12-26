#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D imagem;
uniform vec4 color;

void main()
{
    vec4 result = texture( imagem, TexCoords );
    if(result.a == 0.0)
        discard;

    FragColor = result * color;
}