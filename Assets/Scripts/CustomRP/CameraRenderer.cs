using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;
using UnityEngine.Rendering;
using Palmmedia.ReportGenerator.Core;
using UnityEditor.Rendering.LookDev;
using System.Security.Cryptography;

namespace SpaceshipRunGB.CustomRP
{
	public partial class CameraRenderer
	{
        private const string bufferName = "Camera Render";
        private readonly CommandBuffer _commandBuffer = new CommandBuffer { name = bufferName };
        private ScriptableRenderContext _context;
        private Camera _camera;
        private CullingResults _cullingResult;
        private static readonly List<ShaderTagId> drawingShaderTagIds = new List<ShaderTagId> 
        {
            new ShaderTagId("SRPDefaultUnlit"),
        };


        public void Render(ScriptableRenderContext context, Camera camera)
        {
            _camera = camera;
            _context = context;

            if (!Cull(out var parameters))
            {
                return;
            }

            Settings(parameters);
            DrawVisible();
            DrawUI();
            DrawUnsupportedShaders();
            DrawGizmos();
            Submit();

        }


        private void Settings(ScriptableCullingParameters parameters)
        {
            _cullingResult = _context.Cull(ref parameters);
            _context.SetupCameraProperties(_camera);
            _commandBuffer.ClearRenderTarget(true, true, Color.clear);
            _commandBuffer.BeginSample(bufferName);

            ExecuteCommandBuffer();
        }

        private void DrawVisible()
        {
            var drawingSettings = CreateDrawingSettings(drawingShaderTagIds, SortingCriteria.CommonOpaque, out var sortingSettings);
            var filteringSettings = new FilteringSettings(RenderQueueRange.all);
            _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);

            _context.DrawSkybox(_camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSettings.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;
            _context.DrawRenderers(_cullingResult, ref drawingSettings, ref filteringSettings);
        }

        private void Submit()
        {
            _commandBuffer.EndSample(bufferName);
            ExecuteCommandBuffer();
            _context.Submit();

        }

        private void ExecuteCommandBuffer()
        {
            _context.ExecuteCommandBuffer(_commandBuffer);
            _commandBuffer.Clear();
        }

        private bool Cull(out ScriptableCullingParameters parameters)
        {
            return _camera.TryGetCullingParameters(out parameters);
        }

        private DrawingSettings CreateDrawingSettings(List<ShaderTagId> shaderTags,
            SortingCriteria sortingCriteria, out SortingSettings sortingSettings)
        {
            sortingSettings = new SortingSettings(_camera)
            {
                criteria = sortingCriteria,
            };
            var drawingSettings = new DrawingSettings(shaderTags[0], sortingSettings);
            for (var i = 1; i < shaderTags.Count; i++)
            {
                drawingSettings.SetShaderPassName(i, shaderTags[i]);
            }
            return drawingSettings;
        }

    }
}