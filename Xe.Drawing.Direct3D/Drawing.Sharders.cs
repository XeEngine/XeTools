using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xe.Drawing
{
    using d3d = SharpDX.Direct3D11;
    using dxgi = SharpDX.DXGI;

    public partial class DrawingDirect3D
    {
        private const string VS =
@"cbuffer MatrixBuffer : register(b0)
{
	matrix Matrix;
	matrix dummy;
};

struct VertexIn
{
	float4 pos		: POSITION;
	float2 tex		: TEXTURE;
	float4 color	: COLOR;
};
struct VertexOut
{
	float4 pos		: SV_POSITION;
	float2 tex		: TEXTURE;
	float4 color	: COLOR;
};

VertexOut main(const VertexIn vIn)
{
	VertexOut vOut;
	//vOut.pos = mul(Matrix, vIn.pos);
	vOut.pos = vIn.pos;
	vOut.tex = vIn.tex;
	vOut.color = vIn.color;
	return vOut;
}";
        private const string PS =
@"struct PixelIn
{
	float4 pos		: SV_POSITION;
	float2 tex		: TEXTURE;
	float4 color	: COLOR;
};

Texture2D tImage0;
SamplerState sampleImage0;
Texture2D tClut0;
SamplerState sampleClut0;

float4 main(PixelIn pIn) : SV_TARGET
{
    float4 texColor = tImage0.Sample(sampleImage0, pIn.tex.xy);
	float4 blendColor = pIn.color;
    return texColor * blendColor;
	/*float4 color = pIn.color;
	if (pIn.tex.z < 0.50)
	{
		// Use palette W texture
		float colorIndex = tImage0.Sample(sampleImage0, pIn.tex.xy).r;
		color *= tClut0.Sample(sampleClut0, float2(colorIndex, pIn.tex.z * 2.0));
	}
	else if (pIn.tex.z < 1.0)
	{
		// Only texture
		color *= tImage0.Sample(sampleImage0, pIn.tex.xy);
	}
	else
	{
		// Do not use texture
	}
	return color;*/
}";
    }
}
