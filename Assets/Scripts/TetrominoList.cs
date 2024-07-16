using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TetrominoList", menuName = "Scriptable Object/TetrominoList")]
public class TetrominoList : ScriptableObject
{
    public GameObject[] tetrominoPrefabs;
}
