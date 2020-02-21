﻿using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;
using Beans.Unity.Collections;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace Deform
{
	[ExecuteAlways]
	[Deformer (Name = "Lathe Displace", Description = "Moves vertices away from axis based on distance along curve", Type = typeof (LatheDisplaceDeformer), XRotation = -90f)]
    [HelpURL("https://github.com/keenanwoodall/Deform/wiki/LatheDisplaceDeformer")]
    public class LatheDisplaceDeformer : Deformer, IFactor
	{
		public float Factor
		{
			get => factor;
			set => factor = value;
		}
		public float Bias
		{
			get => bias;
			set => bias = value;
		}
		public float Offset
		{
			get => offset;
			set => offset = value;
		}
		public AnimationCurve Curve
		{
			get => curve;
			set => curve = value;
		}
		public Transform Axis
		{
			get
			{
				if (axis == null)
					axis = transform;
				return axis;
			}
			set => axis = value;
		}

		[SerializeField, HideInInspector] private float factor = 1f;
		[SerializeField, HideInInspector] private float bias = 0f;
		[SerializeField, HideInInspector] private float offset = 0f;
		[SerializeField, HideInInspector] private AnimationCurve curve;
		[SerializeField, HideInInspector] private Transform axis;

		private JobHandle combinedHandle;
		private NativeCurve nativeCurve;

		public override DataFlags DataFlags => DataFlags.Vertices;

		private void OnDisable ()
		{
			combinedHandle.Complete ();
			if (nativeCurve.IsCreated)
				nativeCurve.Dispose ();
		}

		public override void PreProcess ()
		{
			if (curve != null)
				nativeCurve.Update (curve, 32);
		}

		public override JobHandle Process (MeshData data, JobHandle dependency = default)
		{
			if (!nativeCurve.IsCreated || curve == null)
				return dependency;

			var meshToAxis = DeformerUtils.GetMeshToAxisSpace (Axis, data.Target.GetTransform ());

			var newHandle = new LatheDisplaceJob
			{
				factor = Factor,
				bias = Bias,
				offset = Offset,
				meshToAxis = meshToAxis,
				axisToMesh = meshToAxis.inverse,
				curve = nativeCurve,
				vertices = data.DynamicNative.VertexBuffer
			}.Schedule (data.Length, 128, dependency);

			combinedHandle = JobHandle.CombineDependencies (combinedHandle, newHandle);

			return newHandle;
		}

		[BurstCompile (CompileSynchronously = COMPILE_SYNCHRONOUSLY)]
		public struct LatheDisplaceJob : IJobParallelFor
		{
			public float factor;
			public float bias;
			public float offset;
			public float4x4 meshToAxis;
			public float4x4 axisToMesh;
			[ReadOnly]
			public NativeCurve curve;
			public NativeArray<float3> vertices;

			public void Execute (int index)
			{
				var point = mul (meshToAxis, float4 (vertices[index], 1f));

				point.xy *= bias + (curve.Evaluate (point.z + offset) * factor);

				vertices[index] = mul (axisToMesh, point).xyz;
			}
		}
	}
}