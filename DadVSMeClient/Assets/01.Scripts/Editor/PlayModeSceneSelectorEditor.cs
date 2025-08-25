using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace DadVSMe.Editors
{
    using SceneManager = UnityEngine.SceneManagement.SceneManager;

    [InitializeOnLoad]
    public class PlayModeSceneSelectorEditor
    {
        private const string START_SCENE_NAME_KEY = "StartSceneName";
        private const string LAST_SCENE_PATH_KEY = "LastScenePath";
        private const string MENU_ROOT_NAME = "Tools/Start Scene/";

        private const string NONE = "None";
        private const string BOOTSTRAP = "Bootstrap";
        private const string BOOTSTRAP1 = "Bootstrap 1";
        private static readonly string[] SCENES = {
            NONE,
            BOOTSTRAP,
            BOOTSTRAP1,
        };

        private static bool changingSceneFlag = false;

        static PlayModeSceneSelectorEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            string loadedOrDefualtSceneName = EditorPrefs.GetString(START_SCENE_NAME_KEY, BOOTSTRAP);
            SetPlayModeScene(loadedOrDefualtSceneName);
        }

        private static void SetPlayModeScene(string sceneName)
        {
            string scenePath = GetScenePathByName(sceneName);
            if (sceneName != NONE && string.IsNullOrEmpty(scenePath))
                Debug.LogWarning($"There are no scenes registered in the build settings with name. : {sceneName}");

            foreach (string i in SCENES)
                Menu.SetChecked(MENU_ROOT_NAME + i, false);
            Menu.SetChecked(MENU_ROOT_NAME + sceneName, true);

            EditorPrefs.SetString(START_SCENE_NAME_KEY, sceneName);
        }

        [MenuItem(MENU_ROOT_NAME + NONE, secondaryPriority = -1)]
        public static void SelectScene_NONE() => SetPlayModeScene(NONE);

        [MenuItem(MENU_ROOT_NAME + BOOTSTRAP)]
        public static void SelectScene_BOOTSTRAP() => SetPlayModeScene(BOOTSTRAP);

        [MenuItem(MENU_ROOT_NAME + BOOTSTRAP1)]
        public static void SelectScene_BOOTSTRAP1() => SetPlayModeScene(BOOTSTRAP1);

        private static string GetScenePathByName(string sceneName)
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                string scenePath = scene.path;
                if (scenePath.Contains(sceneName))
                    return scenePath;
            }

            return "";
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                    LoadPlayModeStartScene();
                    break;
                case PlayModeStateChange.EnteredEditMode:
                    LoadLastScene();
                    break;
            }
        }

        private static void LoadPlayModeStartScene()
        {
            if (changingSceneFlag)
                return;

            string targetSceneName = EditorPrefs.GetString(START_SCENE_NAME_KEY, BOOTSTRAP);
            if (targetSceneName == NONE)
                return;

            string targetScenePath = GetScenePathByName(targetSceneName);
            Debug.Log(targetScenePath);

            if (string.IsNullOrEmpty(targetScenePath))
                return;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                string currentScenePath = SceneManager.GetActiveScene().path;
                EditorPrefs.SetString(LAST_SCENE_PATH_KEY, currentScenePath);

                changingSceneFlag = true;
                EditorSceneManager.OpenScene(targetScenePath);
                changingSceneFlag = false;
            }
            else
            {
                EditorApplication.isPlaying = false;
            }
        }

        private static void LoadLastScene()
        {
            if (changingSceneFlag)
                return;

            string targetSceneName = EditorPrefs.GetString(START_SCENE_NAME_KEY, BOOTSTRAP);
            if (targetSceneName == NONE)
                return;

            string lastScenePath = EditorPrefs.GetString(LAST_SCENE_PATH_KEY, string.Empty);
            if (string.IsNullOrEmpty(lastScenePath))
                return;

            if (lastScenePath == SceneManager.GetActiveScene().path)
                return;

            changingSceneFlag = true;
            EditorSceneManager.OpenScene(lastScenePath);
            changingSceneFlag = false;
        }
    }
}