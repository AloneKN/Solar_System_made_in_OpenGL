#version 460 core

out vec4 FragColor;

uniform sampler2D DiffuseMap;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoord;

uniform float Timer;

uniform float alpha;

vec3 ondulation()
{
    const float spacingX = 50.0;
    const float spacingY = 50.0;
    
    vec4 textureMove = texture( DiffuseMap, TexCoord + 0.003 * vec2( sin(Timer + spacingX * TexCoord.x),cos(Timer + spacingY * TexCoord.y)) );

    vec4 textureWave = texture(DiffuseMap, vec2(TexCoord.x + Timer * 0.01, TexCoord.y + Timer * 0.001));

    return mix(textureMove, textureWave, 0.5).rgb;
}


void main()
{

    
    // vec3 color_diff = ondulation() - vec3(0.0, 0.0, 0.0);
    // float square_distance = dot(color_diff, color_diff);

    // const float squared_threshold = (0.1 * 0.1);
    // if(square_distance < squared_threshold)
    // {
    //     FragColor = vec4(color_diff, alpha);
    // }

    FragColor = vec4(ondulation(), alpha);

    
}