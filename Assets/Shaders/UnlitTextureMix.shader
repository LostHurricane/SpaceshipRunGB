Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Tex1 ("Texture1", 2D) = "white" {} // текстура1
        _Tex2 ("Texture2", 2D) = "white" {} // текстура2
        _MixValue("Mix Value", Range(0,1)) = 0.5 // параметр смешивания текстур
        _Color("Main Color", COLOR) = (1,1,1,1) // цвет окрашивания
        _Height("Height", Range(0,20)) = 0.5 // сила изгиба
        _Offset("Offset", Range(0, 3.14)) = 0 // сила изгиба
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // библиотека с полезными функциями

            sampler2D _Tex1; // текстура1
            float4 _Tex1_ST;

            sampler2D _Tex2; // текстура2
            float4 _Tex2_ST;
            
            float _MixValue; // параметр смешивания
            float4 _Color; // цвет, которым будет окрашиваться изображение

            float _Height;
            float _Offset;

            struct v2f
            {
                float2 uv : TEXCOORD0; // UV-координаты вершины
                float4 vertex : SV_POSITION; // координаты вершины
            };


            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata_full v)
            {
                v2f o;
                v.vertex.xyz +=  v.normal * _Height * (sin(v.texcoord.x * _Offset)) ;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _Tex1);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_Tex1, i.uv) * _MixValue;
                col += tex2D(_Tex2, i.uv) * (1 - _MixValue);
                col = col * _Color;
                return col;
            }
            ENDCG
        }
    }
}
