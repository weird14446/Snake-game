namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Screen screen = new Screen(20, 20);
            Snake snake = new Snake(screen);
            while (!snake.isOver)
            {
                snake.InitFood();
                snake.Draw();
                screen.Render();
                snake.MoveAfterInput();
                Thread.Sleep(100);
                Console.Clear();
                screen.Clear();
                Console.WriteLine(snake.score);
            }
            Console.Clear();
            Console.WriteLine("Game Over!\nscore : " + snake.score);
        }
    }

    class Screen
    {
        string[,] board;

        public int I
        {
            get
            {
                return board.GetLength(0);
            }
        }
        public int J
        {
            get
            {
                return board.GetLength(1);
            }
        }

        public string this[int y, int x]
        {
            get
            {
                return board[y, x];
            }
        }

        public Screen(int a, int b)
        {
            board = new string[a, b];
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    board[i, j] = "□";
                }
            }
        }

        public void Render()
        {
            int a = board.GetLength(0), b = board.GetLength(1);
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    Console.Write(board[i, j]);
                }
                Console.WriteLine();
            }
        }

        public void Clear()
        {
            int a = board.GetLength(0), b = board.GetLength(1);
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    if (board[i, j] == "★") continue;
                    board[i, j] = "□";
                }
            }
        }

        public void SettingBlock(int y, int x, string block)
        {
            board[y, x] = block;
        }
    }

    class Snake
    {
        List<int[]> snake = new List<int[]>();
        Screen screen;
        int vec;
        bool isInit = false;
        public bool isOver = false;
        bool isEating = false;
        public int score = 0;

        public Snake(Screen _screen)
        {
            screen = _screen;
            int[] pos = { (int)(screen.I / 2) - 1, (int)(screen.J / 2) - 1 };
            snake.Add(pos);
        }

        public void InitFood()
        {
            if (!isInit)
            {
                Random rand = new Random();
                int y = rand.Next(0, screen.I);
                int x = rand.Next(0, screen.J);
                for (int i = 0; i < snake.Count; i++)
                {
                    if (snake[i][0] == y && snake[i][1] == x)
                    {
                        isInit = false;
                        isEating = true;
                        return;
                    }
                }
                screen.SettingBlock(y, x, "★");
                isInit = true;
            }
        }

        public void Draw()
        {
            for (int i = 0; i < snake.Count(); i++)
            {
                screen.SettingBlock(snake[i][0], snake[i][1], "■");
            }
        }

        public void Move()
        {
            int[] pos = { snake[snake.Count() - 1][0], snake[snake.Count() - 1][1] };
            for (int i = snake.Count() - 1; 0 < i; i--)
            {
                snake[i][0] = snake[i - 1][0];
                snake[i][1] = snake[i - 1][1];
            }
            switch (vec)
            {
                case 0:
                    if (snake[0][0] - 1 < 0)
                    {
                        isOver = true;
                        return;
                    }
                    if (screen[snake[0][0] - 1, snake[0][1]] == "■")
                    {
                        isOver = true;
                        return;
                    }
                    snake[0][0]--;
                    break;
                case 1:
                    if (snake[0][1] - 1 < 0)
                    {
                        isOver = true;
                        return;
                    }
                    if (screen[snake[0][0], snake[0][1] - 1] == "■")
                    {
                        isOver = true;
                        return;
                    }
                    snake[0][1]--;
                    break;
                case 2:
                    if (screen.I <= snake[0][0] + 1)
                    {
                        isOver = true;
                        return;
                    }
                    if (screen[snake[0][0] + 1, snake[0][1]] == "■")
                    {
                        isOver = true;
                        return;
                    }
                    snake[0][0]++;
                    break;
                case 3:
                    if (screen.J <= snake[0][1] + 1)
                    {
                        isOver = true;
                        return;
                    }
                    if (screen[snake[0][0], snake[0][1] + 1] == "■")
                    {
                        isOver = true;
                        return;
                    }
                    snake[0][1]++;
                    break;
            }
            if (screen[snake[0][0], snake[0][1]] == "★" || isEating)
            {
                screen.SettingBlock(snake[0][0], snake[0][1], "□");
                isInit = false;
                isEating = false;
                snake.Add(pos);
                score++;
            }
        }

        public void MoveAfterInput()
        {
            if (Console.KeyAvailable == false)
            {
                Move();
                return;
            }
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.W:
                    vec = 0;
                    Move();
                    break;
                case ConsoleKey.A:
                    vec = 1;
                    Move();
                    break;
                case ConsoleKey.S:
                    vec = 2;
                    Move();
                    break;
                case ConsoleKey.D:
                    vec = 3;
                    Move();
                    break;
                default:
                    return;
            }
        }
    }
}