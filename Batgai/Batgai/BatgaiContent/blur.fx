uniform extern texture screenTexture;

sampler screenS = sampler_state
{
	Texture = <screenTexture>;
};

float2 mFocalPos;
float mChargeAmount;

float4 PixelShaderFunction(float2 curCoord : TEXCOORD0) : COLOR0
{
	float2 center = mFocalPos;
	float maxDistSQR = 0.7071f;

	float2 diff = abs(curCoord - center);
	float distSQR = length(diff);

	float blurAmount = (distSQR / maxDistSQR) / mChargeAmount;

	float2 prevCoord = curCoord;
	prevCoord[0] -= blurAmount;

	float2 nextCoord = curCoord;
	nextCoord[0] += blurAmount;

	float4 color = ((tex2D(screenS, curCoord) + tex2D(screenS, prevCoord) + tex2D(screenS, nextCoord))/3.0f);

	return color;
}

technique Technique1
{
	pass P0
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}