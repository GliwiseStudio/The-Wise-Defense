using UnityEngine;

public class IconsStorage : MonoBehaviour
{
    [SerializeField] private IconsStorageSO _iconsStorage;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _iconsStorage.Init();
    }

    public Sprite GetSpriteFromCardStatType(CardStatType name)
    {
        return _iconsStorage.GetSpriteFromCardStatType(name);
    }
}
