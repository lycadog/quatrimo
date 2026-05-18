shader_type canvas_item;

uniform sampler2D screen_texture: hint_screen_texture, filter_linear_mipmap;

void fragment() {
	
	
	float offset = 1.0;
	
	float xMultiplier = (abs(UV.y * 2.0 - 1.0) - 0.3) * 0.006;

	if(UV.x	> 0.5){
		xMultiplier = -xMultiplier;
	}
		
	vec2 distortedUV = vec2(SCREEN_UV.x * (1.0 - xMultiplier), SCREEN_UV.y);
	
	vec3 screen_color = textureLod(screen_texture, (distortedUV), 0.0).rgb;
	COLOR = vec4(screen_color, 1.0);
	
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
