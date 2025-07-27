using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace YAProgressBar
{
    public class SceneLoader : MonoBehaviour
    {
#pragma warning disable 0649
        [Serializable]
        class ButtonScene
        {
            public Button button;
            public string sceneName;
            public string buttonName;
        }
#pragma warning restore 0649

        [SerializeField]
        private ButtonScene[] controls = null;

        private void Start()
        {
            for (int i = 0; i < controls.Length; i++)
            {
                var sceneName = controls[i].sceneName;
                controls[i].button.onClick.AddListener(() => LoadScene(sceneName));
                controls[i].button.GetComponentInChildren<Text>().text = controls[i].buttonName;
            }
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
