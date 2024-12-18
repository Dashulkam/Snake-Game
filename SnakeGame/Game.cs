using SnakeGame;
using System.Collections.Generic;
using System.Windows;
using System;

public class Game
{
    public Snake Snake { get; private set; }
    public Food Food { get; private set; }
    public Score Score { get; private set; }
    public List<Obstacle> Obstacles { get; private set; } // Список препятствий

    private int boardWidth;
    private int boardHeight;

    public Game(int width, int height)
    {
        boardWidth = width;
        boardHeight = height;
        Snake = new Snake(100, 100);
        Food = new Food(200, 200);
        Score = new Score();
        Obstacles = new List<Obstacle>();
       
    }

    

    public void InitializeObstacles(string level)
    {
        Obstacles.Clear(); // Очищаем предыдущие препятствия

        Random random = new Random();
        int obstacleCount = 0;

        switch (level)
        {
            case "Easy":
                // На легком уровне препятствий нет
                break;
            case "Medium":
                obstacleCount = 5; 
                break;
            case "Hard":
                obstacleCount = 10;
                break;
        }

        for (int i = 0; i < obstacleCount; i++)
        {
            double x = random.Next(0, boardWidth / 20) * 20; // Генерация позиции по оси X
            double y = random.Next(0, boardHeight / 20) * 20; // Генерация позиции по оси Y
            Obstacles.Add(new Obstacle(x, y)); // Добавляем новое препятствие
        }
    }

    public void Update(IGameUI gameUI)
    {
        Snake.Move(this);

        var head = Snake.Segments[0].Position;

        if (head.X < 0 || head.X >= boardWidth ||
            head.Y < 0 || head.Y >= boardHeight ||
            Snake.CollidesWithSelf() || CollidesWithObstacles(head))
        {
            gameUI.ShowGameOver();
            return;
        }

        if (head == Food.Position)
        {
            Snake.Grow();
            Score.IncreaseScore(10);
            gameUI.UpdateScore(Score.CurrentScore);
            Food.GenerateNewPosition(boardWidth, boardHeight, Snake.Segments, Obstacles); // Генерация новой позиции еды
        }

        gameUI.RefreshBoard();
    }



    private bool CollidesWithObstacles(Point headPosition)
    {
        foreach (var obstacle in Obstacles)
        {
            if (headPosition == obstacle.Position)
                return true;
        }
        return false;
    }
}


