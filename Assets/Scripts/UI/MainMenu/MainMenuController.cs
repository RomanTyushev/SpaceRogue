using Abstracts;
using Gameplay.Background;
using Gameplay.GameState;
using Scriptables;
using UI.Game;
using UnityEngine;
using Utilities.ResourceManagement;

namespace UI.MainMenu
{
    public sealed class MainMenuController : BaseController
    {
        private readonly CurrentState _currentState;
        private readonly LevelProgressConfig _levelProgressConfig;
        
        private readonly ResourcePath _mainMenuCanvasPath = new(Constants.Prefabs.Canvas.Menu.MainMenuCanvas);
        
        private MainMenuCanvasView _mainMenuCanvasView;

        public MainMenuController(CurrentState currentState, Canvas mainUICanvas, LevelProgressConfig levelProgressConfig)
        {
            _currentState = currentState;
            _levelProgressConfig = levelProgressConfig;
            AddMainMenuCanvas(mainUICanvas.transform);
            _levelProgressConfig.ResetCompletedLevels();


            var menuBackgroundController = new MenuBackgroundController();
            AddController(menuBackgroundController);
        }

        private void AddMainMenuCanvas(Transform transform)
        {
            _mainMenuCanvasView = ResourceLoader.LoadPrefabAsChild<MainMenuCanvasView>(_mainMenuCanvasPath, transform);
            _mainMenuCanvasView.Init(StartGame, ResetRecord, ExitGame, 
                _levelProgressConfig.CompletedLevels, _levelProgressConfig.RecordCompletedLevels);
            AddGameObject(_mainMenuCanvasView.gameObject);
        }

        private void StartGame()
        {
            _currentState.CurrentGameState.Value = GameState.Game;
        }

        private void ResetRecord()
        {
            _levelProgressConfig.ResetRecord();
            _mainMenuCanvasView.UpdateRecordNumber(_levelProgressConfig.RecordCompletedLevels);
        }

        private void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}