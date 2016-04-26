using System.Collections.Generic;
using System.Linq;
using Fusee.Base.Core;
using Fusee.Engine.Common;
using Fusee.Engine.Core;
using Fusee.Math.Core;
using Fusee.Serialization;
using Fusee.Xene;
using static Fusee.Engine.Core.Input;

namespace Fusee.Tutorial.Core
{

    class Renderer : SceneVisitor
    {
        public RenderContext RC;
        public IShaderParam AlbedoParam;
        public IShaderParam ShininessParam;
        public float4x4 View;
        private Dictionary<MeshComponent, Mesh> _meshes = new Dictionary<MeshComponent, Mesh>();
        private CollapsingStateStack<float4x4> _model = new CollapsingStateStack<float4x4>();
        private Mesh LookupMesh(MeshComponent mc)
        {
            Mesh mesh;
            if (!_meshes.TryGetValue(mc, out mesh))
            {
                mesh = new Mesh
                {
                    Vertices = mc.Vertices,
                    Normals = mc.Normals,
                    Triangles = mc.Triangles
                };
                _meshes[mc] = mesh;
            }
            return mesh;
        }

        public Renderer(RenderContext rc)
        {
            RC = rc;
            // Initialize the shader(s)
            var vertsh = AssetStorage.Get<string>("VertexShader.vert");
            var pixsh = AssetStorage.Get<string>("PixelShader.frag");
            var shader = RC.CreateShader(vertsh, pixsh);
            RC.SetShader(shader);
            AlbedoParam = RC.GetShaderParam(shader, "albedo");
            ShininessParam = RC.GetShaderParam(shader, "shininess");
        }

        protected override void InitState()
        {
            _model.Clear();
            _model.Tos = float4x4.Identity;
        }
        protected override void PushState()
        {
            _model.Push();
        }
        protected override void PopState()
        {
            _model.Pop();
            RC.ModelView = View*_model.Tos;
        }
        [VisitMethod]
        void OnMesh(MeshComponent mesh)
        {
            RC.Render(LookupMesh(mesh));
        }
        [VisitMethod]
        void OnMaterial(MaterialComponent material)
        {
            RC.SetShaderParam(AlbedoParam, material.Diffuse.Color);
            RC.SetShaderParam(ShininessParam, material.Specular.Shininess);
        }
        [VisitMethod]
        void OnTransform(TransformComponent xform)
        {
            _model.Tos *= xform.Matrix();
            RC.ModelView = View * _model.Tos;
        }
    }


    [FuseeApplication(Name = "Tutorial Example", Description = "The official FUSEE Tutorial.")]
    public class Tutorial : RenderCanvas
    {
        private Mesh _mesh;
        private TransformComponent _wheelBigL, _wheelBigR, _wheelSmallR, _wheelSmallL, _cubeCube, _wuggy0;

        private IShaderParam _albedoParam;
        private float _alpha = 0.001f;
        private float _beta;

        private SceneOb _root;
        private SceneContainer _wuggy, _cube;
        private Renderer _renderer;

        private float _smallWheelSpeed, _bigWheelSpeed, _objectSpeed, _movZ, _movY, _zoom = 5;

        // Init is called on startup. 
        public override void Init()
        {
            // Load some meshes
            _cube = AssetStorage.Get<SceneContainer>("cube.fus");
            _cubeCube = _cube.Children.FindNodes(n => n.Name == "Cube").First().GetTransform();
            _wuggy = AssetStorage.Get<SceneContainer>("wuggy.fus");
            _wuggy0 = _wuggy.Children.FindNodes(n => n.Name == "Wuggy").First().GetTransform();
            _wheelBigL = _wuggy.Children.FindNodes(n => n.Name == "WheelBigL").First().GetTransform();
            _wheelBigR = _wuggy.Children.FindNodes(n => n.Name == "WheelBigR").First().GetTransform();
            _wheelSmallL = _wuggy.Children.FindNodes(n => n.Name == "WheelSmallL").First().GetTransform();
            _wheelSmallR = _wuggy.Children.FindNodes(n => n.Name == "WheelSmallR").First().GetTransform();
            _renderer = new Renderer(RC);

            _smallWheelSpeed = (_wheelSmallL.Scale.xyz.Length/100)*50;
            _bigWheelSpeed = (_wheelBigL.Scale.xyz.Length/100)*50;
            _objectSpeed = _wuggy0.Scale.xyz.Length * 50;

            // Set the clear color for the backbuffer
            RC.ClearColor = new float4(1, 1, 1, 1);
        }

        // RenderAFrame is called once a frame
        public override void RenderAFrame()
        {
            // Clear the backbuffer
            RC.Clear(ClearFlags.Color | ClearFlags.Depth);

            float2 speed = Mouse.Velocity + Touch.GetVelocity(TouchPoints.Touchpoint_0);
            if (Mouse.LeftButton || Touch.GetTouchActive(TouchPoints.Touchpoint_0))
            {
                _alpha -= speed.x*0.0001f;
                _beta  -= speed.y*0.0001f;
            }

            float _speed = -0.05f * Keyboard.WSAxis;
            float _speedRot = -0.05f * Keyboard.ADAxis;

            _wheelBigL.Rotation += new float3(_bigWheelSpeed * _speed, 0, 0);
            _wheelBigR.Rotation += new float3(_bigWheelSpeed * _speed, 0, 0);

            if (Keyboard.UpDownAxis > 0 && _zoom >= 5)
            {
                _zoom -= Keyboard.UpDownAxis;
            }
            if (Keyboard.UpDownAxis < 0 && _zoom < 10)
            {
                _zoom -= Keyboard.UpDownAxis;
            }

            if ((Keyboard.ADAxis > 0 || Keyboard.ADAxis < 0) && (_wheelSmallL.Rotation.y > -(3.14f/3) && _wheelSmallL.Rotation.y < (3.14f/3)))
            {
                _wheelSmallL.Rotation += new float3(_smallWheelSpeed * _speed, _speedRot, 0);
                _wheelSmallR.Rotation += new float3(_smallWheelSpeed * _speed, _speedRot, 0);

                if (Keyboard.WSAxis == 0)
                {
                    _wheelSmallL.Rotation -= new float3(_speedRot, 0, 0);
                    _wheelSmallR.Rotation -= new float3(_speedRot, 0, 0);
                }
            }
            else
            {
                if (_wheelSmallL.Rotation.y > 0)
                {
                    _wheelSmallL.Rotation.y -= 0.03f;
                    _wheelSmallR.Rotation.y -= 0.03f;
                }

                if (_wheelSmallL.Rotation.y < 0)
                {
                    
                    _wheelSmallL.Rotation.y += 0.03f;
                    _wheelSmallR.Rotation.y += 0.03f;
                }
                _wheelSmallL.Rotation += new float3(_smallWheelSpeed * _speed, 0, 0);
                _wheelSmallR.Rotation += new float3(_smallWheelSpeed * _speed, 0, 0);
            }

            // Setup matrices
            var aspectRatio = Width / (float)Height;
            RC.Projection = float4x4.CreatePerspectiveFieldOfView(fovy: 3.141592f * 0.3f, aspect: aspectRatio, zNear: 0.1f, zFar: 20);

            float4x4 view = float4x4.CreateTranslation(0, 0, _zoom) * float4x4.CreateRotationY(3.14f) * float4x4.CreateRotationX(0.65f) *
                    float4x4.CreateTranslation(0, -0.5f, 0);

            _movZ += _speed * _objectSpeed;
            _movY -= _speedRot * _objectSpeed * 0.3f;

            _cubeCube.Scale = new float3(10, 0.1f, 10);
            _wuggy0.Rotation = new float3(0, _movY, 0);
            _wuggy0.Translation = new float3(0, 0, _movZ);

            _renderer.View = view;
            _renderer.Traverse(_wuggy.Children);

            _renderer.View = view;
            _renderer.Traverse(_cube.Children);

            // Swap buffers: Show the contents of the backbuffer (containing the currently rendered frame) on the front buffer.
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
            var projection = float4x4.CreatePerspectiveFieldOfView(3.141592f * 0.25f, aspectRatio, 1, 20000);
            RC.Projection = projection;
        }

    }
}