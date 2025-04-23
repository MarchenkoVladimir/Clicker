using Leopotam.EcsLite;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GameObject _businessPrefab;
    [SerializeField] private Transform _businessContainer;
    
    [Space(10)]
    [Header("Player")]
    [SerializeField] private PlayerViewBinder _playerView;

    private EcsWorld _world;
    private EcsSystems _systems;

    private void Start()
    {
        _world = new EcsWorld();
        _systems = new EcsSystems(_world)
            .Add(new BusinessProgressSystem(_gameConfig))
            .Add(new BusinessUpdateSystem(_gameConfig))
            .Add(new BusinessPurchaseSystem(_gameConfig))
            .Add(new PlayerUpdateSystem());
        _systems.Init();
    
         SaveSystem.LoadGame(_world, _gameConfig);
    
         InitializePlayer();
         
        if (_world.Filter<Business>().End().GetEntitiesCount() == 0)
        {
            InitializeBusinesses();
            InitializeView();
        }
        else
            InitializeLoadBusinesses();
    }

    private void InitializePlayer()
    {
        if(_world.Filter<Business>().End().GetEntitiesCount() == 0)
            _playerView.Initialize(_world, 0f);
        else
        {
            var playerFilter = _world.Filter<Player>().End();
            foreach (var entity in playerFilter)
            {
                _playerView.Initialize(_world, _world.GetPool<Player>().Get(entity).balance);
                break;
            }
        }
    }
   
    private void InitializeBusinesses()
    {
        for (int i = 0; i < _gameConfig.businesses.Count; i++)
        {
            var entity = _world.NewEntity();
            ref var business = ref _world.GetPool<Business>().Add(entity);
            business.configIndex = i;
            business.level = i == 0 ? 1 : 0;
            business.isUnlocked = i == 0;
        }
    }
    
    private void InitializeView()
    {
        for (int i = 0; i < _gameConfig.businesses.Count; i++)
        {
            var view = Instantiate(_businessPrefab, _businessContainer);
            var binder = view.GetComponent<BusinessViewBinder>();
            binder.Initialize(_world, i);
        }
    }
    
    private void InitializeLoadBusinesses()
    {
        var businessPool = _world.GetPool<Business>();
        var businessFilter = _world.Filter<Business>().End();
        foreach (var entity in businessFilter)
        {
            ref var business = ref businessPool.Get(entity);
            var view = Instantiate(_businessPrefab, _businessContainer);
            var binder = view.GetComponent<BusinessViewBinder>();
            binder.Initialize(_world, business.configIndex);
               
        }
    }

    private void Update() => _systems?.Run();
    private void OnApplicationQuit()
    {
        SaveSystem.SaveGame(_world, _gameConfig);
        _world.Destroy();
    }
    private void OnDestroy() => _world?.Destroy();
}
