using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.CustomRP
{
    public class SpaceRunPipelineRender : RenderPipeline
    {
        private CameraRenderer _cameraRenderer = new CameraRenderer();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            CamerasRender(context, cameras);
        }

        private void CamerasRender(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _cameraRenderer.Render(context, camera);
            }

        }
    }
}