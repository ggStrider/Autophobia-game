using UnityEngine;
using TMPro;

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
        
        private int _index;

        public void OnDialogueStart()
        {
            if (_dialoguesData.Count == 0) return;
            
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
            }
        }

        public void GetAnswerInt(int answerInt)
        {
            var haveToRespondOnPlayerAnswer = _dialoguesData[_index].Answers[answerInt].HaveToRespond; 
            if (haveToRespondOnPlayerAnswer)
            {
                var nextDialogueInfo = _dialoguesData[_index + 1];
                var companionRespond = _dialoguesData[_index].Answers[answerInt].CompanionRespond;

                nextDialogueInfo.dialogueText = companionRespond;
            }

            _index++;
            OnShowMessage();
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
            [SerializeField] private bool _haveToRespond;
            public bool HaveToRespond => _haveToRespond;
            
            [SerializeField] private string _playerAnswer;
            public string PlayerAnswer => _playerAnswer;

            [SerializeField] private string _companionRespond;
            public string CompanionRespond => _companionRespond;
        }
    }
}