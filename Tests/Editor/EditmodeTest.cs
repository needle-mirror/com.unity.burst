using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.TestTools;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using System.Threading;

[TestFixture]
public class EditModeTest
{
    private bool jobCompilerStatusStorage;
    private const int MaxIterations = 500;

    [SetUp]
    public void Setup()
    {
        jobCompilerStatusStorage = JobsUtility.JobCompilerEnabled;
    }

    [UnityTest]
    public IEnumerator CheckBurstJobEnabled()
    {
        JobsUtility.JobCompilerEnabled = true;
//        BurstCompiler.Options.EnableBurstCompileSynchronously = true;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        using (var jobTester = new BurstJobTester())
        {
            var result = jobTester.Calculate();
            Assert.AreNotEqual(0.0f, result);
        }
    }

#if UNITY_BURST_BUG_FUNCTION_POINTER_FIXED
    [UnityTest]
    public IEnumerator CheckBurstFunctionPointerException()
    {
        JobsUtility.JobCompilerEnabled = true;
 //       BurstCompiler.Options.EnableBurstCompileSynchronously = true;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        using (var jobTester = new BurstJobTester())
        {
            var exception = Assert.Throws<InvalidOperationException>(() => jobTester.CheckFunctionPointer());
            StringAssert.Contains("Exception was thrown from a function compiled with Burst", exception.Message);
        }
    }
#endif

    [UnityTest]
    public IEnumerator CheckBurstJobDisabled()
    {
        JobsUtility.JobCompilerEnabled = false;
 //       BurstCompiler.Options.EnableBurstCompileSynchronously = true;
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        yield return null;

        using (var jobTester = new BurstJobTester())
        {
            var result = jobTester.Calculate();
            Assert.AreEqual(0.0f, result);
        }
    }

    // Note: this test generates instabilities when running on linux that needs further investigation.
    // Running on windows and OSX only for now
    [UnityTest]
    [UnityPlatform(RuntimePlatform.OSXEditor, RuntimePlatform.WindowsEditor)]
    public IEnumerator CheckBurstAsyncJob()
    {
        JobsUtility.JobCompilerEnabled = true;
        BurstCompiler.Options.EnableBurstCompileSynchronously = false;

        var iteration = 0;
        var result = 0.0f;
        var array = new NativeArray<float>(10, Allocator.Persistent);

        while (result == 0.0f && iteration < MaxIterations)
        {
            array[0] = 0.0f;
            var job = new BurstJobTester.MyJobAsync { Result = array };
            job.Schedule().Complete();
            result = job.Result[0];
            iteration++;
            yield return null;
        }
        Debug.Log($"{iteration} frames showed before burst-compiled job executed.");
        array.Dispose();
        Assert.AreNotEqual(0.0f, result, "The test timed out. Probably async compilation is not working properly.");
        BurstCompiler.Options.EnableBurstCompileSynchronously = true;
        Thread.Sleep(10000);
    }

    [TearDown]
    public void Restore()
    {
        JobsUtility.JobCompilerEnabled = jobCompilerStatusStorage;
    }
}
