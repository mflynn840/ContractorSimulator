using ContractorSimulator.SaveSystem;
using ContractorSimulator.SaveSystem.Data;
using ContractorSimulator.Tests;
using NUnit.Framework;

namespace ContractorSimulator.Tests.EditMode.SaveSystem
{
    [Category(TestCategories.Unit)]
    public class SaveDataSerializerTests
    {
        [Test]
        public void Serialize_ThenDeserialize_RoundTripsSaveData()
        {
            var original = new SaveData
            {
                Credits = 1250,
                LastScene = "TestScene",
                MasterVolume = 0.5f,
                Player = new PlayerSaveData { PlayerId = "player-1" }
            };
            original.Player.LastPosition = new Vector3Serializable(1f, 2f, 3f);

            var json = SaveDataSerializer.Serialize(original);
            var restored = SaveDataSerializer.Deserialize<SaveData>(json);

            Assert.That(restored, Is.Not.Null);
            Assert.That(restored.Credits, Is.EqualTo(1250));
            Assert.That(restored.LastScene, Is.EqualTo("TestScene"));
            Assert.That(restored.MasterVolume, Is.EqualTo(0.5f).Within(0.001f));
            Assert.That(restored.Player.PlayerId, Is.EqualTo("player-1"));
            Assert.That(restored.Player.LastPosition.X, Is.EqualTo(1f).Within(0.001f));
        }

        [Test]
        public void Deserialize_EmptyJson_ReturnsDefault()
        {
            var result = SaveDataSerializer.Deserialize<SaveData>(string.Empty);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Deserialize_WhitespaceJson_ReturnsDefault()
        {
            var result = SaveDataSerializer.Deserialize<SaveData>("   ");
            Assert.That(result, Is.Null);
        }
    }
}
