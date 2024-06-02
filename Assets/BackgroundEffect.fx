//sampler2D implicitInputSampler : register(S0);
float Time : register(C0);
float2 Resolution : register(C1);

//Made by bhpcv252 on Shadertoy.com: https://www.shadertoy.com/view/fsSfD3
#define NS 100.
#define CI 0.3

float N21(float2 p)
{
    return frac(sin(p.x * 100. + p.y * 7446.) * 8345.);
}

float SS(float2 uv)
{
    float2 lv = frac(uv);
    lv = lv * lv * (3. - 2. * lv);
    float2 id = floor(uv);
    
    float bl = N21(id);
    float br = N21(id + float2(1., 0.));
    float b = lerp(bl, br, lv.x);

    float tl = N21(id + float2(0., 1.));
    float tr = N21(id + float2(1., 1.));
    float t = lerp(tl, tr, lv.x);

    return lerp(b, t, lv.y);
}

float L(float2 uv, float2 ofs, float b, float l)
{
    return smoothstep(0., 1000., b * max(0.1, l) / pow(max(0.0000000000001, length(uv - ofs)), 1. / max(0.1, l)));
}

float rand(float2 co, float s)
{
    float PHI = 1.61803398874989484820459;
    return frac(tan(distance(co * PHI, co) * s) * co.x);
}

float2 H12(float s)
{
    float x = rand(float2(243.234, 63.834), s) - .5;
    float y = rand(float2(53.1434, 13.1234), s) - .5;
    return float2(x, y);
}

float4 main(float2 uv : TEXCOORD0) : COLOR
{
    float4 col = float4(.0,0,0,0);
    
    float4 b = float4(0.01176470588, 0.05098039215, 0.14117647058, 1.);
    float4 p = float4(0.13333333333, 0.07843137254, 0.13725490196, 1.);
    float4 lb = float4(0.10196078431, 0.21568627451, 0.33333333333, 1.);
    
    float4 blb = lerp(b, lb, -uv.x * .2 - (uv.y * .5));
    
    col += lerp(blb, p, uv.x - (uv.y * 1.5));
    
    for (float i = 0.; i < NS; i++)
    {
    
        float2 ofs = H12(i + 1.);
        ofs *= float2(1.8, 1.1);
        float r = (fmod(i, 20.) == 0.) ? 0.5 + abs(sin(i / 50.)) : 0.25;
        float ligma = L(uv, ofs, r + (sin(frac(Time) * .5 * i) + 1.) * 0.02, 1.);
        col += float4(ligma, ligma, ligma, ligma);
    }
    
    uv.x += Time * .03;
    uv.y += sin(Time * .03);
    
    
    float c = 0.;
    
    for (float i2 = 1.; i2 < 8.; i2 += 1.)
    {
        c += SS(uv * pow(2., i2)) * pow(0.5, i2);
    }
    
    col = col + c * CI;

    return col;
}