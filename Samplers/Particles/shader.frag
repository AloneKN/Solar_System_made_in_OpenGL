#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D imagem;
uniform vec4 color = vec4(1.0);

void main()
{
    vec4 result = texture( imagem, TexCoords ) * color;

    if(result.a == 0.0)
        discard;

    FragColor = result;
    
}