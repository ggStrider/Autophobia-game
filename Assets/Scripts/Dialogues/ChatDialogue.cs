using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

using System;
using System.Collections;
using System.Collections.Generic;

using Autophobia.Model;

namespace Autophobia.Dialogues
{
    public class ChatDialogue : MonoBehaviour
    {
        [SerializeField] private GameObject _chatDataContent;
        [SerializeField] private GameObject _messagePrefab;

        [SerializeField] private List<GameObject> _answerButtons;
        [Space] [SerializeField] private List<Message> _messages;

        [Space] [Header("UI")] 
        [SerializeField] private TextMeshProUGUI _companionIsTextingTextComponent;
        
        [SerializeField] private float _readDelay;
        [SerializeField] private float _textingDelay;
        
        [SerializeField] private ChatListSettings _chatListSettings;
        
        private int _index;
        private int _answerIndex => _gameSession.Data.ChatAnswerIndex;
        
        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        [ContextMenu("st d")]
        public void OnChatStart()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            SendMessage();
        }
        
        private void SendMessage()
        {
            if (_messages.Count <= _index || _messages == null) return;
            
            var message = Instantiate(_messagePrefab, _chatDataContent.transform);

            var textMessageComponent = message.GetComponentInChildren<TextMeshProUGUI>();
            textMessageComponent.text = _messages[_index].messageText;

            var imageMessageComponent = message.GetComponentInChildren<Image>();
            imageMessageComponent.sprite = _messages[_index].PFP;

            ReDrawChatAnswers();
            ReBuildChatList();
            _messages[_index].Action?.Invoke();
        }
        
        public void ReBuildChatDialogue()
        {
            if (_messages.Count <= _index)
            {
                EndChatting();
                return;
            }

            var haveToRespond = _messages[_index].ChatAnswerData[_answerIndex].HaveToRespond;
            if (haveToRespond)
            {
                var nextMessage = _messages[_index + 1];
                var textForNextMessage = _messages[_index].ChatAnswerData[_answerIndex].PlayerChatAnswer;
                nextMessage.messageText = textForNextMessage;

                var nextToTheNextMessage = _messages[_index + 2];
                var textForTheNextToTheNextMessage =
                    _messages[_index].ChatAnswerData[_answerIndex].CompanionChatRespond;
                nextToTheNextMessage.messageText = textForTheNextToTheNextMessage;
            }

            _index++;
            SendMessage();
        }

        public void OnCompanionTexting()
        {
            StartCoroutine(CompanionIsTexting());
        }

        private IEnumerator CompanionIsTexting()
        {
            yield return new WaitForSeconds(_readDelay);
            
            _companionIsTextingTextComponent.enabled = true;
            yield return new WaitForSeconds(_textingDelay);
            _companionIsTextingTextComponent.enabled = false;

            _index++;
            SendMessage();
        }

        private void ReBuildChatList()
        {
            if (_messages[_index].IsThisPlayerMessage)
            {
                _chatListSettings.AuthorTextComponent.text = _chatListSettings.PlayerTexted;
                
                _chatListSettings.LastMessageRect.localPosition = 
                    _chatListSettings.LastMessagePositionIfPlayerTexted;
                
                var textLength = _messages[_index].messageText.Length;
                if (textLength < _chatListSettings.MaxLengthPlayerText)
                {
                    _chatListSettings.LastMessageComponent.text = _messages[_index].messageText;
                }
                else
                {
                    var lastMessage =
                        _messages[_index].messageText.Substring(0, _chatListSettings.MaxLengthPlayerText) + "...";
                    _chatListSettings.LastMessageComponent.text = lastMessage;
                }
            }
            else
            {
                _chatListSettings.AuthorTextComponent.text = _chatListSettings.CompanionTexted;
                
                _chatListSettings.LastMessageRect.localPosition =
                    _chatListSettings.LastMessagePositionIfCompanionTexted;
                
                var textLength = _messages[_index].messageText.Length;
                if (textLength < _chatListSettings.MaxLengthCompanionText)
                {
                    _chatListSettings.LastMessageComponent.text = _messages[_index].messageText;
                }
                else
                {
                    var lastMessage =
                        _messages[_index].messageText.Substring(0, _chatListSettings.MaxLengthCompanionText) + "...";
                    _chatListSettings.LastMessageComponent.text = lastMessage;
                }
            }
        }

        private void ReDrawChatAnswers()
        {
            foreach (var button in _answerButtons)
            {
                button.SetActive(false);
            }

            for (var i = 0; i < _messages[_index].ChatAnswerData.Count; i++)
            {
                _answerButtons[i].SetActive(true);

                _messages[_index].ChatAnswerData[i].answerComponent.text =
                    _messages[_index].ChatAnswerData[i].PlayerChatAnswer;
            }
        }
        
        private void EndChatting()
        {
        }

        [Serializable]
        public class Message
        {
            [SerializeField] private Sprite _pfp;
            public Sprite PFP => _pfp;

            public string messageText;
            
            [SerializeField] private UnityEvent _action;
            public UnityEvent Action => _action;

            [SerializeField] private List<AnswersData> _chatAnswerData;
            public List<AnswersData> ChatAnswerData => _chatAnswerData;

            [SerializeField] private bool _isThisPlayerMessage;
            public bool IsThisPlayerMessage => _isThisPlayerMessage;
        }

        [Serializable]
        public class AnswersData
        {
            [SerializeField] private TextMeshProUGUI _answerComponent;
            public TextMeshProUGUI answerComponent => _answerComponent;
            
            [SerializeField] private string _playerChatAnswer;
            public string PlayerChatAnswer => _playerChatAnswer;

            [SerializeField] private bool _haveToRespond;
            public bool HaveToRespond => _haveToRespond;
            
            [SerializeField] private string _companionChatRespond;
            public string CompanionChatRespond => _companionChatRespond;
        }

        [Serializable]
        public class ChatListSettings
        {
            public TextMeshProUGUI AuthorTextComponent;
            public TextMeshProUGUI LastMessageComponent;

            [SerializeField] private string _playerTexted;
            public string PlayerTexted => _playerTexted;
            
            [SerializeField] private string _companionTexted;
            public string CompanionTexted => _companionTexted;

            [SerializeField] private RectTransform _lastMessageRect;
            public RectTransform LastMessageRect => _lastMessageRect;
            
            [SerializeField] private Vector2 _lastMessagePositionIfPlayerTexted;
            public Vector2 LastMessagePositionIfPlayerTexted => _lastMessagePositionIfPlayerTexted;
            
            [SerializeField] private Vector2 _lastMessagePositionIfCompanionTexted;
            public Vector2 LastMessagePositionIfCompanionTexted => _lastMessagePositionIfCompanionTexted;

            [SerializeField] private byte _maxLengthPlayerText;
            public byte MaxLengthPlayerText => _maxLengthPlayerText;
            
            [SerializeField] private byte _maxLengthCompanionText;
            public byte MaxLengthCompanionText => _maxLengthCompanionText;
        }
    }
}
// 55  400.4878 Lucia