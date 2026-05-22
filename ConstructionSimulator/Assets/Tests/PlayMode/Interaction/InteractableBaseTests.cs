using System.Collections;
using ContractorSimulator.Interaction;
using ContractorSimulator.Tests;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ContractorSimulator.Tests.PlayMode.Interaction
{
    [Category(TestCategories.Integration)]
    public class InteractableBaseTests
    {
        [UnityTest]
        public IEnumerator Interactable_CanBeCreated_AndExposesDefaults()
        {
            var go = new GameObject("Test_Interactable");
            var interactable = go.AddComponent<InteractableBase>();

            yield return null;

            Assert.That(interactable, Is.Not.Null);
            Assert.That(interactable.CanInteract, Is.True);
            Assert.That(interactable.InteractionPrompt, Is.Not.Empty);

            Object.Destroy(go);
        }
    }
}
