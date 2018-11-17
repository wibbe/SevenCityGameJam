Shader "Unlit/Backdrop"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Intensity ("Intensity", Range(0, 4)) = 1
        _PullStrength ("Pull Strength", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            //Blend One One
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            uniform float4 _GravityWell0;
            uniform float4 _GravityWell1;
            uniform float4 _GravityWell2;
            uniform float4 _GravityWell3;
            uniform float4 _GravityWell4;
            uniform float4 _Color;
            uniform float _Intensity;
            uniform float _PullStrength;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 applyWell(float3 vertex, float4 well)
            {
                float3 wellPos = mul(unity_WorldToObject, well.xyz);
				wellPos.y = vertex.y;
                float3 dir = normalize(wellPos - vertex);
                float strength = well.w <= 0.01 ? 0 : 1 - clamp(distance(wellPos, vertex) / well.w, 0, 1);
                return vertex + dir * strength * _PullStrength;
            }

            v2f vert (appdata v)
            {
                v2f o;

                float3 result = applyWell(v.vertex.xyz, _GravityWell0);
                result = applyWell(result, _GravityWell1);
                result = applyWell(result, _GravityWell2);
                result = applyWell(result, _GravityWell3);
                result = applyWell(result, _GravityWell4);

                o.vertex = UnityObjectToClipPos(float4(result, v.vertex.w));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                
                return col * _Color * float4(_Intensity, _Intensity, _Intensity, 1);
            }
            ENDCG
        }
    }
}
