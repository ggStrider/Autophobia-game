using System.Threading.Tasks;
using Autophobia.Additional;
using UnityEngine;
using UnityEngine.AI;

namespace Autophobia.Dialogues
{
    public class LuciasWalkAI : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _threshold = 0.2f;
        
        [SerializeField] private CheckInOnLayer _checkForPlayer;
        
        private NavMeshAgent _agent;
        private int _currentPoint;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            
            _checkForPlayer.EventOnTrue.AddListener(SetDestination);
            _checkForPlayer.EventOnFalse.AddListener(Stop);
        }

        private void SetDestination()
        {
            _agent.destination = _points[_currentPoint].position;
            _agent.isStopped = false;
        }

        private void Stop()
        {
            _agent.isStopped = true;
        }

        private async void CheckIsOnPosition()
        {
            while (transform.position.magnitude > _threshold)
            {
                Task.Yield();
            }
            
            Stop();
        }
    }
}