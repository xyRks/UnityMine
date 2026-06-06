using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Автоматический генератор сцен для проекта UnityMine.
/// Добавляет пункт меню Tools -> Generate Demo Scenes.
/// </summary>
public class SceneGenerator
{
    [MenuItem("Tools/Generate Demo Scenes")]
    public static void GenerateScenes()
    {
        // Убедимся, что папка Scenes существует
        string scenesDir = "Assets/Scenes";
        if (!Directory.Exists(scenesDir))
        {
            Directory.CreateDirectory(scenesDir);
        }

        GenerateMainMenuScene();
        GenerateGameScene();

        Debug.Log("Генерация сцен завершена! Откройте сцены в папке Assets/Scenes.");
    }

    private static void GenerateMainMenuScene()
    {
        // Создаем новую пустую сцену с камерой и светом по умолчанию
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "MainMenuScene";

        // Создаем менеджер главного меню
        var managerGO = new GameObject("MainMenuManager");
        var mainMenu = managerGO.AddComponent<MainMenu>();
        mainMenu.gameSceneName = "GameScene";

        // Создаем Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Создаем EventSystem
        var eventSystemGO = new GameObject("EventSystem");
        eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Создаем заголовок "UnityMine"
        var titleGO = new GameObject("TitleText");
        titleGO.transform.SetParent(canvasGO.transform, false);
        var titleText = titleGO.AddComponent<Text>();
        titleText.text = "UnityMine";
        titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        titleText.fontSize = 55;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(0.9f, 0.8f, 0.4f); // Золотистый цвет
        var titleRect = titleGO.GetComponent<RectTransform>();
        titleRect.anchoredPosition = new Vector2(0, 120);
        titleRect.sizeDelta = new Vector2(500, 100);

        // Создаем кнопку Play
        var playButtonGO = CreateButton(canvasGO, "PlayButton", "Играть (Play)", new Vector2(0, 10));
        var playBtn = playButtonGO.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(playBtn.onClick, mainMenu.PlayGame);

        // Создаем кнопку Quit
        var quitButtonGO = CreateButton(canvasGO, "QuitButton", "Выйти (Quit)", new Vector2(0, -60));
        var quitBtn = quitButtonGO.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(quitBtn.onClick, mainMenu.QuitGame);

        // Сохраняем сцену
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/MainMenuScene.unity");
        Debug.Log("Сцена MainMenuScene успешно создана!");
    }

    private static void GenerateGameScene()
    {
        // Создаем новую сцену
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        scene.name = "GameScene";

        // Удаляем камеру по умолчанию, так как у игрока будет своя
        var defaultCam = GameObject.Find("Main Camera");
        if (defaultCam != null)
        {
            GameObject.DestroyImmediate(defaultCam);
        }

        // Создаем землю (Plane)
        var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(30f, 1f, 30f); // 300x300 метров
        
        // Красим землю в приятный зеленый цвет
        var groundRenderer = ground.GetComponent<Renderer>();
        var groundMat = new Material(Shader.Find("Standard"));
        groundMat.color = new Color(0.25f, 0.45f, 0.25f);
        groundRenderer.sharedMaterial = groundMat;

        // Создаем Игрока
        var player = new GameObject("Player");
        player.transform.position = new Vector3(0f, 1.1f, 0f);
        
        var cc = player.AddComponent<CharacterController>();
        cc.height = 2f;
        cc.radius = 0.5f;
        cc.center = new Vector3(0f, 1f, 0f);

        var pm = player.AddComponent<PlayerMovement>();
        var health = player.AddComponent<Health>();
        health.maxHealth = 100;
        health.currentHealth = 100;

        var inventory = player.AddComponent<Inventory>();
        player.AddComponent<SaveManager>();

        // Создаем камеру игрока
        var camGO = new GameObject("PlayerCamera");
        camGO.transform.SetParent(player.transform);
        camGO.transform.localPosition = new Vector3(0f, 1.8f, 0f); // На уровне глаз
        camGO.transform.localRotation = Quaternion.identity;
        
        camGO.AddComponent<Camera>();
        camGO.AddComponent<AudioListener>();
        var ml = camGO.AddComponent<MouseLook>();
        ml.playerBody = player.transform;

        // Создаем Спавнер Мира
        var spawnerGO = new GameObject("WorldSpawner");
        var spawner = spawnerGO.AddComponent<WorldSpawner>();
        spawner.numberOfObjectsToSpawn = 120;
        spawner.spawnAreaWidth = 150f;
        spawner.spawnAreaLength = 150f;

        // Загружаем префабы
        spawner.forestTreePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ForestTree.prefab");
        spawner.forestStonePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ForestStone.prefab");
        spawner.forestOrePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ForestOre.prefab");
        spawner.forestHousePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/ForestHouse.prefab");
        spawner.meadowTreePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MeadowTree.prefab");
        spawner.meadowStonePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MeadowStone.prefab");
        spawner.meadowOrePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MeadowOre.prefab");
        spawner.meadowHousePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/MeadowHouse.prefab");

        // Создаем Верстак (Cube)
        var bench = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bench.name = "CraftingBenchObj";
        bench.transform.position = new Vector3(2f, 0.5f, 5f);
        bench.transform.localScale = new Vector3(1.2f, 1f, 1.2f);
        
        // Накладываем приятный цвет верстака (дерево)
        var benchRenderer = bench.GetComponent<Renderer>();
        var benchMat = new Material(Shader.Find("Standard"));
        benchMat.color = new Color(0.5f, 0.35f, 0.2f);
        benchRenderer.sharedMaterial = benchMat;

        var cb = bench.AddComponent<CraftingBench>();
        cb.playerInventory = inventory;

        // Загружаем предметы и привязываем их к верстаку
        cb.woodItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Wood.asset");
        cb.stoneItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Stone.asset");
        cb.pickaxeItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Pickaxe.asset");
        cb.axeItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Axe.asset");
        cb.swordItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Sword.asset");
        cb.bowItem = AssetDatabase.LoadAssetAtPath<ItemData>("Assets/Items/Item_Bow.asset");

        // Добавляем стартовые ресурсы в инвентарь игрока для тестирования крафта
        inventory.slots = new InventorySlot[20];
        for (int i = 0; i < 20; i++)
        {
            inventory.slots[i] = new InventorySlot();
        }
        inventory.slots[0].item = cb.woodItem;
        inventory.slots[0].amount = 20;
        inventory.slots[1].item = cb.stoneItem;
        inventory.slots[1].amount = 20;

        // Создаем Canvas для игры
        var canvasGO = new GameObject("GameCanvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // EventSystem
        var eventSystemGO = new GameObject("EventSystem");
        eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
        eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // GameUIController
        var uiController = canvasGO.AddComponent<GameUIController>();
        uiController.playerHealth = health;
        uiController.playerInventory = inventory;
        uiController.craftingBench = cb;

        // Создаем Текст Здоровья
        var healthGO = new GameObject("HealthText");
        healthGO.transform.SetParent(canvasGO.transform, false);
        var healthText = healthGO.AddComponent<Text>();
        healthText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        healthText.fontSize = 24;
        healthText.color = new Color(0.9f, 0.2f, 0.2f); // Красный
        healthText.text = "Здоровье: 100 / 100";
        var healthRect = healthGO.GetComponent<RectTransform>();
        healthRect.anchorMin = new Vector2(0f, 1f);
        healthRect.anchorMax = new Vector2(0f, 1f);
        healthRect.pivot = new Vector2(0f, 1f);
        healthRect.anchoredPosition = new Vector2(25, -25);
        healthRect.sizeDelta = new Vector2(300, 50);
        uiController.healthText = healthText;

        // Создаем Текст Инвентаря
        var invGO = new GameObject("InventoryText");
        invGO.transform.SetParent(canvasGO.transform, false);
        var invText = invGO.AddComponent<Text>();
        invText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        invText.fontSize = 20;
        invText.color = Color.white;
        invText.text = "Инвентарь:\n• Дерево x20\n• Камень x20";
        var invRect = invGO.GetComponent<RectTransform>();
        invRect.anchorMin = new Vector2(0f, 0f);
        invRect.anchorMax = new Vector2(0f, 0f);
        invRect.pivot = new Vector2(0f, 0f);
        invRect.anchoredPosition = new Vector2(25, 25);
        invRect.sizeDelta = new Vector2(300, 250);
        uiController.inventoryText = invText;

        // Кнопка открытия верстака (инструкция)
        var openBenchGO = CreateButton(canvasGO, "OpenBenchButton", "Открыть верстак (E)", new Vector2(-110, -35));
        var openBenchBtn = openBenchGO.GetComponent<Button>();
        UnityEventTools.AddPersistentListener(openBenchBtn.onClick, uiController.OpenCrafting);
        var openRect = openBenchGO.GetComponent<RectTransform>();
        openRect.anchorMin = new Vector2(1f, 1f);
        openRect.anchorMax = new Vector2(1f, 1f);
        openRect.pivot = new Vector2(1f, 1f);
        openRect.sizeDelta = new Vector2(180, 40);

        // Создаем Панель Крафта (Интерфейс верстака)
        var panelGO = new GameObject("CraftingPanel");
        panelGO.transform.SetParent(canvasGO.transform, false);
        var panelImage = panelGO.AddComponent<Image>();
        panelImage.color = new Color(0.12f, 0.12f, 0.12f, 0.95f);
        var panelRect = panelGO.GetComponent<RectTransform>();
        panelRect.anchoredPosition = new Vector2(0f, 0f);
        panelRect.sizeDelta = new Vector2(350f, 420f);
        uiController.craftingPanel = panelGO;

        // Панель: Заголовок
        var headerGO = new GameObject("Header");
        headerGO.transform.SetParent(panelGO.transform, false);
        var headerText = headerGO.AddComponent<Text>();
        headerText.text = "Верстак (Crafting)";
        headerText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        headerText.fontSize = 24;
        headerText.alignment = TextAnchor.MiddleCenter;
        headerText.color = new Color(0.9f, 0.8f, 0.3f);
        var headerRect = headerGO.GetComponent<RectTransform>();
        headerRect.anchoredPosition = new Vector2(0f, 180f);
        headerRect.sizeDelta = new Vector2(300f, 40f);

        // Панель: Подзаголовок с рецептами
        var subGO = new GameObject("RecipeInfo");
        subGO.transform.SetParent(panelGO.transform, false);
        var subText = subGO.AddComponent<Text>();
        subText.text = "Рецепты:\nКирка: 2д + 3к | Топор: 3д + 2к\nМеч: 1д + 4к | Лук: 5д + 1к";
        subText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        subText.fontSize = 13;
        subText.alignment = TextAnchor.MiddleCenter;
        subText.color = new Color(0.7f, 0.7f, 0.7f);
        var subRect = subGO.GetComponent<RectTransform>();
        subRect.anchoredPosition = new Vector2(0f, 130f);
        subRect.sizeDelta = new Vector2(300f, 50f);

        // Кнопки крафта внутри панели
        var cpBtnGO = CreateButton(panelGO, "CraftPickaxeButton", "Скрафтить кирку", new Vector2(0f, 80f));
        UnityEventTools.AddPersistentListener(cpBtnGO.GetComponent<Button>().onClick, cb.CraftPickaxe);

        var caBtnGO = CreateButton(panelGO, "CraftAxeButton", "Скрафтить топор", new Vector2(0f, 25f));
        UnityEventTools.AddPersistentListener(caBtnGO.GetComponent<Button>().onClick, cb.CraftAxe);

        var csBtnGO = CreateButton(panelGO, "CraftSwordButton", "Скрафтить меч", new Vector2(0f, -30f));
        UnityEventTools.AddPersistentListener(csBtnGO.GetComponent<Button>().onClick, cb.CraftSword);

        var cbBtnGO = CreateButton(panelGO, "CraftBowButton", "Скрафтить лук", new Vector2(0f, -85f));
        UnityEventTools.AddPersistentListener(cbBtnGO.GetComponent<Button>().onClick, cb.CraftBow);

        var closeBtnGO = CreateButton(panelGO, "CloseButton", "Закрыть (E)", new Vector2(0f, -150f));
        closeBtnGO.GetComponent<Image>().color = new Color(0.5f, 0.15f, 0.15f, 0.85f);
        UnityEventTools.AddPersistentListener(closeBtnGO.GetComponent<Button>().onClick, uiController.CloseCrafting);

        // Сохраняем сцену
        EditorSceneManager.SaveScene(scene, "Assets/Scenes/GameScene.unity");
        Debug.Log("Сцена GameScene успешно создана!");
    }

    private static GameObject CreateButton(GameObject parent, string name, string label, Vector2 pos)
    {
        var btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent.transform, false);
        
        var image = btnGO.AddComponent<Image>();
        image.color = new Color(0.2f, 0.2f, 0.2f, 0.85f);
        
        var button = btnGO.AddComponent<Button>();
        
        var rect = btnGO.GetComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.sizeDelta = new Vector2(200f, 45f);

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(btnGO.transform, false);
        
        var text = textGO.AddComponent<Text>();
        text.text = label;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 18;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        
        var textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        return btnGO;
    }
}
