attribute vec3 fuVertex;
attribute vec3 fuNormal;
attribute vec2 fuUV;
uniform mat4 FUSEE_MVP;
uniform mat4 FUSEE_MV;
uniform mat4 FUSEE_ITMV;
varying vec3 normal;
varying vec3 viewpos;
varying vec2 uv;

void main()
{
    normal = normalize(mat3(FUSEE_ITMV) * fuNormal);
    viewpos = (FUSEE_MV * vec4(fuVertex, 1.0)).xyz;
    uv = fuUV;
    gl_Position = FUSEE_MVP * vec4(fuVertex, 1.0);
}

/*
attribute vec3 fuVertex;
attribute vec3 fuNormal;

varying vec3 normal;

uniform mat4 FUSEE_MVP;
uniform mat4 FUSEE_ITMV;

uniform vec2 linewidth;

void main() {
	normal = mat3(FUSEE_ITMV[0].xyz, FUSEE_ITMV[1].xyz, FUSEE_ITMV[2].xyz) * fuNormal;
	normal = normalize(normal);
	gl_Position = (FUSEE_MVP * vec4(fuVertex, 1.0) ) + vec4(linewidth * normal.xy, 0, 0); // + vec4(0, 0, 0.06, 0);
} 
*/ 