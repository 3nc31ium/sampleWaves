float4x4 World;
float4x4 View;
float4x4 Projection;
float time;
float size;
 
texture particleTexture;

sampler2D textureSampler = sampler_state {
    Texture = (particleTexture);
    MinFilter = Linear;
    MagFilter = Linear;
    AddressU = Clamp;
    AddressV = Clamp;
};
 
struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TextureCoordinate : TEXCOORD0;

	float TimeOfBirth	: TEXCOORD1;
};
 
struct VertexShaderOutput
{
    float4 Position : POSITION0;
};

struct PixelShaderOutput
{
	float4 Color : COLOR0;
	
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

	float3 upVector = float3(0,1,0);
	float3 sideVector = float3(1,0,0);

	float3 finalPosition = input.Position;
	finalPosition += (input.TextureCoordinate.x-0.5f)*sideVector*size;
	finalPosition += upVector*(1-input.TextureCoordinate.y)*(size);	
	float4 worldPosition = mul(float4(finalPosition, 1), World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = float4(0,0,0,0);

	//Math.Sin(2f * Math.PI * (i+5) * gameTime.TotalGameTime.TotalSeconds) * 30
	/*
	if (input.TimeOfBirth==0&&sin(2*3.14159*29.99*time)>0) //left
		output.Position = mul(viewPosition, Projection);
	if (input.TimeOfBirth==1&&sin(2*3.14159*15*time)>0)
		output.Position = mul(viewPosition, Projection); //right
	if (input.TimeOfBirth==2&&sin(2*3.14159*7.5*time)>0)
		output.Position = mul(viewPosition, Projection); //up
	if (input.TimeOfBirth==3&&sin(2*3.14159*3.75*time)>0)
		output.Position = mul(viewPosition, Projection);  //down
 */
 if (input.TimeOfBirth==3&&sin(2*3.14159*0.1*time)>0)
		output.Position = mul(viewPosition, Projection);  //down
    return output;
}


 
float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float4 textureColor= float4(1,1,1,1);

    return textureColor;
}
 
technique Textured
{
    pass Pass1
    {
		CullMode = cw;
        VertexShader = compile vs_1_1 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

