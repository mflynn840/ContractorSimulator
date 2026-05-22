using ContractorSimulator.SaveSystem.Data;
using ContractorSimulator.Tests;
using NUnit.Framework;
using UnityEngine;

namespace ContractorSimulator.Tests.EditMode.SaveSystem
{
    [Category(TestCategories.Unit)]
    public class Vector3SerializableTests
    {
        [Test]
        public void FromUnityVector_CopiesComponents()
        {
            var source = new Vector3(4f, 5f, 6f);
            var serializable = Vector3Serializable.FromUnityVector(source);

            Assert.That(serializable.X, Is.EqualTo(4f));
            Assert.That(serializable.Y, Is.EqualTo(5f));
            Assert.That(serializable.Z, Is.EqualTo(6f));
        }

        [Test]
        public void ToUnityVector_RestoresComponents()
        {
            var serializable = new Vector3Serializable(7f, 8f, 9f);
            var vector = serializable.ToUnityVector();

            Assert.That(vector.x, Is.EqualTo(7f));
            Assert.That(vector.y, Is.EqualTo(8f));
            Assert.That(vector.z, Is.EqualTo(9f));
        }

        [Test]
        public void RoundTrip_PreservesVector()
        {
            var original = new Vector3(-1.5f, 0f, 12.25f);
            var roundTripped = Vector3Serializable.FromUnityVector(original).ToUnityVector();

            Assert.That(roundTripped, Is.EqualTo(original));
        }
    }
}
