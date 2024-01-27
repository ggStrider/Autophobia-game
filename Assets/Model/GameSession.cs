using UnityEngine;

namespace Autophobia.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;

        private void Awake()
        {
            if (IsSessionsExist())
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }

        private bool IsSessionsExist()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach (var session in sessions)
            {
                if (session != this) return true;
            }

            return false;
        }

        public void GetDialogueAnswer(int index)
        {
            _data.DialogueAnswerIndex = index;
        }

        public void GetChatAnswer(int index)
        {
            _data.ChatAnswerIndex = index;
        }
    }
}