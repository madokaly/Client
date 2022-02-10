using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    //游戏角色数组
    public GameObject[] characters;

    private int currentCharacter = 0;

    public int CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            UpdateCharacter();
        }
    }

    private void UpdateCharacter()
    {
        for(int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(i == currentCharacter);
        }
    }
}
