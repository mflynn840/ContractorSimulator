using ContractorSimulator.SaveSystem;
using ContractorSimulator.SaveSystem.Data;
using ContractorSimulator.Tests;
using NUnit.Framework;
using Unity.PerformanceTesting;

namespace ContractorSimulator.Tests.Performance
{
    [Category(TestCategories.Performance)]
    public class SaveSerializationPerformanceTests
    {
        [Test, Performance]
        public void Serialize_SaveData_MeetsBudget()
        {
            var save = new SaveData
            {
                Credits = 10_000,
                LastScene = "StarterHouse",
                Player = new PlayerSaveData { PlayerId = "perf-test-player" }
            };

            Measure.Method(() => SaveDataSerializer.Serialize(save))
                .WarmupCount(5)
                .MeasurementCount(20)
                .IterationsPerMeasurement(10)
                .Run();
        }
    }
}
