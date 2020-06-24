using Unity.Burst.LowLevel;
using UnityEditor;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Unity.Burst.Editor
{
    /// <summary>
    /// Register all menu entries for burst to the Editor
    /// </summary>
    internal static class BurstMenu
    {
        private const string EnableBurstCompilationText = "Jobs/Burst/Enable Compilation";
        private const string EnableSafetyChecksTextOff = "Jobs/Burst/Safety Checks/Off";
        private const string EnableSafetyChecksTextOn = "Jobs/Burst/Safety Checks/On";
        private const string EnableSafetyChecksTextForceOn = "Jobs/Burst/Safety Checks/Force On";
        private const string ForceSynchronousCompilesText = "Jobs/Burst/Synchronous Compilation";
        private const string EnableDebugCompilationText = "Jobs/Burst/Native Debug Mode Compilation";
        private const string ShowBurstTimingsText = "Jobs/Burst/Show Timings";
        private const string BurstInspectorText = "Jobs/Burst/Open Inspector...";

        // ----------------------------------------------------------------------------------------------
        // #1 Enable Compilation
        // ----------------------------------------------------------------------------------------------
        [MenuItem(EnableBurstCompilationText, false)]
        private static void EnableBurstCompilation()
        {
            BurstEditorOptions.EnableBurstCompilation = !BurstEditorOptions.EnableBurstCompilation;
        }

        [MenuItem(EnableBurstCompilationText, true)]
        private static bool EnableBurstCompilationValidate()
        {
            Menu.SetChecked(EnableBurstCompilationText, BurstEditorOptions.EnableBurstCompilation);
#if UNITY_2019_3_OR_NEWER
            return BurstCompilerService.IsInitialized;
#else
            return BurstCompilerService.IsInitialized && (BurstEditorOptions.EnableBurstCompilation || !EditorApplication.isPlayingOrWillChangePlaymode);
#endif
        }

        // ----------------------------------------------------------------------------------------------
        // #2 Safety Checks
        // ----------------------------------------------------------------------------------------------
        [MenuItem(EnableSafetyChecksTextOff, false)]
        private static void EnableBurstSafetyChecksOff()
        {
            BurstEditorOptions.EnableBurstSafetyChecks = false;
            BurstEditorOptions.ForceEnableBurstSafetyChecks = false;
            Menu.SetChecked(EnableSafetyChecksTextOff, true);
            Menu.SetChecked(EnableSafetyChecksTextOn, false);
            Menu.SetChecked(EnableSafetyChecksTextForceOn, false);
        }

        [MenuItem(EnableSafetyChecksTextOff, true)]
        private static bool EnableBurstSafetyChecksOffValidate()
        {
            Menu.SetChecked(EnableSafetyChecksTextOff, !BurstEditorOptions.EnableBurstSafetyChecks && !BurstEditorOptions.ForceEnableBurstSafetyChecks);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        [MenuItem(EnableSafetyChecksTextOn, false)]
        private static void EnableBurstSafetyChecksOn()
        {
            BurstEditorOptions.EnableBurstSafetyChecks = true;
            BurstEditorOptions.ForceEnableBurstSafetyChecks = false;
            Menu.SetChecked(EnableSafetyChecksTextOff, false);
            Menu.SetChecked(EnableSafetyChecksTextOn, true);
            Menu.SetChecked(EnableSafetyChecksTextForceOn, false);
        }

        [MenuItem(EnableSafetyChecksTextOn, true)]
        private static bool EnableBurstSafetyChecksOnValidate()
        {
            Menu.SetChecked(EnableSafetyChecksTextOn, BurstEditorOptions.EnableBurstSafetyChecks && !BurstEditorOptions.ForceEnableBurstSafetyChecks);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        [MenuItem(EnableSafetyChecksTextForceOn, false)]
        private static void EnableBurstSafetyChecksForceOn()
        {
            BurstEditorOptions.EnableBurstSafetyChecks = true;
            BurstEditorOptions.ForceEnableBurstSafetyChecks = true;
            Menu.SetChecked(EnableSafetyChecksTextOff, false);
            Menu.SetChecked(EnableSafetyChecksTextOn, false);
            Menu.SetChecked(EnableSafetyChecksTextForceOn, true);
        }

        [MenuItem(EnableSafetyChecksTextForceOn, true)]
        private static bool EnableBurstSafetyChecksForceOnValidate()
        {
            Menu.SetChecked(EnableSafetyChecksTextForceOn, BurstEditorOptions.ForceEnableBurstSafetyChecks);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        // ----------------------------------------------------------------------------------------------
        // #3 Synchronous Compilation
        // ----------------------------------------------------------------------------------------------
        [MenuItem(ForceSynchronousCompilesText, false)]
        private static void ForceSynchronousCompiles()
        {
            BurstEditorOptions.EnableBurstCompileSynchronously = !BurstEditorOptions.EnableBurstCompileSynchronously;
        }

        [MenuItem(ForceSynchronousCompilesText, true)]
        private static bool ForceSynchronousCompilesValidate()
        {
            Menu.SetChecked(ForceSynchronousCompilesText, BurstEditorOptions.EnableBurstCompileSynchronously);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        // ----------------------------------------------------------------------------------------------
        // #4 Synchronous Compilation
        // ----------------------------------------------------------------------------------------------
        [MenuItem(EnableDebugCompilationText, false)]
        private static void EnableDebugMode()
        {
            BurstEditorOptions.EnableBurstDebug = !BurstEditorOptions.EnableBurstDebug;
        }

        [MenuItem(EnableDebugCompilationText, true)]
        private static bool EnableDebugModeValidate()
        {
            Menu.SetChecked(EnableDebugCompilationText, BurstEditorOptions.EnableBurstDebug);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        // ----------------------------------------------------------------------------------------------
        // #5 Show Timings
        // ----------------------------------------------------------------------------------------------
        [MenuItem(ShowBurstTimingsText, false)]
        private static void ShowBurstTimings()
        {
            BurstEditorOptions.EnableBurstTimings = !BurstEditorOptions.EnableBurstTimings;
        }
        
        [MenuItem(ShowBurstTimingsText, true)]
        private static bool ShowBurstTimingsValidate()
        {
            Menu.SetChecked(ShowBurstTimingsText, BurstEditorOptions.EnableBurstTimings);
            return BurstCompilerService.IsInitialized && BurstEditorOptions.EnableBurstCompilation;
        }

        // ----------------------------------------------------------------------------------------------
        // #6 Open Inspector...
        // ----------------------------------------------------------------------------------------------
        [MenuItem(BurstInspectorText, false)]
        private static void BurstInspector()
        {
            // Get existing open window or if none, make a new one:
            BurstInspectorGUI window = EditorWindow.GetWindow<BurstInspectorGUI>("Burst Inspector");
            window.Show();
        }

        [MenuItem(BurstInspectorText, true)]
        private static bool BurstInspectorValidate()
        {
            return BurstCompilerService.IsInitialized;
        }
    }
}
