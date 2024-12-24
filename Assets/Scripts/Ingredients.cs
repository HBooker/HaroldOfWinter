using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredients
{
    public int snowMax;
    public int giftsMax;
    public int lightsMax;
    public int snowCount = 0;
    public int giftsCount = 0;
    public int lightsCount = 0;

    public enum Type
    {
        None,
        Snow,
        Gifts,
        Lights
    };

    public Ingredients(int snow, int gifts, int lights) 
    {
        snowMax = snow;
        giftsMax = gifts;
        lightsMax = lights;
    }

    public bool CanAddIngredient(Type ing)
    {
        switch (ing)
        {
            case Type.Snow:
                return snowCount < snowMax;
            case Type.Gifts:
                return giftsCount < giftsMax;
            case Type.Lights:
                return lightsCount < lightsMax;
        }

        return false;
    }

    public bool AddIngredient(Type ing)
    {
        switch (ing)
        {
            case Type.Snow:
                if (snowCount >= snowMax)
                    return false;
                ++snowCount;
                break;
            case Type.Gifts:
                if (giftsCount >= giftsMax)
                    return false;
                ++giftsCount;
                break;
            case Type.Lights:
                if (lightsCount >= lightsMax)
                    return false;
                ++lightsCount;
                break;
            default:
                return false;
        }

        return true;
    }

    public bool RemoveIngredient(Type ing)
    {
        switch (ing)
        {
            case Type.Snow:
                if (snowCount <= 0)
                    return false;
                --snowCount;
                break;
            case Type.Gifts:
                if (giftsCount <= 0)
                    return false;
                --giftsCount;
                break;
            case Type.Lights:
                if (lightsCount <= 0)
                    return false;
                --lightsCount;
                break;
            default: 
                return false;
        }

        return true;
    }
}