using UnityEngine;
using TMPro;

namespace ContractorSimulator.Interaction
{
    [RequireComponent(typeof(CanvasGroup))]
    public class InteractionPromptUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text promptText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private string defaultPrompt = "";

        private void Awake()
        {
            if (promptText == null)
            {
                promptText = GetComponentInChildren<TMP_Text>();
            }

            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
            }

            HidePrompt();
        }

        public void ShowPrompt(string prompt)
        {
            if (promptText == null)
                return;

            promptText.text = string.IsNullOrWhiteSpace(prompt) ? defaultPrompt : prompt;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                promptText.enabled = true;
            }
        }

        public void HidePrompt()
        {
            if (promptText == null)
                return;

            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
            else
            {
                promptText.enabled = false;
            }
        }
    }
}
