#version 460 core

out vec4 FragColor;

uniform sampler2D DiffuseMap;

in vec2 TexCoord;
in vec3 Normal;

uniform float Timer;
uniform float LightDiffuse;

const float velWaves = 0.005; // 0.0004 < - >  0.01
vec4 ondulation()
{
    const float spacingX = 50.0;
    const float spacingY = 50.0;
    
    vec3 result = texture( DiffuseMap, TexCoord + velWaves * vec2( sin(Timer + spacingX * TexCoord.x),cos(Timer + spacingY * TexCoord.y)) ).rgb;
    return vec4(result, 1.0);
}


void main()
{
    vec4 resul = ondulation();

    vec3 diffuse = LightDiffuse  * ondulation().rgb;

    FragColor = vec4(resul.rgb + diffuse, 1.0);
}