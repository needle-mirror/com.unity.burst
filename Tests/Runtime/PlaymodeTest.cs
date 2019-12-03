using System.Collections;
using NUnit.Framework;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine.TestTools;

[TestFixture]

public class PlaymodeTest
{
    private bool _jobCompilerStatusStorage;
    private bool _burstSynchronousCompilationState;

    [SetUp]
    public void Setup()
    {
        _jobCompilerStatusStorage = JobsUtility.JobCompilerEnabled;
        _burstSynchronousCompilationState = BurstCompiler.Options.EnableBurstCompileSynchronously;
        BurstCompiler.Options.EnableBurstCompileSynchronously = true;
    }

    [UnityTest]
    public IEnumerator CheckBurstJobEnabled()
    {
        JobsUtility.JobCompilerEnabled = true;

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

    [UnityTest]
    public IEnumerator CheckBurstJobDisabled()
    {
        JobsUtility.JobCompilerEnabled = false;

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

    [TearDown]
    public void Restore()
    {
        JobsUtility.JobCompilerEnabled = _jobCompilerStatusStorage;
        BurstCompiler.Options.EnableBurstCompileSynchronously = _burstSynchronousCompilationState;
    }
}
