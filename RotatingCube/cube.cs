using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAssembly;
using WebGl;

namespace RotatingCube
{
    public class RotatingCube
    {
        protected WebGLRenderingContextBase gl;
        protected Vector4 clearColor;
        protected JSObject canvas;
        protected int canvasWidth;
        protected int canvasHeight;

        ushort[] indices;
        WebGLBuffer vertexBuffer;
        WebGLBuffer indexBuffer;
        WebGLBuffer colorBuffer;

        WebGLUniformLocation pMatrixUniform;
        WebGLUniformLocation vMatrixUniform;
        WebGLUniformLocation wMatrixUniform;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        public virtual bool EnableFullScreen => true;

        public virtual Task InitAsync(JSObject canvas, Vector4 clearColor)
        {
            this.clearColor = clearColor;
            this.canvas = canvas;

            canvasWidth = (int)canvas.GetObjectProperty("width");
            canvasHeight = (int)canvas.GetObjectProperty("height");

            var contextAttributes = new WebGLContextAttributes { Stencil = true };
            gl = new WebGL2RenderingContext(canvas, contextAttributes);
            return Task.CompletedTask;
        }

        public virtual void Draw()
        {
            gl.Enable(WebGLRenderingContextBase.DEPTH_TEST);
            gl.Viewport(0, 0, canvasWidth, canvasHeight);
            gl.ClearColor(clearColor.X, clearColor.Y, clearColor.Z, clearColor.W);
            gl.Clear(WebGLRenderingContextBase.COLOR_BUFFER_BIT);

            gl.UniformMatrix4fv(pMatrixUniform, false, projectionMatrix.ToArray());
            gl.UniformMatrix4fv(vMatrixUniform, false, viewMatrix.ToArray());
            gl.UniformMatrix4fv(wMatrixUniform, false, worldMatrix.ToArray());
            gl.DrawElements(WebGLRenderingContextBase.TRIANGLES, indices.Length, WebGLRenderingContextBase.UNSIGNED_SHORT, 0);
        }

        public virtual void Resize(int width, int height)
        {
            canvasWidth = width;
            canvasHeight = height;
        }

        public void Run()
        {
            var vertices = new float[]
            {
                -1, -1, -1,
                 1, -1, -1,
                 1,  1, -1,

                -1,  1, -1,
                -1, -1,  1,
                 1, -1,  1,

                 1,  1,  1,
                -1,  1,  1,
                -1, -1, -1,

                -1,  1, -1,
                -1,  1,  1,
                -1, -1,  1,

                 1, -1, -1,
                 1,  1, -1,
                 1,  1,  1,

                 1, -1,  1,
                -1, -1, -1,
                -1, -1,  1,

                 1, -1,  1,
                 1, -1, -1,
                -1,  1, -1,

                -1,  1,  1,
                 1,  1,  1,
                 1,  1, -1
            };
            vertexBuffer = gl.CreateArrayBuffer(vertices);

            indices = new ushort[]
            {
                 0,  1,  2,
                 0,  2,  3,

                 4,  5,  6,
                 4,  6,  7,

                 8,  9, 10,
                 8, 10, 11,

                12, 13, 14,
                12, 14, 15,

                16, 17, 18,
                16, 18, 19,

                20, 21, 22,
                20, 22, 23
            };
            indexBuffer = gl.CreateElementArrayBuffer(indices);

            var colors = new float[]
            {
                1, 0, 0,
                1, 0, 0,
                1, 0, 0,
                1, 0, 0,

                0, 1, 0,
                0, 1, 0,
                0, 1, 0,
                0, 1, 0,

                0, 0, 1,
                0, 0, 1,
                0, 0, 1,
                0, 0, 1,

                1, 1, 0,
                1, 1, 0,
                1, 1, 0,
                1, 1, 0,

                0, 1, 1,
                0, 1, 1,
                0, 1, 1,
                0, 1, 1,

                1, 1, 1,
                1, 1, 1,
                1, 1, 1,
                1, 1, 1
            };
            colorBuffer = gl.CreateArrayBuffer(colors);

            var shaderProgram = InitShaders();

            pMatrixUniform = gl.GetUniformLocation(shaderProgram, "pMatrix");
            vMatrixUniform = gl.GetUniformLocation(shaderProgram, "vMatrix");
            wMatrixUniform = gl.GetUniformLocation(shaderProgram, "wMatrix");

            gl.BindBuffer(WebGLRenderingContextBase.ARRAY_BUFFER, vertexBuffer);
            var positionAttribute = (uint)gl.GetAttribLocation(shaderProgram, "position");
            gl.VertexAttribPointer(positionAttribute, 3, WebGLRenderingContextBase.FLOAT, false, 0, 0);
            gl.EnableVertexAttribArray(positionAttribute);

            gl.BindBuffer(WebGLRenderingContextBase.ARRAY_BUFFER, colorBuffer);
            var colorAttribute = (uint)gl.GetAttribLocation(shaderProgram, "color");
            gl.VertexAttribPointer(colorAttribute, 3, WebGLRenderingContextBase.FLOAT, false, 0, 0);
            gl.EnableVertexAttribArray(colorAttribute);

            gl.BindBuffer(WebGLRenderingContextBase.ELEMENT_ARRAY_BUFFER, indexBuffer);
            worldMatrix = Matrix.Identity;
        }

        private WebGLProgram InitShaders()
        {
            return gl.InitializeShaders(
                vertexShaderCode:
                    @"attribute vec3 position;
                    attribute vec3 color;
                    uniform mat4 pMatrix;
                    uniform mat4 vMatrix;
                    uniform mat4 wMatrix;
                    varying vec3 vColor;
                    void main(void) {
                        gl_Position = pMatrix * vMatrix * wMatrix * vec4(position, 1.0);
                        vColor = color;
                    }",
                fragmentShaderCode:
                    @"precision mediump float;
                    varying vec3 vColor;
                    void main(void) {
                        gl_FragColor = vec4(vColor, 1.0);
                    }");
        }

        public void Update(double elapsedMilliseconds)
        {
            var aspectRatio = (float)canvasWidth / (float)canvasHeight;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4, aspectRatio, 0.1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(Vector3.UnitZ * 10, Vector3.Zero, Vector3.Up);
            var elapsedMillisecondsFloat = (float)elapsedMilliseconds;
            var rotation = Quaternion.CreateFromYawPitchRoll(
                elapsedMillisecondsFloat * 2 * 0.001f,
                elapsedMillisecondsFloat * 4 * 0.001f,
                elapsedMillisecondsFloat * 3 * 0.001f);
            worldMatrix *= Matrix.CreateFromQuaternion(rotation);
        }
    }
}
