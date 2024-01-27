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
            
            [Space] [SerializeField] private UnityEvent _action;
            public UnityEvent Action => _action;

            [SerializeField] private List<AnswersData> _chatAnswerData;
            public List<AnswersData> ChatAnswerData => _chatAnswerData;
        }

        [Serializable]
        public class AnswersData
        {
            public TextMeshProUGUI answerComponent;
            
            [SerializeField] private string _playerChatAnswer;
            public string PlayerChatAnswer => _playerChatAnswer;

            [SerializeField] private bool _haveToRespond;
            public bool HaveToRespond => _haveToRespond;
            
            [SerializeField] private string _companionChatRespond;
            public string CompanionChatRespond => _companionChatRespond;
        }
    }
}
