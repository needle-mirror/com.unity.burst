using System;
using System.Diagnostics;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.TestTools;

#if UNITY_2019_3_OR_NEWER
namespace ExceptionsFromBurstJobs
{
    class ManagedExceptionsBurstJobs
    {
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewArgumentException()
        {
            throw new ArgumentException("A");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowArgumentExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewArgumentException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowArgumentException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("ArgumentException: A"));

            var jobData = new ThrowArgumentExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewArgumentNullException()
        {
            throw new ArgumentNullException("N");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowArgumentNullExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewArgumentNullException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowArgumentNullException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("System.ArgumentNullException: N"));

            var jobData = new ThrowArgumentNullExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewNullReferenceException()
        {
            throw new NullReferenceException("N");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowNullReferenceExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewNullReferenceException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowNullReferenceException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("NullReferenceException: N"));

            var jobData = new ThrowNullReferenceExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewInvalidOperationException()
        {
            throw new InvalidOperationException("IO");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowInvalidOperationExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewInvalidOperationException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowInvalidOperationException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("InvalidOperationException: IO"));

            var jobData = new ThrowInvalidOperationExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewNotSupportedException()
        {
            throw new NotSupportedException("NS");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowNotSupportedExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewNotSupportedException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowNotSupportedException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("NotSupportedException: NS"));

            var jobData = new ThrowNotSupportedExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewUnityException()
        {
            throw new UnityException("UE");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowUnityExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewUnityException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowUnityException()
        {
            LogAssert.Expect(LogType.Exception, new Regex("UnityException: UE"));

            var jobData = new ThrowUnityExceptionJob();
            jobData.Run();
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        private static void ThrowNewIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException("IOOR");
        }

        [BurstCompile(CompileSynchronously = true)]
        struct ThrowIndexOutOfRangeExceptionJob : IJob
        {
            public void Execute()
            {
                ThrowNewIndexOutOfRangeException();
            }
        }

        [Test]
        [UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.OSXEditor, RuntimePlatform.LinuxEditor)]
        [Description("Requires ENABLE_UNITY_COLLECTIONS_CHECKS which is currently only enabled in the Editor")]
        public void ThrowIndexOutOfRange()
        {
            LogAssert.Expect(LogType.Exception, new Regex("IndexOutOfRangeException: IOOR"));

            var jobData = new ThrowIndexOutOfRangeExceptionJob();
            jobData.Run();
        }
    }
}
#endif // #if UNITY_2019_3_OR_NEWER
