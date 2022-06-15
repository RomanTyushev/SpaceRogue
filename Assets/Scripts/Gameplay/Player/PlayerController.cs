using Abstracts;
using Gameplay.Player.Inventory;
using Gameplay.Player.Movement;
using Scriptables;
using Scriptables.Modules;
using UnityEngine;
using Utilities.Reactive.SubscriptionProperty;
using Utilities.ResourceManagement;

namespace Gameplay.Player
{
    public class PlayerController : BaseController
    {
        private readonly ResourcePath _configPath = new("Configs/PlayerConfig");
        private readonly ResourcePath _viewPath = new("Prefabs/Gameplay/Player");
        
        private readonly PlayerConfig _config;
        private readonly PlayerView _view;

        private readonly SubscribedProperty<float> _horizontalInput = new();
        private readonly SubscribedProperty<float> _verticalInput = new();
        private readonly PlayerMovementController _movementController;
        private readonly PlayerInventoryController _inventoryController;
        

        public PlayerController()
        {
            _config = ResourceLoader.LoadObject<PlayerConfig>(_configPath);
            _view = LoadView<PlayerView>(_viewPath, Vector3.zero);

            _inventoryController = AddInventoryController(_config.Inventory);
            _movementController = AddInputController(_inventoryController.Engine, _view);
        }

        private PlayerInventoryController AddInventoryController(PlayerInventoryConfig config)
        {
            var inventoryController = new PlayerInventoryController(_config.Inventory);
            AddController(inventoryController);
            return inventoryController;
        }

        private PlayerMovementController AddInputController(EngineModuleConfig movementConfig, PlayerView view)
        {
            var movementController = new PlayerMovementController(_horizontalInput, _verticalInput, movementConfig, view);
            AddController(movementController);
            return movementController;
        }
    }
}