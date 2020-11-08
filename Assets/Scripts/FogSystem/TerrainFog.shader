Shader "Hidden/TerrainFogShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogUL ("Fog - upper left", 2D) = "black" {}
        _FogUR ("Fog - upper right", 2D) = "black" {}
        _FogLL ("Fog - lower left", 2D) = "black" {}
        _FogLR ("Fog - lower right", 2D) = "black" {}
        _FogPos ("FogUL world coord", Vector) = (0, 0, 0, 0)
        _FogSize ("Fog texture grid size", Float) = 16
        _CameraBounds ("Camera zoom", Vector) = (0, 0, 1, 1)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _FogUL;
            sampler2D _FogUR;
            sampler2D _FogLL;
            sampler2D _FogLR;
            fixed4 _FogPos;
            fixed _FogSize;
            fixed4 _CameraBounds;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed2 camMin = _CameraBounds.xy;
                fixed2 camMax = _CameraBounds.zw;
                fixed2 fragPos = camMin * (1 - i.uv) + camMax * i.uv;
                fixed2 fogPos = _FogPos.xy;
                
                fixed2 uv = (fragPos - fogPos) / _FogSize;
                fixed fog = 0;
                if (fragPos.x < fogPos.x) {
                    if (fragPos.y < fogPos.y) {
                        uv = 1 + uv;
                        fog = tex2D(_FogLL, uv);
                    } else {
                        uv.x = 1 + uv.x;
                        fog = tex2D(_FogUL, uv);
                    }
                } else {
                    if (fragPos.y < fogPos.y) {
                        uv.y = 1 + uv.y;
                        fog = tex2D(_FogLR, uv);
                    } else {
                        fog = tex2D(_FogUR, uv);
                    }
                }
                
                // fixed fog = tex2D(target, uv).r;
                // if (_CameraPos.x < _FogPos.x && _CameraPos.y > _FogPos.y) fog = tex2D(_FogUL, i.uv - )
                // just invert the colors
                col.rgb *= fog;
                return col;
            }
            ENDCG
        }
    }
}
