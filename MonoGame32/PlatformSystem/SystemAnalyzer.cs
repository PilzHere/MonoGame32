using System;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame32.PlatformSystem
{
    public static class SystemAnalyzer
    {
        private static GraphicsDevice _graphicsDevice;
        private static int _numberOfAdapters;
        private static string _gpuName;

        private static OperatingSystem _operatingSystem;

        private static bool _architecture;

        private static Version _dotNetVersion;
        //private static PlatformID _platformId;

        private static double _processMemoryInGarbageCollector;
        private static double _processMemoryInUseMax;
        private static double _processMemoryUsedPercent;

        public static double ProcessMemoryUsedPercent => _processMemoryUsedPercent;

        public static double ProcessMemoryInUseMax => _processMemoryInUseMax;

        public static double ProcessMemoryInGarbageCollector => _processMemoryInGarbageCollector;

        /// <summary>
        /// Should be called AFTER game has initialized.
        /// </summary>
        /// <param name="graphicsDevice"></param>
        public static void Init(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;

            _numberOfAdapters = GraphicsAdapter.Adapters.Count;
            _gpuName = GraphicsAdapter.DefaultAdapter.Description;

            _operatingSystem = Environment.OSVersion;

            _architecture = Environment.Is64BitOperatingSystem;

            _dotNetVersion = Environment.Version;


            Console.WriteLine("-System-");
            Console.Write("OS: " + _operatingSystem.VersionString);
            Console.WriteLine(_architecture ? " 64 bit" : " 32 bit");
            Console.WriteLine("dotNET: " + _dotNetVersion);
            Console.WriteLine("GPU: " + _gpuName);


            Console.WriteLine("CPU: Logical cores: " + Environment.ProcessorCount);

            //Console.WriteLine("PageSize: " + GameMath.GameMath.BytesToMegabytes(Environment.SystemPageSize) + " MiB");
            //Console.WriteLine("2" + Environment.MachineName);
            //Console.WriteLine("WorkingSet: " + GameMath.GameMath.BytesToMegabytes(Environment.WorkingSet) + " MiB");

            // TODO: Platform specific code?
            /*switch (_operatingSystem.Platform)
            {
                case PlatformID.Win32NT:
                    Console.WriteLine("Running on " + _operatingSystem.VersionString);
                    break;
                case PlatformID.Unix:
                    Console.WriteLine("Running on " + _operatingSystem.VersionString);
                    break;
                case PlatformID.MacOSX:
                    Console.WriteLine("Running on " + _operatingSystem.VersionString);
                    break;
            }*/
        }

        private static float _newUpdateTime = 0;
        private static float _newUpdateTimeCounter = 0;
        
        /// <summary>
        /// TODO: Not sure if this is correct...
        /// </summary>
        public static void UpdateMemoryUsage(float dt)
        {
            //Console.WriteLine("WorkingSet: " + Math.Floor(GameMath.GameMath.BytesToMegabytes(Environment.WorkingSet)) + " MiB");
            //Console.WriteLine(GameMath.GameMath.BytesToMegabytes(Process.GetCurrentProcess().PrivateMemorySize64) + " MiB");
            //Console.WriteLine(GameMath.GameMath.BytesToMegabytes(Process.GetCurrentProcess().WorkingSet64) + " MiB");

            /*Console.WriteLine(Math.Floor(GameMath.GameMath.BytesToMegabytes(GC.GetTotalMemory(false))) + " / " +
                              Math.Floor(GameMath.GameMath.BytesToMegabytes(Environment.WorkingSet)) +
                              " MiB"); // seems nice
            */

            _newUpdateTimeCounter += dt;

            if (_newUpdateTimeCounter >= _newUpdateTime)
            {
                Process proc = Process.GetCurrentProcess();
                _processMemoryInUseMax = Math.Floor(GameMath.GameMath.BytesToMegabytes(proc.PrivateMemorySize64));
                proc.Dispose();

                _processMemoryInGarbageCollector = Math.Floor(GameMath.GameMath.BytesToMegabytes(GC.GetTotalMemory(false)));
                _newUpdateTime += 1; // Add one second.
                //Console.WriteLine("NEW TIME RAM MEMORY");

                _processMemoryUsedPercent = (_processMemoryInGarbageCollector / _processMemoryInUseMax ) * 100;
            }
            
            /*_processMemoryUsed2 = 0;
            Process proc = Process.GetCurrentProcess();
            _processMemoryUsed2 = proc.PrivateMemorySize64 / (1024*1024);
            proc.Dispose();

            _processMemoryUsed = Math.Floor(GameMath.GameMath.BytesToMegabytes(GC.GetTotalMemory(false)));
            _processMemoryAllocated = Math.Floor(GameMath.GameMath.BytesToMegabytes(Environment.WorkingSet)); // old*/


            //Console.WriteLine("frags: " + GameMath.GameMath.BytesToMegabytes(GC.GetGCMemoryInfo().FragmentedBytes));
            //Console.WriteLine("heap: " + GameMath.GameMath.BytesToMegabytes(GC.GetGCMemoryInfo().HeapSizeBytes));
            //Console.WriteLine("loaded: " + GameMath.GameMath.BytesToMegabytes(GC.GetGCMemoryInfo().MemoryLoadBytes));
            //Console.WriteLine("total: " + GameMath.GameMath.BytesToMegabytes(GC.GetGCMemoryInfo().TotalAvailableMemoryBytes));
            //Console.WriteLine("high: " + GameMath.GameMath.BytesToMegabytes(GC.GetGCMemoryInfo().HighMemoryLoadThresholdBytes));
        }
    }
}