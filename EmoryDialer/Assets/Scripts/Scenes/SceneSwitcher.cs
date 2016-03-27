using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Assets.Scripts.Scenes
{
    class SceneSwitcher
    {
        public enum SceneNames : int
        {
            SceneLogin,
            ScenePhoneDialer,
            SceneHistory,
            SceneMenu,
            SceneAbout,
            SceneDirectory
        };
        static List<string> SceneNamesEnumToString = new List<string>();

        public static Stack<SceneNames> SceneHistory = new Stack<SceneNames>();

        public static SceneNames EnumFromSceneName(string SceneName)
        {
            SceneNamesEnumToString = new List<string>(System.Enum.GetNames(typeof(SceneNames)));
            int EnumIndex = SceneNamesEnumToString.IndexOf(SceneName);
            if (EnumIndex < 0)
            {
                throw new System.Exception("TypeNotFound");
            }
            else
            {
                return (SceneNames)EnumIndex;
            }
        }

        public static void SwitchScene(SceneNames SceneToSwitchTo)
        {
            // Save the current scene
            SceneHistory.Push(EnumFromSceneName(SceneManager.GetActiveScene().name));
            SceneManager.LoadScene(SceneToSwitchTo.ToString());
        }

        public static void BackScene()
        {
            if(SceneHistory.Count > 0)
            {
                // Save the current scene
                string LastScene = SceneHistory.Peek().ToString();
                SceneHistory.Push(EnumFromSceneName(SceneManager.GetActiveScene().name));
                SceneManager.LoadScene(LastScene);
            }
            else
            {
                SwitchScene(SceneNames.ScenePhoneDialer);
            }
        }
    }
}
