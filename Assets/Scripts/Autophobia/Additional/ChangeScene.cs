using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Autophobia.Additional
{
    public class ChangeScene : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void _OnChangeScene()
        {
            SceneManager.LoadScene(_sceneName);
        }

        public async void _OnChangeLevelAsync()
        {
            await OnChangeLevelAsync();
        }
        
        private async Task OnChangeLevelAsync()
        {
            var loadLevel = SceneManager.LoadSceneAsync(_sceneName);

            while (!loadLevel.isDone)
            {
                Debug.Log(loadLevel.progress);
                await Task.Yield();
            }
        }
    }
}