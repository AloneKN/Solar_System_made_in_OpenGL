#version 460 core

out vec4 FragColor;

uniform sampler2D DiffuseMap;

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoord;

struct Light
{
    float Ambient;
    float Shininess;
    float Specular;
    float Diffuse;

};

uniform Light light;

uniform vec3 lightPos;
uniform vec3 viewPos;


void main()
{
    vec3 ambient = light.Ambient * texture(DiffuseMap, TexCoord).rgb;

    // Diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(lightPos - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.Diffuse * diff * vec3(texture(DiffuseMap, TexCoord));

    // Specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), light.Shininess);
    vec3 specular = vec3(light.Specular) * spec;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
    
}