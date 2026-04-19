shader_type canvas_item;
render_mode unshaded, skip_vertex_transform;

uniform sampler2D screen_texture: hint_screen_texture, filter_linear_mipmap;

void fragment() {
	vec2 centered_uv = SCREEN_UV* 2.0 -1.0;

	vec3 scanlines = vec3(sin(2.0 * centered_uv.y * 180.0 - (TIME * 2.)) * .2 + 1.2);

	vec3 screen_color = textureLod(screen_texture, (centered_uv + 1.0) / 2.0, 0.4).rgb * scanlines;
	
	COLOR = vec4(screen_color, 0.45);
}