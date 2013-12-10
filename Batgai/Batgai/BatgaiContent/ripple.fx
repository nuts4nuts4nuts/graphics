uniform extern texture ScreenTexture;	

sampler ScreenS = sampler_state
{
	Texture = <ScreenTexture>;	
};

float wave;
float distortion;
float2 centerCoord;

float4 PixelShaderFunction(float2 texCoord: TEXCOORD0) : COLOR
{
	float2 distance = abs(texCoord - centerCoord);
	float scalar = length(distance);

	scalar = abs(1 - scalar);

	float sinoffset = sin(wave / scalar);
	sinoffset = clamp(sinoffset, 0, 1);
	
	float sinsign = cos(wave / scalar);	
	
	sinoffset = sinoffset * distortion/32;
	
	float4 color = tex2D(ScreenS, texCoord+(sinoffset*sinsign));	
		
	return color;
}
technique
{
	pass P0
	{
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
