#version 460 core

in vec2 TexCoords;

out vec4 FragColor;

uniform sampler2D imagem;
uniform vec4 color;

void main()
{

    FragColor = texture( imagem, TexCoords );
    if(color.a <= 0.1) discard;
    
    FragColor = FragColor * color;
}