#Tutorial 01

##Exercise/Questions
Investigate how the vertice's coordinates relate to pixel positions within the output window.
 
 - What are the smallest and largest x- and y-values for vertices that can be displayed within the output window?
 ```C#
 Mögliche X- und Y-Werte sind zwischen -1 bis 1
 ```
 - What happens to your geometry if you re-size the output window? 
 ```C#
 Die Geometry ist relativ zur Fenstergröße. D.h. das die Mitte des Fenster immer den Koordinaten 0 entspricht.
 ```
 - What happens if you change the z-values of your vertices (currently set to 0)?
 ```C#
Da noch keine Bewegung sondern nur eine Draufsicht verfügbar ist, ist keine Sichtbare änderung zu sehen. Da es sich hier um eine 3D Ansicht handelt, besteht das Koordinatensystem aus 3 Richtungen: x, y, z und würde aus dieser Sicht die Entfernung des Objektes definieren.
 ```

Understand how the `Triangles` are indices into the `Vertices` array.
 - What happens if you change the order of the indices in the `Triangles` array? Try to explain your observation.
 ```C#
Die Ausgabereihenfolge kann dadurch bestimmt werden.
 ``` 
