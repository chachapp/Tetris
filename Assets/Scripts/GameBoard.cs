using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static int width = 20;
    public static int height = 30;
    public Transform[,] grid = new Transform[width, height];
    
    // 특정 좌표가 보드 내에 있는지 확인
    public static bool InsideBorder(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < width && (int)pos.y >= 0);
    }

    // 특정 좌표의 블록을 그리드에 추가
    public void AddToGrid(Transform block)
    {
        Vector2 pos = Round(block.position);
        grid[(int)pos.x, (int)pos.y] = block;
    }

    // 블록의 좌표를 반올림
    public static Vector2 Round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    // 주어진 행이 가득 찼는지 확인
    public bool IsRowFull(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    // 특정 행의 모든 블록을 제거
    public void RemoveRow(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }

        // 위의 모든 행을 한 칸 아래로 이동
        for (int i = y + 1; i < height; ++i)
        {
            MoveRowDown(i);
        }
    }

    // 특정 행의 모든 블록을 한 칸 아래로 이동
    private void MoveRowDown(int y)
    {
        for (int x = 0; x < width; ++x)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    // 그리드를 시각적으로 표시하는 메서드 (디버깅용)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow; // 그리드의 색상을 노란색으로 설정
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 그리드에 블록이 있는 경우 해당 위치에 큐브를 그린다
                if (grid[x, y] != null)
                {
                    Gizmos.DrawCube(new Vector3(x, y, 0), Vector3.one);
                }
                else
                {
                    // 그리드에 블록이 없는 경우, 격자선을 그린다
                    Gizmos.DrawWireCube(new Vector3(x, y, 0), Vector3.one);
                }
            }
        }
    }
}
