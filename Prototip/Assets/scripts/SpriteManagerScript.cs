using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManagerScript : MonoBehaviour
{
    public Sprite PowerSprite;
    public Sprite SkillSprite;
    public Sprite AttackSprite;

    public List<Sprite> artSprites;

    public Sprite GetFlavourSprite(CardFlavour flavour)
    {
        switch (flavour)
        {
            case CardFlavour.Power:
                return PowerSprite;
            case CardFlavour.Attack:
                return AttackSprite;
            case CardFlavour.Skill:
                return SkillSprite;
        }
        return null;
    }

    public Sprite GetSpriteByName(string name)
    {
        foreach (Sprite sprite in artSprites)
        {
            if (sprite.name == name)
            {
                return sprite;
            }
        }
        return null;
    }
    void Start()
    {
    }

}
