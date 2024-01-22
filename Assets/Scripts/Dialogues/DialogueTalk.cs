using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Autophobia.Model;
using Autophobia.PlayerComponents;
using System;
using System.Collections.Generic;

namespace Autophobia.Dialogues
{
    public class DialogueTalk : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _mainTextComponent;
        [SerializeField] private GameObject _dialogueObjectParent;
        [SerializeField] private List<GameObject> _answerObjectsParents;

        [SerializeField] private List<DialogueInfo> _dialoguesData = new List<DialogueInfo>();

        [SerializeField] private UnityEvent _dialogueIsStarted;
        [SerializeField] private UnityEvent _dialogueIsEnded;

        [SerializeField] private bool _canUseOnce;
        private bool _canUse;

        private GameSession _gameSession;
        private int _answerIndex => _gameSession.Data.AnswerIndex;
        private int _index;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        [ContextMenu("start dialogue")]
        public void OnDialogueStart()
        {
            if (_dialoguesData.Count == 0 || !_canUse) return;
            if (_canUseOnce) _canUse = false;
            
            _dialogueIsStarted?.Invoke();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            var player = FindObjectOfType<PlayerSystem>();
            player.GetCurrentDialogue(this);

            _dialogueObjectParent.SetActive(true);
            OnShowMessage();
        }

        private void OnShowMessage()
        {
            var currentDialogueText = _dialoguesData[_index].dialogueText;
            _mainTextComponent.text = currentDialogueText;

            ReDrawAnswers();
        }

        private void ReDrawAnswers()
        {
            foreach (var answerParent in _answerObjectsParents)
            {
                answerParent.SetActive(false);
            }

            var answersCount = _dialoguesData[_index].Answers.Count;
            for (var i = 0; i < answersCount; i++)
            {
                _answerObjectsParents[i].SetActive(true);

                var currentAnswer = _dialoguesData[_index].Answers[i];
                currentAnswer.AnswerComponent.text = currentAnswer.PlayerAnswer;
            }
        }

        public void ReBuildDialogue()
        {
            if (_dialoguesData.Count <= _index + 1)
            {
                EndDialogue();
                return;
            }

            var haveToRespondOnPlayerAnswer = _dialoguesData[_index].Answers[_answerIndex].HaveToRespond;
            if (haveToRespondOnPlayerAnswer)
            {
                var nextDialogueInfo = _dialoguesData[_index + 1];
                var companionRespond = _dialoguesData[_index].Answers[_answerIndex].CompanionRespond;

                nextDialogueInfo.dialogueText = companionRespond;
            }

            _index++;
            OnShowMessage();
        }

        private void EndDialogue()
        {
            _dialogueIsEnded?.Invoke();

            var player = FindObjectOfType<PlayerSystem>();
            player.GetCurrentDialogue(null);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            _dialogueObjectParent.SetActive(false);
        }

        [Serializable]
        public class DialogueInfo
        {
            public string dialogueText;

            [SerializeField] private List<AnswersInfo> _answers;
            public List<AnswersInfo> Answers => _answers;
        }

        [Serializable]
        public class AnswersInfo
        {
            [SerializeField] private TextMeshProUGUI _answerComponent;
            public TextMeshProUGUI AnswerComponent => _answerComponent;

            [Space] [SerializeField] private bool _haveToRespond;
            public bool HaveToRespond => _haveToRespond;

            [SerializeField] private string _playerAnswer;
            public string PlayerAnswer => _playerAnswer;

            [SerializeField] private string _companionRespond;
            public string CompanionRespond => _companionRespond;
        }
    }
}