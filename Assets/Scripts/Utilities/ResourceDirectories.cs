public static class ResourceDirectories
{
    // Chess Piece Prefab Paths
    private const string CHESS_PIECES_BASE_PATH = "Prefabs/ChessPieces";
    public const string PLAYER_PIECES_PATH = CHESS_PIECES_BASE_PATH + "/Player/PlayerPiece_{0}";
    public const string ENEMY_PIECES_PATH = CHESS_PIECES_BASE_PATH + "/Enemy/EnemyPiece_{0}";
    
    public const string HOLE_SHOP_LEVEL_DIR = "Scenes/HoleSkinShop";
    public const string RENOVATION_LEVEL_DIR = "Scenes/Renovate Scene";
    public const string ROADMAP_LEVEL_DIR = "Scenes/Roadmap Scene";

    // Material Lists
    public const string KİTCHEN_MATERİALS_DIR = "ScriptableObjects/KitchenMaterials";

    // Managers
    public const string COIN_MANAGER_DIR = "ScriptableObjects/Managers/Coin Manager";
    public const string INCOME_CALCULATOR_DIR = "ScriptableObjects/Managers/Income Calculator";
    public const string PREFS_KEY_MANAGER_DIR = "ScriptableObjects/Managers/Pref Keys Manager";

    // Events
    public const string HOLE_LEVEL_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/HoleLevelLoadRequest";
    public const string META_LEVEL_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/MetaLevelLoadRequest";
    public const string HOLE_SKIN_SHOP_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/HoleSkinShopLoadRequest";
    public const string POWERUP_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/PowerUpLoadRequest";
    public const string RENOVATION_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/OnRenovationRequest";
    public const string ROADMAP_LOAD_REQUEST_DIR = "ScriptableObjects/Events/Load Requests/OnRoadmapLoadRequest";

    // Global Variables
    public const string GLOBAL_MAIN_MENU_STATE_DIR = "ScriptableObjects/Global Variables/Global Active Menu State";
    public const string GLOBAL_HOLE_SKIN_DIR = "ScriptableObjects/Global Variables/Global Hole Skin";
    public const string GLOBAL_META_LEVEL_COUNT = "ScriptableObjects/Global Variables/Global Meta Level Count";

    public static readonly string SkyboxMaterials = "Materials/Skyboxes/";
} 