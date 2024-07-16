using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoControl : MonoBehaviour
{
    // 블록이 떨어지는 속도 (초 단위)
    public float fallSpeed = 1.0f;
    private float fallTimer = 0f; // 블록이 떨어지기까지 경과된 시간

    private GameBoard gameBoard; // 게임 보드를 참조하는 변수

    void Start()
    {
        // 게임 보드 객체를 찾아서 gameBoard 변수에 할당
        gameBoard = FindObjectOfType<GameBoard>();
        Debug.Log("Tetromino spawned at: " + transform.position);
    }

    void Update()
    {
        // 블록 자동 낙하 1초마다 1씩 아래로 내려감
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallSpeed)
        {
            Move(Vector3.down); // 블록을 아래로 한 칸 이동
            fallTimer = 0.0f; // 타이머 초기화
        }

        // 이동과 회전
        // A는 회전 WD로 좌우 이동, S로 한단계 아래 내리기 Space로 빠르게 내리기

        // W : Rotate
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Tetromino Rotate at: " + transform.position);

            Rotate();
        }

        // S : 아래로 한칸 이동
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Tetromino down at: " + transform.position);

            Move(Vector3.down);
        }

        // A : 왼쪽으로 한칸 이동
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Tetromino left at: " + transform.position);

            Move(Vector3.left);
        }

        // D : 오른쪽으로 한칸 이동
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Tetromino right at: " + transform.position);

            Move(Vector3.right);
        }

        // SpaceBar : 한번에 내려가기
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (ValidMove())
            {
                Move(Vector3.down);
            }
            
            Debug.Log("Tetromino Space to bottom at: " + transform.position);
        }
    }
    
    private void Move(Vector3 direction)
    {
        transform.position += direction;

        if (!ValidMove())
        {
            transform.position -= direction;

            if (direction == Vector3.down)
            {
                LockBlock();
            }
        }

        // UpdateGameBoard();
        Debug.Log("Tetromino Move at: " + transform.position);

    }

    private void LockBlock()
    {
        Debug.Log("Tetromino locked at: " + transform.position);

        AddToGrid();
        CheckForCompletedRows();
        this.enabled = false; // 블록 고정
        
        // 바닥에 닿으면 새로운 테트리스 블록을 다시 스폰한다. 
        GameManager.Instance.CreateNewTetromino();
        
    }
    
    private void Rotate()
    {
        transform.Rotate(0, 0, -90);

        if (ValidMove())
        {
            // UpdateGameBoard();
        }
        else
        {
            transform.Rotate(0, 0, 90);
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform) // 4씩 존재함.
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);
            
            Debug.Log($"Child Position: {children.transform.position}, RoundX : {roundedX}, RoundY : {roundedY}");
            
            if (roundedX < 0 || roundedX >= GameBoard.width || roundedY < 0 || roundedY >= GameBoard.height)
                return false; // 그리드 좌표를 벗어나면 false
            
            // 다른 블록과 충돌하면 false 반환
            if (gameBoard.grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }
        
        return true;
    }

    private void UpdateGameBoard()
    {
        // 현재 블록의 모든 위치를 그리드에서 제거
        for (int y = 0; y < GameBoard.height; ++y)
        {
            for (int x = 0; x < GameBoard.width; ++x)
            {
                if (gameBoard.grid[x, y] != null && gameBoard.grid[x, y].parent == transform)
                {
                    gameBoard.grid[x, y] = null;
                }
            }
        }

        // 새로운 위치에 블록 추가
        foreach (Transform child in transform)
        {
            Vector2 pos = GameBoard.Round(child.position);
            gameBoard.grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = GameBoard.Round(child.position);
            gameBoard.grid[(int)pos.x, (int)pos.y] = child;
            
            // // 게임 보드 범위 검사
            // if (pos.x >= 0 && pos.x < GameBoard.width && pos.y >= 0 && pos.y < GameBoard.height)
            // {
            //     gameBoard.grid[(int)pos.x, (int)pos.y] = child;
            // }
            // else
            // {
            //     Debug.LogWarning($"Block at position {pos} is out of bounds and will not be added to the grid.");
            // }
        }
        
        // foreach (Transform child in transform)
        // {
        //     Vector2 pos = GameBoard.Round(child.position);
        //     gameBoard.grid[(int)pos.x, (int)pos.y] = child;
        // }
    }

    // 가로 라인 블록이 가득 차있는지 체크
    private void CheckForCompletedRows()
    {
        for (int y = 0; y < GameBoard.height; y++)
        {
            if (gameBoard.IsRowFull(y))
            {
                gameBoard.RemoveRow(y);
                RowDown(y);
                
                // 게임 점수 추가하기
                GameManager.Instance.AddScore();
            }
        }
    }

    // 해당 라인의 블록을 아래로 내려주기
    private void RowDown(int row)
    {
        for (int y = row; y < GameBoard.height - 1; y++)
        {
            for (int x = 0; x < GameBoard.width; x++)
            {
                if (gameBoard.grid[x, y] != null)
                {
                    // 현재 위치의 블록을 아래로 이동
                    gameBoard.grid[x, y - 1] = gameBoard.grid[x, y];
                    gameBoard.grid[x, y] = null;
                    gameBoard.grid[x, y - 1].position += Vector3.down;
                }
            }
        }
    }
} // end class