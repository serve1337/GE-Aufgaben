using System;
using Fusee.Base.Common;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Engine.Core.GUI;
using Fusee.Math.Core;
using Fusee.Serialization;
using static Fusee.Engine.Core.Input;


namespace Fusee.Tutorial.Core
{

    [FuseeApplication(Name = "Tutorial Example", Description = "The official FUSEE Tutorial.")]
    public class Tutorial : RenderCanvas
    {
        private Mesh _mesh;
        private IShaderParam _alphaParam, _yRotationParam, _mouseposParam;
        private float _alpha, _yRotation;
        private float2 _mousepos;

        /* Color animation
        private float4 _color;
        private IShaderParam _colorParam;
        
        private const string _vertexShader = @"
        attribute vec3 fuVertex;
        uniform float alpha;

        void main()
        {
            float s = sin(alpha);
            float c = cos(alpha);
            gl_Position = vec4( fuVertex.x * c - fuVertex.y * s,   // The transformed x coordinate
                                fuVertex.x * s + fuVertex.y * c,   // The transformed y coordinate
                                fuVertex.z * s + fuVertex.z * c,   // z is unchanged
                                1.0);
        }";

        private const string _pixelShader = @"
            #ifdef GL_ES
                precision highp float;
            #endif
            uniform vec4 color;

            void main()
            {
                gl_FragColor = color;
            }";
        */

        private const string _vertexShader = @"
            attribute vec3 fuVertex;
            uniform float alpha, yRotation;
            varying vec3 modelpos;
            varying mat4 yRot, alphaRot;

            void main()
            {
                modelpos = fuVertex;
                float s = sin(alpha);
                float c = cos(alpha);

                float s2 = sin(yRotation);
                float c2 = cos(yRotation);

                
                alphaRot = mat4(cos(alpha), 0, sin(alpha), 0,
                            0, 1, 0, 0,
                            -sin(alpha), 0, cos(alpha), 0,
                            0, 0, 0, 1);

                yRot = mat4(1, 0, 0, 0,
                            0, cos(yRotation), -sin(yRotation), 0,
                            0, sin(yRotation), cos(yRotation), 0,
                            0, 0, 0, 1);
                gl_Position = alphaRot * yRot * vec4(fuVertex, 1);
        }";

        private const string _pixelShader = @"
            #ifdef GL_ES
                precision highp float;
            #endif
            uniform vec2 mousepos;
            varying vec3 modelpos;
            
            void main()
            {
                float distance = distance(vec4(mousepos, 0, 1), vec4(modelpos*0.5+0.5, 1));
                gl_FragColor = vec4(modelpos*0.5 + 0.5, 1) * distance;
        }";

        // Init is called on startup. 
        public override void Init()
        {
            _mesh = new Mesh
            {
                Vertices = new[]
                {
                    new float3(-0.5f, -0.75f, -1),  // Vertex 0
                    new float3(0.5f, -0.75f, -1),   // Vertex 1
                    new float3(-0.5f, 0, -1),      // Vertex 2
                    new float3(0.5f, 0, -1),       // Vertex 3
                    new float3(0.5f, 0, 0),        // Vertex 4
                    new float3(-0.5f, 0, 0),       // Vertex 5
                    new float3(0.5f, -0.75f, 0),    // Vertex 6
                    new float3(-0.5f, -0.75f, 0),   // Vertex 7
                    new float3(0, 0.5f, -0.5f),          // Vertex 8
                },
                Triangles = new ushort[]
                {
                    0, 1, 3, 3, 2, 0, // Front panel
                    1, 6, 4, 4, 3, 1, // Right panel
                    6, 7, 5, 5, 4, 6, // Back panel
                    7, 0, 2, 2, 5, 7, // Left panel
                    7, 6, 1, 1, 0, 7, // Bottom panel
                    2, 3, 8, // Front panel roof
                    3, 4, 8, // Right panel roof
                    4, 5, 8, // Back panel roof
                    5, 2, 8, // Left panel roof
                },
            };

            var shader = RC.CreateShader(_vertexShader, _pixelShader);
            RC.SetShader(shader);
            _alphaParam = RC.GetShaderParam(shader, "alpha");
            _alpha = 0;

            _yRotationParam = RC.GetShaderParam(shader, "yRotation");
            _yRotation = 0;

            _mouseposParam = RC.GetShaderParam(shader, "mousepos");
            _mousepos = new float2(0, 0);

            /* Color animation
            _colorParam = RC.GetShaderParam(shader, "color");
            _color = new float4(x: 0, y: 0, z: 0, w: 0);
            */

            // Set the clear color for the backbuffer.
            RC.ClearColor = new float4(0.1f, 0.3f, 0.2f, 1);
        }

        static float NextFloat(Random random)
        {
            var buffer = new byte[4];
            random.NextBytes(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            if (Mouse.LeftButton)
            {
                float2 speed = Mouse.Velocity;
                _alpha += speed.x*0.0001f;
                _yRotation += speed.y*0.0001f;
            }

            if (Keyboard.LeftRightAxis > 0 || Keyboard.LeftRightAxis < 0)
            {
                float speed = Keyboard.LeftRightAxis;
                _alpha += speed * 0.1f;
            }

            if (Keyboard.UpDownAxis > 0 || Keyboard.UpDownAxis < 0)
            {
                float speed = Keyboard.UpDownAxis;
                _yRotation += speed * 0.1f;
            }

            /*
            _alpha += 0.01f;
            */

            _mousepos = new float2(Mouse.Position.x / Width, Mouse.Position.y / Height);
            RC.SetShaderParam(_mouseposParam, _mousepos);
            RC.SetShaderParam(_alphaParam, _alpha);
            RC.SetShaderParam(_yRotationParam, _yRotation);

            /* Color animation
            Random random = new Random();
            _color = new float4(NextFloat(random), NextFloat(random), NextFloat(random), NextFloat(random));
            RC.SetShaderParam(_colorParam, _color);
            */

            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            RC.Render(_mesh);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rerndered farame) on the front buffer.
            Present();
        }


        // Is called when the window was resized
        public override void Resize()
        {
            // Set the new rendering area to the entire new windows size
            RC.Viewport(0, 0, Width, Height);

            // Create a new projection matrix generating undistorted images on the new aspect ratio.
            var aspectRatio = Width/(float) Height;

            // 0.25*PI Rad -> 45° Opening angle along the vertical direction. Horizontal opening angle is calculated based on the aspect ratio
            // Front clipping happens at 1 (Objects nearer than 1 world unit get clipped)
            // Back clipping happens at 2000 (Anything further away from the camera than 2000 world units gets clipped, polygons will be cut)
            var projection = float4x4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }

    }
}
 