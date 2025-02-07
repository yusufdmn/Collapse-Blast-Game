using BaseComponents;
using Data;
using Features;
using InputSystem;
using Settings;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    #region Fields

    [SerializeField] private LevelSettings _levelSettings;
    [SerializeField] private BlockTileStorageSO _blockTileStorageSo;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private BlockPool _blockPool;
    
    private InputManager _inputManager;
    private BoardData _boardData;
    private SmartShuffler _smartShuffler;
    private GroupDetector _groupDetector;
    private IconUpdater _iconUpdater;
    private Blaster _blaster;
    private BoardRenderer _boardRenderer;
    private Collapser _collapser;
    private BlockAnimator _blockAnimator;
    private TilemapController _tilemapController;

    #endregion
    

    #region Unity Methods
    void Start()
    {
        // Initialize the components
        _inputManager = new InputManager();
        _boardData = new BoardData(_levelSettings); 
        _smartShuffler = new SmartShuffler(_levelSettings.TotalTileTypes, _boardData);
        _tilemapController = new TilemapController(_tilemap, _boardData, _blockTileStorageSo);
        _boardRenderer = new BoardRenderer(_blockTileStorageSo.DefaultBlockTilesSo, _tilemapController); 
        _iconUpdater = new IconUpdater(_blockTileStorageSo, _levelSettings, _tilemapController); 
        _groupDetector = new GroupDetector(_boardData.Grid(), _boardData, _smartShuffler);
        _blaster = new Blaster(_tilemapController, _levelSettings);
        _blockAnimator = new BlockAnimator(_tilemapController, _blockPool);
        _collapser = new Collapser(_tilemapController, _levelSettings, _blockAnimator, _blockTileStorageSo);
        
        SubscribeEvents();
        InitializeBoard();
    }

    void OnDisable()
    {
        UnsubscribeEvents();
    }
    #endregion

    #region Private Methods

    private void InitializeBoard()
    {
        _smartShuffler.Shuffle();
    }

    
    // Event handlers
    private void OnTileClick(Vector3 worldPosition)
    {
        if(_blaster.CanBlast(_groupDetector.AllGroups, worldPosition))
            _inputManager.CanClick(false);
        
        _collapser.FlagTilesToCollapse(_groupDetector.AllGroups, worldPosition);
        _blaster.Blast(_groupDetector.AllGroups, worldPosition);
        _collapser.Collapse();
    }
    
    private void OnBoardShuffled()
    {
        _boardRenderer.RenderFullGrid(_boardData.Grid());
    }
    
    private void OnGridRendered()
    {
        _groupDetector.DetectGroups();
    }

    private void OnCollapseEnded()
    {
        _groupDetector.DetectGroups();
    }
    
    private void OnGroupsDetected()
    {
        _iconUpdater.UpdateIcons(_groupDetector.AllGroups, _boardData.Grid());
    }
    

    private void OnIconsUpdated()
    {
        _inputManager.CanClick(true);
    }
    
    
    
    // Event subscription/unsubscription
    private void SubscribeEvents()
    {
        _inputManager.Clicked += OnTileClick;
        _smartShuffler.BoardShuffled += OnBoardShuffled;
        _boardRenderer.GridRendered += OnGridRendered;
        _collapser.CollapseEnded += OnCollapseEnded;
        _groupDetector.GroupsDetected += OnGroupsDetected;
        _iconUpdater.IconUpdateEnded += OnIconsUpdated;
    }

    private void UnsubscribeEvents()
    {
        _inputManager.Clicked -= OnTileClick; 
        _smartShuffler.BoardShuffled -= OnBoardShuffled;
        _boardRenderer.GridRendered -= OnGridRendered;
        _collapser.CollapseEnded -= OnCollapseEnded;
        _groupDetector.GroupsDetected -= OnGroupsDetected;
        _iconUpdater.IconUpdateEnded -= OnIconsUpdated;
    }

    #endregion

}