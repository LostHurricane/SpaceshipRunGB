Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _Tex1 ("Texture1", 2D) = "white" {} // ��������1
        _Tex2 ("Texture2", 2D) = "white" {} // ��������2
        _MixValue("Mix Value", Range(0,1)) = 0.5 // �������� ���������� �������
        _Color("Main Color", COLOR) = (1,1,1,1) // ���� �����������
        _Height("Height", Range(0,20)) = 0.5 // ���� ������
        _Offset("Offset", Range(0, 3.14)) = 0 // ���� ������
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
            #include "UnityCG.cginc" // ���������� � ��������� ���������

            sampler2D _Tex1; // ��������1
            float4 _Tex1_ST;

            sampler2D _Tex2; // ��������2
            float4 _Tex2_ST;
            
            float _MixValue; // �������� ����������
            float4 _Color; // ����, ������� ����� ������������ �����������

            float _Height;
            float _Offset;

            struct v2f
            {
                float2 uv : TEXCOORD0; // UV-���������� �������
                float4 vertex : SV_POSITION; // ���������� �������
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
