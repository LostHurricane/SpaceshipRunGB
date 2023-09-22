using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityRandom = UnityEngine.Random;

namespace SpaceshipRunGB.CustomRP
{
    [CreateAssetMenu(menuName = "Rendering/SpaceRunPipelineRenderAsset")]
    public class SpaceRunPipelineRenderAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new SpaceRunPipelineRender();
        }
    }
}