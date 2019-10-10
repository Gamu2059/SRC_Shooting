Shader "Custom/CollisionRender"
{
	SubShader{
	  Pass {
		ZWrite Off
		Cull Off
		BindChannels {
		  Bind "vertex", vertex Bind "color", color
		}
	  }
	}
}
