// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VertexColor" {
	Properties{
		point_size("Point Size", Float) = 5.0
	}
    SubShader {
    Pass {
        LOD 200
        Cull Off
                 
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
  
        struct VertexInput {
            float4 v : POSITION;
            float4 color: COLOR;
        };
         
        struct VertexOutput {
            float4 pos : SV_POSITION;
            float4 col : COLOR;
			float size : PSIZE;
        };
         

		 float point_size;

        VertexOutput vert(VertexInput v) {
         
            VertexOutput o;
            o.pos = UnityObjectToClipPos(v.v);
            o.col = v.color;
            o.size = point_size;
            return o;
        }
         
        float4 frag(VertexOutput o) : COLOR {
            return o.col;
        }
 
        ENDCG
        } 
    }
 
}

/*
Shader "Custom/VertexColor" {
     SubShader {
     Pass {
         Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
         LOD 200
               
                  
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         #include "UnityCG.cginc"
   
         struct VertexInput {
             float4 v : POSITION;
             float4 color: COLOR;
         };
          
         struct VertexOutput {
             float4 pos : SV_POSITION;
             float4 col : COLOR;
             float4 size : PSIZE;
         };
          
         VertexOutput vert(VertexInput v) {
          
             VertexOutput o;
             o.pos = mul(UNITY_MATRIX_MVP, v.v);
             o.col = v.color;
             o.size = 10.0;
             return o;
         }
          
         float4 frag(VertexOutput o) : COLOR {
             return o.col;
         }
  
         ENDCG
         } 
     }
  
 }
 */