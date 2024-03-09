using UnityEngine;
using System;

namespace Autophobia.Model
{
    [Serializable]
    public class PlayerData
    {
        public int DialogueAnswerIndex;
        public int ChatAnswerIndex;

        [Range(0, 1)] public float MasterVolume;
    }
}