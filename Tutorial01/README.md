#Tutorial 01

##Exercise/Questions
Investigate how the vertice's coordinates relate to pixel positions within the output window.
 
 - What are the smallest and largest x- and y-values for vertices that can be displayed within the output window?
 
 - What happens to your geometry if you re-size the output window? 
 
 - What happens if you change the z-values of your vertices (currently set to 0)?
 

Understand how the `Triangles` are indices into the `Vertices` array.
 
 - Add another vertex to the `Vertices` array. 
 
 - Add another triangle (three more indices) to the `Triangles` array to display a rectangle using four entries in `Vertices` and six entries in `Triangles`.
 
 - What happens if you change the order of the indices in the `Triangles` array? Try to explain your observation.
 
 
Understand the concept of "the current Shader".

 - Add one more geometry `Mesh` and another pixel shader string (setting a different color).
 
 - Compile another shader using the new pixel shader (and the exisiting vertex shader). Store both shaders (resulting from the two `RC.CreateShader` calls) in fields rather than in local variables. 
 
 - Within `RenderAFrame` render each of the two meshes with a different shader (call `RC.SetShader` before `RC.Render`).
 
  
 

 
