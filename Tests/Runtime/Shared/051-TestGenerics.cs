using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

namespace Burst.Compiler.IL.Tests
{
    // TODO: We should add a lot more tests for generics
    // - instance methods with generics
    // - instance methods with generics and outer generics from declaring type
    // - check generic name collisions
    // - ...etc.
    internal partial class TestGenerics
    {
        [TestCompiler]
        public static int StructNestedGenerics()
        {
            var value = new GenericStruct<float, GenericStruct<int, int>>();
            return value.FieldMixed1.Field4.FieldMixed1.Field4;
        }

        private unsafe struct DataOutput2<TType>
            where TType : struct
        {
#pragma warning disable 0649
            internal TType m_Value;
#pragma warning restore 0649
        }

        [TestCompiler]
        public static int CheckSizeOfWithGenerics()
        {
            return UnsafeUtility.SizeOf<DataOutput2<int>>() + UnsafeUtility.SizeOf<DataOutput2<DataOutput2<GenericStruct<int, float>>>>() * 10;
        }

        [TestCompiler]
        public static float TestOuterInnerGenerics()
        {
            var yoyo = new GenericStructOuter<MyValueData1, MyValueData2>.GenericStructInner
            {
                Field1 = { Value = 1.0f },
                Field2 = { Value = 11.0f }
            };
            Executor<GenericStructOuter<MyValueData1, MyValueData2>.GenericStructInner>.Execute(ref yoyo);
            return yoyo.Result;
        }

        [TestCompiler]
        public static float TestOuterInnerGenerics2()
        {
#pragma warning disable 0649
            var yoyo = new GenericStructOuter2<MyValueData1, MyValueData2>.GenericStructInner<MyValueData2>
            {
                Field1 = { Value = 1.0f },
                Field2 = { Value = 11.0f },
                Field3 = { Value = 106.0f }
            };
#pragma warning restore 0649
            Executor<GenericStructOuter2<MyValueData1, MyValueData2>.GenericStructInner<MyValueData2>>.Execute(ref yoyo);
            return yoyo.Result;
        }

        [TestCompiler]
        public static float TestActivator()
        {
            var yoyo = new MyActivator<MyValueData1>();
            var result = yoyo.Create(1.0f);
            return result.Value;
        }

        [TestCompiler]
        public static float TestActivatorIndirect()
        {
            var yoyo = new MyActivatorIndirect<MyValueData1>();
            var result = yoyo.Create(1.0f);
            return result.Value;
        }

        [TestCompiler]
        public static float TestStaticMethodGeneric()
        {
            var v1 = new MyValueData1() { Value = 10.0f };
            var v2 = new MyValueData2() { Value = 100.0f };
            var result = ComputeData<MyValueData2, MyValueData1, MyValueData2>(v1, v2);
            return result.Value;
        }

        public interface IMyActivator<T> where T : IMyData, new()
        {
            T Create(float value);
        }

        public struct MyActivator<T> : IMyActivator<T> where T : IMyData, new()
        {
            public T Create(float value)
            {
                var data = new T { Value = value + 2.0f };
                return data;
            }
        }

        public struct MyActivatorIndirect<T> : IMyActivator<T> where T : IMyData, new()
        {
            public T Create(float value)
            {
                return CreateActivator<T>(value);
            }
        }

        public interface IMyData
        {
            float Value { get; set; }
        }

        public struct MyValueData1 : IMyData
        {
            public float Value { get; set; }
        }

        public struct MyValueData2 : IMyData
        {
            public float Value { get; set; }
        }

        private struct GenericStructOuter<T1, T2> where T1 : IMyData where T2 : IMyData
        {
            public struct GenericStructInner : IJob
            {
#pragma warning disable 0649
                public T1 Field1;

                public T2 Field2;
#pragma warning restore 0649
                public float Result;

                public void Execute()
                {
                    Result = Field1.Value + Field2.Value;
                }
            }
        }

        private struct GenericStructOuter2<T1, T2> where T1 : IMyData where T2 : IMyData
        {
            public struct GenericStructInner<T3> : IJob where T3 : IMyData
            {
#pragma warning disable 0649
                public T1 Field1;

                public T2 Field2;

                public T3 Field3;

                public float Result;
#pragma warning restore 0649
                public void Execute()
                {
                    Result = Field1.Value + Field2.Value + Field3.Value;
                }
            }
        }

        private struct Executor<T> where T : IJob
        {
            public static void Execute(ref T job)
            {
                job.Execute();
            }
        }

        private struct GenericStruct<T1, T2>
        {
#pragma warning disable 0649
            public GenericSubStruct<int, T2> FieldMixed1;

            public GenericSubStruct<T1, float> FieldMixed2;
#pragma warning restore 0649
        }

        private struct GenericSubStruct<T3, T4>
        {
#pragma warning disable 0649
            public T3 Field3;

            public T4 Field4;
#pragma warning restore 0649
        }

        public interface IRotation
        {
            float Value { get; set; }
        }

        public struct SimpleRotation : IRotation
        {
            public float Value { get; set; }
        }

        public struct SimpleRotation2 : IRotation
        {
            public float Value { get; set; }
        }

        private static TNew CreateActivator<TNew>(float value) where TNew : IMyData, new()
        {
            var data = new TNew { Value = value + 5.0f };
            return data;
        }

        private static TResult ComputeData<TResult, TLeft, TRight>(TLeft left, TRight right) where TLeft : IMyData where TRight : IMyData where TResult : IMyData, new()
        {
            var result = new TResult();
            result.Value = 5.0f;
            result.Value += left.Value;
            result.Value += right.Value;
            return result;
        }

        [TestCompiler]
        public static void TestCrossConstraints()
        {
            var job = new ReproBurstError();
            job.Execute();
        }

        struct ReproBurstError : IJob
        {
#pragma warning disable 0649
            public FirstLevel<SecondLevel<int>, int> first;
            public SecondLevel<int> second;
#pragma warning restore 0649

            public void Execute()
            {
                first.First(second, 0);
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct FirstLevel<T1, T2> where T1 : struct, ISecondLevel<T2>
        {
            public void First(T1 t1, T2 t2) { t1.Second(t2); }
        }
        interface ISecondLevel<T2> { void Second(T2 x); }
        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct SecondLevel<T> : ISecondLevel<T>
        {
            public void Second(T x) { }
        }

        [TestCompiler]
        public static float TestCrossAndGenericArgumentsInGenericInterface()
        {
            var value = new CaseMixedGenerics<SimpleRotation2>.Check<CaseMixedImplem, SimpleRotation>();
            return value.Execute();
        }

        public struct CaseMixedGenerics<T1> where T1 : IRotation
        {
            public interface MyInterface<T2> where T2 : IRotation
            {
                // Here we have a test with generics coming from interface but also coming from parameters
                // through an interface method call
                float MyMethod<T>(T2 t2, T value) where T : IRotation;
            }

            public struct Check<T3, T4> where T3 : MyInterface<T4> where T4 : IRotation
            {
#pragma warning disable 0649
                private T3 t3Value;

                private T4 t4Value;
#pragma warning restore 0649

                public float Execute()
                {
                    return t3Value.MyMethod(t4Value, t4Value);
                }

                public static float Run(T1 t1, Check<T3, T4> t3t4)
                {
                    return t1.Value + t3t4.Execute();
                }
            }
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        public struct CaseMixedImplem : CaseMixedGenerics<SimpleRotation2>.MyInterface<SimpleRotation>
        {
            public float MyMethod<T>(SimpleRotation t2, T value) where T : IRotation
            {
                return t2.Value + value.Value;
            }
        }

        [TestCompiler]
        public static int TestCase_1059355()
        {
            var job = new ReproBurstError2();
            job.Execute();
            return job.Result;
        }

        [TestCompiler]
        public static void ExplicitInterfaceImplementation()
        {
            ExplicitRunner.RunJob(new ExplicitInterfaceStruct());
        }

        struct ReproBurstError2 : IJob
        {
#pragma warning disable 0649
            Simplified<BugRepro<Variant>.Foo> solver;

            public int Result;
#pragma warning restore 0649
            public void Execute()
            {
                Result = solver.Run(default(BugRepro<Variant>.Foo));
            }
        }

        struct Variant { }

        struct BugRepro<TVariant>
        {

            public struct Foo : IFoo
            {
                public void Bug() { }
            }
        }

        interface IFoo
        {
            void Bug();
        }

        [StructLayout(LayoutKind.Sequential, Size = 1)]
        struct Simplified<T>
            where T : IFoo
        {
            public int Run(T foo)
            {
                foo.Bug();
                foo.Bug();
                return 1;
            }
        }


        struct ExplicitInterfaceStruct : IJob
        {
            void IJob.Execute()
            {
            }
        }

        struct ExplicitRunner
        {
            public static void RunJob<T>(T job) where T : IJob
            {
                job.Execute();
            }
        }

        // case devirtualizer not working for a Physics Job
        [TestCompiler]
        public static int ExecutePhysicsJob()
        {
            var job = new PhysicsJob();
            job.Execute(0);
            return job.result ? 1 : 0;
        }

        public interface IQueryResult
        {
            float Fraction { get; set; }
        }

        // The output of ray cast queries
        public struct RayCastResult : IQueryResult
        {
            public float Fraction { get; set; }
            public float3 SurfaceNormal;
            public int RigidBodyIndex;
        }

        public interface ICollector<T> where T : struct, IQueryResult
        {
            float MaxFraction { get; }
            bool HasHit { get; }
            int NumHits { get; }
            void AddHit(T hit);
        }

        public struct AnyHitCollector<T> : ICollector<T> where T : struct, IQueryResult
        {
            public float MaxFraction { get; private set; }
            public bool HasHit { get; private set; }
            public int NumHits { get { return HasHit ? 1 : 0; } }
            public void AddHit(T hit) { HasHit = true; }
        }

        public struct ClosestHitCollector<T> : ICollector<T> where T : struct, IQueryResult
        {
            public float MaxFraction { get { return ClosestHit.Fraction; } }
            public bool HasHit { get; private set; }
            public int NumHits { get { return HasHit ? 1 : 0; } }
            public T ClosestHit;

            public void AddHit(T hit)
            {
                ClosestHit = hit;
                HasHit = true;
            }
        }

        public interface IRaycastLeafProcessor
        {
            // Cast a ray against a leaf node of the bounding volume hierarchy.
            void RayLeaf<T>(int leafData, ref T collector) where T : struct, ICollector<RayCastResult>;
        }

        static void castRay<T>(int data, ref T collector) where T : struct, ICollector<RayCastResult>
        {
            RayCastResult result = new RayCastResult();
            result.Fraction = 0.5f;
            collector.AddHit(result);
        }

        private struct RayLeafProcessor : IRaycastLeafProcessor
        {
            public void RayLeaf<T>(int leafData, ref T collector) where T : struct, ICollector<RayCastResult>
            {
                castRay(leafData, ref collector);
            }
        }

        static void processLeaves<T, U>(ref T processor, ref U collector) where T : struct, IRaycastLeafProcessor where U : struct, ICollector<RayCastResult>
        {
            for (int i = 0; i < 10; i++)
            {
                if (collector.MaxFraction > 0.5f)
                {
                    processor.RayLeaf(i, ref collector);
                }
            }
        }

        static void castRayMesh<T>(ref T collector) where T : struct, ICollector<RayCastResult>
        {
            RayLeafProcessor processor;
            processLeaves(ref processor, ref collector);
        }

        [BurstCompile]
        protected struct PhysicsJob : IJobParallelFor
        {
            public bool result;
            public unsafe void Execute(int index)
            {
                ClosestHitCollector<RayCastResult> collector = new ClosestHitCollector<RayCastResult>();
                castRayMesh(ref collector);
                result = collector.HasHit;
            }
        }

        [TestCompiler]
        public static float TestGenericIssueWithIJobProcessComponentData()
        {
            var jobProcess = new JobStruct_Process_DD<MyReadJob, GenericComponent<float>, Translation>();
            jobProcess.DataU0.Value = 5.0f;
            jobProcess.DataU1.Value = 22.0f;
            JobStruct_Process_DD<MyReadJob, GenericComponent<float>, Translation>.Execute(ref jobProcess);
            return jobProcess.DataU0.Value + jobProcess.DataU1.Value;
        }

        public interface IComponentData
        {

        }

        internal struct JobStruct_Process_DD<T, U0, U1>
            where T : struct, IJobProcessComponentData<U0, U1>
            where U0 : struct, IComponentData
            where U1 : struct, IComponentData
        {
            public T Data;

            public U0 DataU0;
            public U1 DataU1;

            public static unsafe void Execute(ref JobStruct_Process_DD<T, U0, U1> jobData)
            {
                jobData.Data.Execute(ref jobData.DataU0, ref jobData.DataU1);
            }
        }


        public interface IJobProcessComponentData<U0, U1>
            where U0 : struct, IComponentData
            where U1 : struct, IComponentData
        {
            void Execute(ref U0 c0, ref U1 c1);
        }

        public struct GenericComponent<T> : IComponentData
        {
            public T Value;
        }

        public struct Translation : IComponentData
        {
            public float Value;
        }

        struct MyReadJob : IJobProcessComponentData<GenericComponent<float>, Translation>
        {
            public void Execute(ref GenericComponent<float> c0, ref Translation c1)
            {
                c1.Value = c0.Value;
            }
        }

        public struct GenericTypeContainer<TType>
           where TType : struct
        {
            public TType Value;
        }

        [TestCompiler]
        public static int TestSizeOfWithGenericType()
        {
            return UnsafeUtility.SizeOf<GenericTypeContainer<int>>();
        }

        public class GenericContainerOuter<T>
            where T : struct
        {
            public struct GenericContainerInner<TType>
                where TType : struct
            {
                public TType Value;
                public T Value2;
            }
        }

        [TestCompiler]
        public static int TestSizeOfWithNestedGenericTypes()
        {
            return UnsafeUtility.SizeOf<GenericContainerOuter<long>.GenericContainerInner<int>>();
        }

        [TestCompiler]
        public static int CheckInterfaceCallsThroughGenericsOfGenerics()
        {
            var job = MyOuterStructWithGenerics<MyComponentData>.GetJob();
            job.Value1.Component.Value = 1;
            job.Value1.Component.Value = 2;

            job.Execute();

            return job.Result;
        }

        private interface IComponentDataOrdered
        {
            int Order { get; }
        }


        private struct EntityInChunkWithComponent<TComponent> where TComponent : struct, IComponentData
        {
            public TComponent Component;

            public EntityInChunkWithComponent(TComponent component)
            {
                Component = component;
            }
        }

        private struct EntityInChunkWithComponentComparer<TComponent> : IComparer<EntityInChunkWithComponent<TComponent>>
            where TComponent : unmanaged, IComponentData, IComparable<TComponent>

        {
            public int Compare(EntityInChunkWithComponent<TComponent> x, EntityInChunkWithComponent<TComponent> y)
            {
                return x.Component.CompareTo(y.Component);
            }
        }

        private struct MyOuterStructWithGenerics<TComponent>
            where TComponent : unmanaged, IComponentData, IComparable<TComponent>
        {

            public struct InnerWithComparer<T, TComparer> : IJob
                where T : struct
                where TComparer : struct, IComparer<T>
            {
                public T Value1;
#pragma warning disable 0649
                public T Value2;
#pragma warning restore 0649

                public int Result;

                public void Execute()
                {
                    var comparer = new TComparer();
                    Result = comparer.Compare(Value1, Value2);
                }
            }

            public static InnerWithComparer<EntityInChunkWithComponent<TComponent>, EntityInChunkWithComponentComparer<TComponent>> GetJob()
            {
                return new InnerWithComparer<EntityInChunkWithComponent<TComponent>, EntityInChunkWithComponentComparer<TComponent>>();
            }
        }

        private struct MyComponentData : IComponentData, IComparable<MyComponentData>
        {
            public int Value;

            public MyComponentData(int value)
            {
                Value = value;
            }

            public int CompareTo(MyComponentData other)
            {
                return Value.CompareTo(other.Value);
            }
        }

        [TestCompiler]
        public static long TestNestedGenericsWithStaticAndSameName()
        {
            return TypeIndexCache<float>.GetValue();
        }

        private class TypeIndexCache<T>
        {
            public static long GetValue()
            {
                return InnerIndex<int>.Create<TestGenerics, T>();
            }
        }

        private struct InnerIndex<T>
        {
            public static long Create<T1, T2>()
            {
                var value = BurstRuntime.GetHashCode64<T1>();
                value *= BurstRuntime.GetHashCode64<T2>();
                return value;
            }
        }

        // These tests require 2020.2 because that enabled C# 8.0.
#if BURST_INTERNAL || UNITY_2020_2_OR_NEWER
        // Set this to no-inlining so the compiler can't fold the branch away with anything other than type deduction.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int GenericResolutionBranchTrick<T>()
        {
            if (default(T) is null)
            {
                return 42;
            }
            else
            {
                return 13;
            }
        }

        [TestCompiler]
        public static int TestGenericResolutionBranchTrickInt()
        {
            return GenericResolutionBranchTrick<int>();
        }

        private struct SomeStruct { }

        [TestCompiler]
        public static int TestGenericResolutionBranchTrickStruct()
        {
            return GenericResolutionBranchTrick<SomeStruct>();
        }

        private class SomeClass { }

        [TestCompiler(ExpectCompilerException = true, ExpectedDiagnosticId = DiagnosticId.ERR_InstructionBoxNotSupported)]
        public static unsafe int TestGenericResolutionBranchTrickClass()
        {
            return GenericResolutionBranchTrick<SomeClass>();
        }
#endif
    }
}
