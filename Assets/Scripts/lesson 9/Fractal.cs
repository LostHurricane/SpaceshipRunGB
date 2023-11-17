using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


using static Unity.Mathematics.math;
using float4x4 = Unity.Mathematics.float4x4;
using quaternion = Unity.Mathematics.quaternion;


public class Fractal : MonoBehaviour
{
    private static readonly float3[] _directions =
    {
        up(),
        left(),
        right(),
        forward(),
        back()
    };


    private static readonly quaternion[] _rotations =
    {
        quaternion.identity,
        quaternion.RotateZ(.5f * PI),
        quaternion.RotateZ(-.5f * PI),
        quaternion.RotateX(.5f * PI),
        quaternion.RotateX(-.5f * PI),
    };









    private struct FractalPart
    {
        public Vector3 Direction;
        public Quaternion Rotation;
        public Vector3 WorldPosition;
        public Quaternion WorldRotation;
        public float SpinAngle;
    }

    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    [SerializeField, Range(1, 8)] private int _depth = 4;
    [SerializeField, Range(0, 1)] private float _speedRotation = .125f;
    private const float _positionOffset = 10f;
    private const float _scaleBias = .5f;
    private const int _childCount = 5;
    private FractalPart[][] _parts;
    private Matrix4x4[][] _matrices;
    private ComputeBuffer[] _matricesBuffers;
    private static readonly int _matricesId = Shader.PropertyToID("_Matrices");
    private static MaterialPropertyBlock _propertyBlock;




    private void Update()
    {
        var spinAngelDelta = _speedRotation * PI * Time.deltaTime;
        var rootPart = _parts[0][0];
        rootPart.SpinAngle += spinAngelDelta;
        var deltaRotation = Quaternion.Euler(.0f, rootPart.SpinAngle, .0f);
        rootPart.WorldRotation = rootPart.Rotation * deltaRotation;
        _parts[0][0] = rootPart;
        _matrices[0][0] = Matrix4x4.TRS(rootPart.WorldPosition,
        rootPart.WorldRotation, Vector3.one);
        var scale = 0.5f;
        for (var li = 1; li < _parts.Length; li++)
        {
            scale *= _scaleBias;
            var parentParts = _parts[li - 1];
            var levelParts = _parts[li];
            var levelMatrices = _matrices[li];
            for (var fpi = 0; fpi < levelParts.Length; fpi++)
            {
                Debug.Log($"{fpi}");
                var parent = parentParts[fpi / _childCount];
                var part = levelParts[fpi];
                part.SpinAngle += spinAngelDelta;
                deltaRotation = Quaternion.Euler(.0f, part.SpinAngle, .0f);
                part.WorldRotation = parent.WorldRotation * part.Rotation *
                deltaRotation;
                part.WorldPosition = parent.WorldPosition +
                parent.WorldRotation * (_positionOffset
                * scale * part.Direction);
                levelParts[fpi] = part;
                levelMatrices[fpi] = Matrix4x4.TRS(part.WorldPosition,
                part.WorldRotation, scale * Vector3.one);
            }
        }
        var bounds = new Bounds(rootPart.WorldPosition, 3f * Vector3.one);
        for (var i = 0; i < _matricesBuffers.Length; i++)
        {
            var buffer = _matricesBuffers[i];
            buffer.SetData(_matrices[i]);
            _propertyBlock.SetBuffer(_matricesId, buffer);
            _material.SetBuffer(_matricesId, buffer);
            Graphics.DrawMeshInstancedProcedural(_mesh, 0, _material, bounds,
            buffer.count, _propertyBlock);
        }
    }


    private void OnEnable()
    {
        _parts = new FractalPart[_depth][];
        _matrices = new Matrix4x4[_depth][];
        _matricesBuffers = new ComputeBuffer[_depth];
        var stride = 16 * 4;
        for (int i = 0, length = 1; i < _parts.Length; i++, length *=
        _childCount)
        {
            _parts[i] = new FractalPart[length];
            _matrices[i] = new Matrix4x4[length];
            _matricesBuffers[i] = new ComputeBuffer(length, stride);
        }
        _parts[0][0] = CreatePart(0);
        for (var li = 1; li < _parts.Length; li++)
        {
            var levelParts = _parts[li];
            for (var fpi = 0; fpi < levelParts.Length; fpi += _childCount)
            {
                for (var ci = 0; ci < _childCount; ci++)
                {
                    levelParts[fpi + ci] = CreatePart(ci);
                }
            }

        }
        _propertyBlock ??= new MaterialPropertyBlock();
    }


    private void OnDisable()
    {
        for (var i = 0; i < _matricesBuffers.Length; i++)
        {
            _matricesBuffers[i].Release();
        }
        _parts = null;
        _matrices = null;
        _matricesBuffers = null;
    }

    private void OnValidate()
    {
        if (_parts is null || !enabled)
        {
            return;
        }
        OnDisable();
        OnEnable();
    }

    private FractalPart CreatePart(int childIndex) => new FractalPart
    {
        Direction = _directions[childIndex],
        Rotation = _rotations[childIndex],
    };

}