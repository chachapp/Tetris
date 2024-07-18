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
    
    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            Vector2 pos = GameBoard.Round(child.position);
            gameBoard.grid[(int)pos.x, (int)pos.y] = child;
        }
    }
    
    private void CheckForCompletedRows()
    {
        for (int y = 0; y < GameBoard.height; y++)
        {
            if (gameBoard.IsRowFull(y))
            {
                gameBoard.RemoveRow(y);
            
                // 게임 점수 추가하기
                GameManager.Instance.AddScore();
            
                // 현재 행이 제거되었으므로 다시 체크
                y--;
            }
        }
    }
    
    private void RowDown(int startRow)
    {
        for (int y = startRow; y > 0; y--)
        {
            for (int x = 0; x < GameBoard.width; x++)
            {
                Debug.Log("RowDown : ");
                
                gameBoard.grid[x, y] = gameBoard.grid[x, y - 1];
             
                if (gameBoard.grid[x, y] != null)
                {
                    gameBoard.grid[x, y].position += Vector3.down;
                }
            }
        }

        // 최상단 행을 비웁니다.
        for (int x = 0; x < GameBoard.width; x++)
        {
            gameBoard.grid[x, 0] = null;
        }
    }

} // end class
