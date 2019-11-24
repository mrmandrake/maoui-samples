﻿using System;
using WebAssembly;
using WebGl;

namespace RotatingCube
{
    public class Program
    {
        const int CanvasWidth = 640;
        const int CanvasHeight = 480;
        static readonly Vector4 CanvasColor = new Vector4(255, 0, 255, 255);

        static ISample[] samples;

        static Action<double> loop = new Action<double>(Loop);
        static double previousMilliseconds;

        static JSObject window;

        private static string fullscreenDivCanvasName;
        private static string fullscreenCanvasName;

        private static void Main()
        {
            if (!WebGL2RenderingContextBase.IsSupported)
            {
                HtmlHelper.AddParagraph("We are sorry, but your browser does not seem to support WebGL2.");
                HtmlHelper.AddParagraph("See the <a href=\"https://github.com/WaveEngine/WebGL.NET\">GitHub repo</a>.");
                return;
            }

            var divCanvasName = $"div_canvas";
            var canvasName = $"canvas";
            using (var canvas = HtmlHelper.AddCanvas(divCanvasName, canvasName, CanvasWidth, CanvasHeight))
            {
                await sample.InitAsync(canvas, CanvasColor);
                sample.Run();

                if (sample.EnableFullScreen)
                {
                    var fullscreenButtonName = $"fullscreen_{sampleName}";
                    HtmlHelper.AddButton(fullscreenButtonName, "Enter fullscreen");
                    AddFullScreenHandler(sample, fullscreenButtonName, divCanvasName, canvasName);
                }
            }
        }

        private static void AddEnterFullScreenHandler()
        {
            using (var document = (JSObject)Runtime.GetGlobalObject("document"))
            {
                document.Invoke("addEventListener", "fullscreenchange", new Action<JSObject>((o) =>
                {
                    using (var d = (JSObject)Runtime.GetGlobalObject("document"))
                    {
                        var fullscreenElement = (JSObject)d.GetObjectProperty("fullscreenElement");
                        var divCanvasObject = (JSObject)d.Invoke("getElementById", fullscreenDivCanvasName);
                        var canvasObject = (JSObject)d.Invoke("getElementById", fullscreenCanvasName);

                        if (fullscreenElement != null)
                        {
                            var width = (int)divCanvasObject.GetObjectProperty("clientWidth");
                            var height = (int)divCanvasObject.GetObjectProperty("clientHeight");

                            SetNewCanvasSize(canvasObject, width, height);
                            fullscreenSample.Resize(width, height);
                        }
                        else
                        {
                            SetNewCanvasSize(canvasObject, CanvasWidth, CanvasHeight);
                            fullscreenSample.Resize(CanvasWidth, CanvasHeight);
                        }
                    }

                    o.Dispose();
                }), false);
            }
        }

        static void Loop(double milliseconds)
        {
            var elapsedMilliseconds = milliseconds - previousMilliseconds;
            previousMilliseconds = milliseconds;

            foreach (var item in samples)
            {
                item.Update(elapsedMilliseconds);
                item.Draw();
            }

            RequestAnimationFrame();
        }

        static void RequestAnimationFrame()
        {
            if (window == null)
                window = (JSObject)Runtime.GetGlobalObject();

            window.Invoke("requestAnimationFrame", loop);
        }

        static void AddGenerationStamp()
        {
            var buildDate = StampHelper.GetBuildDate(Assembly.GetExecutingAssembly());
            HtmlHelper.AddParagraph($"Generated on {buildDate.ToString()} ({buildDate.Humanize()})");

            var commitHash = StampHelper.GetCommitHash(Assembly.GetExecutingAssembly());
            if (!string.IsNullOrEmpty(commitHash))
            {
                HtmlHelper.AddParagraph($"From git commit: {commitHash}");
            }
        }
    }
}
