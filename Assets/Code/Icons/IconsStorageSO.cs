using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[CreateAssetMenu(menuName = "ScriptableObjects/Icons/Icons Storage", fileName = "IconsStorage")]
public class IconsStorageSO : ScriptableObject
{
    [SerializeField] private List<IconConfiguration> _icons;

    private Dictionary<CardStatType, Sprite> _iconsStorage;
    private bool _isInitialized = false;

    public void Init()
    {
        _iconsStorage = GetIconsDictionary();
        _isInitialized = true;
    }

    private Dictionary<CardStatType, Sprite> GetIconsDictionary()
    {
        Dictionary<CardStatType, Sprite> iconsDictionary = new Dictionary<CardStatType, Sprite>();
        bool status = true;

        foreach (IconConfiguration icon in _icons)
        {
            status = iconsDictionary.TryAdd(icon.Name, icon.Sprite);

#if UNITY_EDITOR
            if (!status)
            {
                Debug.LogWarning($"The are more than one icon called {icon.Name}.");
            }
#endif
        }

        return iconsDictionary;
    }

    public Sprite GetSpriteFromCardStatType(CardStatType name)
    {
        Assert.IsTrue(_isInitialized, $"[IconsStorageSO at GetSpriteFromCardStatType]: The icons Storage needs to be initialized. Call Init() method");

        Sprite wantedAudioClip;
        bool status = _iconsStorage.TryGetValue(name, out wantedAudioClip);

        Assert.IsTrue(status, $"[IconsStorageSO at GetSpriteFromCardStatType]: There is not an icon called {name.ToString()} in the storage");

        return wantedAudioClip;
    }
}
