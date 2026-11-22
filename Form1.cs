namespace MiniArkanoid;

public class Form1 : Form
{
    private readonly PictureBox ball = new();
    private readonly Panel panel1 = new();
    private readonly Label scoreLabel = new();
    private readonly System.Windows.Forms.Timer timer = new();
    private int speedX, speedY, score;

    public Form1()
    {
        Text = "Mini Arkanoid";
        ClientSize = new Size(788, 520);
        BackColor = Color.White;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        KeyPreview = true;

        ball.Size = new Size(64, 64);
        ball.SizeMode = PictureBoxSizeMode.Zoom;
        ball.Image = Image.FromFile(Path.Combine(AppContext.BaseDirectory, "assets", "ball.png"));
        panel1.Size = new Size(151, 20);
        panel1.BackColor = Color.Black;
        scoreLabel.AutoSize = true;
        scoreLabel.Location = new Point(12, 8);
        Controls.AddRange(new Control[] { ball, panel1, scoreLabel });

        timer.Interval = 20;
        timer.Tick += MoveBall;
        KeyDown += Form1_KeyDown;
        Shown += (sender, e) => StartGame();
    }

    private void StartGame()
    {
        score = 0;
        scoreLabel.Text = "Xal: 0";
        speedX = 4;
        speedY = 4;
        ball.Location = new Point(70, 32);
        panel1.Location = new Point((ClientSize.Width - panel1.Width) / 2, 434);
        timer.Start();
    }

    private void Form1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Left)
            panel1.Left = Math.Max(0, panel1.Left - 20);
        if (e.KeyCode == Keys.Right)
            panel1.Left = Math.Min(ClientSize.Width - panel1.Width, panel1.Left + 20);
    }

    private void MoveBall(object? sender, EventArgs e)
    {
        int x = ball.Left + speedX;
        int y = ball.Top + speedY;
        if (x <= 0 || x + ball.Width >= ClientSize.Width)
        {
            speedX = -speedX;
            x = Math.Clamp(x, 0, ClientSize.Width - ball.Width);
        }
        if (y <= 0)
        {
            speedY = Math.Abs(speedY);
            y = 0;
        }

        if (speedY > 0 && ball.Bottom <= panel1.Top && y + ball.Height >= panel1.Top)
        {
            if (x < panel1.Right && x + ball.Width > panel1.Left)
            {
                score++;
                scoreLabel.Text = $"Xal: {score}";
                speedY = -speedY;
                y = panel1.Top - ball.Height;
            }
            else
            {
                timer.Stop();
                ball.Location = new Point(x, y);
                MessageBox.Show(this, $"Oyun bitdi! Yekun xal: {score}\nYeni oyun üçün OK düyməsinə basın.",
                    Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                StartGame();
                return;
            }
        }
        ball.Location = new Point(x, y);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            timer.Dispose();
            ball.Image?.Dispose();
        }
        base.Dispose(disposing);
    }
}
