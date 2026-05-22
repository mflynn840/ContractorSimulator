using ContractorSimulator.Networking;
using ContractorSimulator.Networking.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace ContractorSimulator.UI
{
    public class LobbyUIController : MonoBehaviour
    {
        private NetworkLobbyService _lobbyService;
        private NetworkSessionFlow _sessionFlow;
        private NetworkSessionConfig _sessionConfig;

        private TMP_Text _statusText;
        private TMP_Text _playerCountText;
        private TMP_InputField _addressInput;
        private Button _hostButton;
        private Button _joinButton;
        private Button _startSessionButton;
        private Button _disconnectButton;

        public void Initialize(
            NetworkLobbyService lobbyService,
            NetworkSessionFlow sessionFlow,
            NetworkSessionConfig sessionConfig)
        {
            _lobbyService = lobbyService;
            _sessionFlow = sessionFlow;
            _sessionConfig = sessionConfig;

            BuildUI();
            BindEvents();
            RefreshUI();
        }

        private void OnDestroy()
        {
            if (_lobbyService == null)
            {
                return;
            }

            _lobbyService.StatusChanged -= OnStatusChanged;
            _lobbyService.StatusMessageChanged -= OnStatusMessageChanged;
            _lobbyService.PlayerCountChanged -= OnPlayerCountChanged;
        }

        private void BuildUI()
        {
            EnsureEventSystem();

            var canvasObject = new GameObject("LobbyCanvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvasObject.AddComponent<GraphicRaycaster>();

            var panel = CreatePanel(canvasObject.transform);
            var layout = panel.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(32, 32, 32, 32);
            layout.spacing = 12;
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            CreateTitle(panel, "Contractor Simulator — Lobby");
            _statusText = CreateText(panel, "Status: Offline");
            _playerCountText = CreateText(panel, "Players: 0/2");

            _addressInput = CreateAddressInput(panel);
            _hostButton = CreateButton(panel, "Host Game", OnHostClicked);
            _joinButton = CreateButton(panel, "Join Game", OnJoinClicked);
            _startSessionButton = CreateButton(panel, "Start Session", OnStartSessionClicked);
            _disconnectButton = CreateButton(panel, "Disconnect", OnDisconnectClicked);
        }

        private void BindEvents()
        {
            if (_lobbyService == null)
            {
                return;
            }

            _lobbyService.StatusChanged += OnStatusChanged;
            _lobbyService.StatusMessageChanged += OnStatusMessageChanged;
            _lobbyService.PlayerCountChanged += OnPlayerCountChanged;
        }

        private void OnHostClicked()
        {
            _lobbyService?.StartHost();
            RefreshUI();
        }

        private void OnJoinClicked()
        {
            var address = _addressInput != null ? _addressInput.text : null;
            _lobbyService?.StartClient(address);
            RefreshUI();
        }

        private void OnStartSessionClicked()
        {
            _sessionFlow?.TryStartGameplaySession();
            RefreshUI();
        }

        private void OnDisconnectClicked()
        {
            _lobbyService?.Disconnect();
            RefreshUI();
        }

        private void OnStatusChanged(LobbyConnectionStatus _)
        {
            RefreshUI();
        }

        private void OnStatusMessageChanged(string _)
        {
            RefreshUI();
        }

        private void OnPlayerCountChanged(int _)
        {
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (_lobbyService == null)
            {
                return;
            }

            var maxPlayers = _sessionConfig != null
                ? _sessionConfig.MaxPlayers
                : NetworkAuthorityRules.MvpMaxPlayers;

            _statusText.text = $"Status: {_lobbyService.StatusMessage}";
            _playerCountText.text = $"Players: {_lobbyService.ConnectedPlayerCount}/{maxPlayers}";

            var inLobby = _lobbyService.IsInLobby;
            var isHost = _lobbyService.IsHost;

            _hostButton.interactable = !inLobby;
            _joinButton.interactable = !inLobby;
            _disconnectButton.interactable = inLobby || _lobbyService.Status == LobbyConnectionStatus.Failed;
            _startSessionButton.interactable = isHost && _lobbyService.CanStartGameplaySession();
            _addressInput.interactable = !inLobby;
        }

        private static void EnsureEventSystem()
        {
            if (FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }

        private static RectTransform CreatePanel(Transform parent)
        {
            var panelObject = new GameObject("LobbyPanel");
            panelObject.transform.SetParent(parent, false);

            var rect = panelObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(520, 420);

            var image = panelObject.AddComponent<Image>();
            image.color = new Color(0.1f, 0.12f, 0.16f, 0.92f);
            return rect;
        }

        private static TMP_Text CreateTitle(Transform parent, string text)
        {
            var title = CreateText(parent, text);
            title.fontSize = 28;
            title.fontStyle = FontStyles.Bold;
            return title;
        }

        private static TMP_Text CreateText(Transform parent, string text)
        {
            var textObject = new GameObject("Text");
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 36);

            var tmp = textObject.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = 20;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.white;
            return tmp;
        }

        private TMP_InputField CreateAddressInput(Transform parent)
        {
            var inputObject = new GameObject("AddressInput");
            inputObject.transform.SetParent(parent, false);
            var rect = inputObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 40);

            var background = inputObject.AddComponent<Image>();
            background.color = new Color(0.2f, 0.22f, 0.28f, 1f);

            var textArea = new GameObject("Text Area");
            textArea.transform.SetParent(inputObject.transform, false);
            var textAreaRect = textArea.AddComponent<RectTransform>();
            textAreaRect.anchorMin = Vector2.zero;
            textAreaRect.anchorMax = Vector2.one;
            textAreaRect.offsetMin = new Vector2(10, 6);
            textAreaRect.offsetMax = new Vector2(-10, -6);

            var placeholderObject = new GameObject("Placeholder");
            placeholderObject.transform.SetParent(textArea.transform, false);
            var placeholderRect = placeholderObject.AddComponent<RectTransform>();
            placeholderRect.anchorMin = Vector2.zero;
            placeholderRect.anchorMax = Vector2.one;
            placeholderRect.offsetMin = Vector2.zero;
            placeholderRect.offsetMax = Vector2.zero;
            var placeholder = placeholderObject.AddComponent<TextMeshProUGUI>();
            placeholder.text = "Host address (127.0.0.1)";
            placeholder.fontSize = 18;
            placeholder.color = new Color(1f, 1f, 1f, 0.4f);

            var textObject = new GameObject("Text");
            textObject.transform.SetParent(textArea.transform, false);
            var textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            var text = textObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = 18;
            text.color = Color.white;

            var input = inputObject.AddComponent<TMP_InputField>();
            input.textViewport = textAreaRect;
            input.textComponent = text;
            input.placeholder = placeholder;
            input.text = _sessionConfig != null ? _sessionConfig.ConnectionAddress : "127.0.0.1";
            return input;
        }

        private static Button CreateButton(Transform parent, string label, UnityEngine.Events.UnityAction onClick)
        {
            var buttonObject = new GameObject(label);
            buttonObject.transform.SetParent(parent, false);
            var rect = buttonObject.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 44);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.24f, 0.45f, 0.78f, 1f);

            var button = buttonObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(onClick);

            var textObject = new GameObject("Label");
            textObject.transform.SetParent(buttonObject.transform, false);
            var textRect = textObject.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            var text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = label;
            text.fontSize = 20;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;

            return button;
        }
    }
}
