Shader "Unlit/ShaderElegantGlow"
{
    Properties
    {
        _WarnaUtama ("Warna Utama", Color) = (0.1, 0.5, 1, 1)
        _WarnaKedua ("Warna Aksen", Color) = (0.8, 0.2, 1, 1)
        _KecepatanAnimasi ("Kecepatan Aliran", Float) = 1.5
        _SkalaPola ("Skala Pola", Float) = 3.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "Unlit2D"
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float4 _WarnaUtama;
                float4 _WarnaKedua;
                float _KecepatanAnimasi;
                float _SkalaPola;
            CBUFFER_END

            struct Atribut
            {
                float4 posisiObjek : POSITION;
                float2 uv : TEXCOORD0;
            };  

            struct KeFragment
            {
                float4 posisiClip : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            KeFragment Vert(Atribut masuk)
            {
                KeFragment keluar;
                keluar.posisiClip = TransformObjectToHClip(masuk.posisiObjek.xyz);
                keluar.uv = masuk.uv;
                return keluar;
            }

            half4 Frag(KeFragment masuk) : SV_Target
            {
                // Menggunakan koordinat UV dengan pusat di tengah (0,0)
                float2 uv = masuk.uv * _SkalaPola - (_SkalaPola * 0.5);
                
                // Efek gelombang sinus yang bergeser perlahan berdasarkan waktu
                float t = _Time.y * _KecepatanAnimasi;
                
                // Perhitungan matematis untuk membentuk gradasi cair yang elegan
                float gelombang = sin(uv.x + t) + cos(uv.y + t) + sin(uv.x - uv.y + (t * 0.5));
                
                // Normalisasi hasil gelombang ke rentang 0 sampai 1
                float f = (gelombang + 3.0) / 6.0;

                // Melakukan interpolasi (blend) yang halus antara dua warna pilihan
                half3 warnaGradasi = lerp(_WarnaUtama.rgb, _WarnaKedua.rgb, f);

                // Mengatur alpha (transparansi) agar tetap lembut di bagian pinggir jika menggunakan UV
                half alpha = (_WarnaUtama.a + _WarnaKedua.a) * 0.5;

                return half4(warnaGradasi, alpha);
            }
            ENDHLSL
        }
    }
}