
sampler samplerState;
uniform extern float time; 

float4 greyscale( float2 Tex : TEXCOORD0 ) : COLOR0
{
    float4 Color = float4(0,0,0,0);
    
    for(int x=0; x<4; x++)
    {
		for(int y=0; y<4; y++)
		{
			float2 offset = float2(x / 51.0f, y / 51.0f);
			Color += tex2D(samplerState, Tex + offset + float2(sin(time / 3000 + Tex.y * 30),sin(time / 3000 +Tex.x * 30)) * 0.001);
		}
    }
    
    Color.r = 5;
        
    return Color / 16.0f;
}

technique GreyScale 
{
    pass P0 
    {
        PixelShader = compile ps_3_0 greyscale();
    }
}

