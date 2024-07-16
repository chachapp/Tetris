using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 테트리스 블록이 랜덤하게 나오는 스포너. 
/// </summary>
public class Spawner : MonoBehaviour
{
    public TetrominoList tetrominoList;
    private Vector3 spawnPosition = new Vector3(8.5f, 26.5f, 1);
    
    void Start()
    {
        CreateTetris();
    }

    public void CreateTetris()
    {
        if (tetrominoList != null)
        {
            GameObject[] tetrominos = tetrominoList.tetrominoPrefabs;
            GameObject tetromino = Instantiate(tetrominos[Random.Range(0, tetrominos.Length)], spawnPosition, Quaternion.identity);

            //tetromino.transform.localPosition = spawnPosition;
            Debug.Log("Spawned Tetromino at: " + spawnPosition);
        }
      }
} // end class
