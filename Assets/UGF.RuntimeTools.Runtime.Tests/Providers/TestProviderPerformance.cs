using NUnit.Framework;
using UGF.RuntimeTools.Runtime.Providers;
using Unity.PerformanceTesting;

namespace UGF.RuntimeTools.Runtime.Tests.Providers
{
    public class TestProviderPerformance
    {
        [Test, Performance]
        public void AddRefs()
        {
            var provider = new Provider<string, string>();

            Measure.Method(() => provider.Add("0", "0")).SetUp(() => provider.Clear()).WarmupCount(1).MeasurementCount(10).GC().Run();
        }

        [Test, Performance]
        public void AddStructs()
        {
            var provider = new Provider<int, int>();

            Measure.Method(() => provider.Add(0, 0)).SetUp(() => provider.Clear()).WarmupCount(1).MeasurementCount(10).GC().Run();
        }
    }
}
