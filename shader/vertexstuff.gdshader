shader_type canvas_item;

void vertex() {
	// Called for every vertex the material is visible on.
	if(VERTEX_ID == 0){
		VERTEX.x += 2.;
		VERTEX.y += 2.;
	}
	
	if(VERTEX_ID == 3){
		VERTEX.x -= 2.;
		VERTEX.y += 2.;
	}
}

//void light() {
//	// Called for every pixel for every light affecting the CanvasItem.
//	// Uncomment to replace the default light processing function with this one.
//}
