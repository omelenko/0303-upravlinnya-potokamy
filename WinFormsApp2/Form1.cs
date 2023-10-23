namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        static Thread thread;
        static CancellationTokenSource tokenSource = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int a = int.Parse(textBox1.Text), b = int.Parse(textBox2.Text) * 1000, c = int.Parse(textBox3.Text);
            button1.Enabled = false;
            button2.Enabled = true;
            thread = new Thread(new ThreadStart(() => Task.Run(() => Func(tokenSource.Token, a, b, c))));
            thread.Start();
            thread.Join();
        }

        private void Func(CancellationToken ct, int a, int b, int c)
        {
            try
            {
                for (int i = 0; i < c; i++)
                {
                    Console.Beep(a, b);
                    if (ct.IsCancellationRequested)
                    {
                        break; 
                    }
                }
            }
            catch
            {
                MessageBox.Show("Помилка!");
            }
            button1.BeginInvoke((Action)delegate ()
            {
                button1.Enabled = true;
            });
            button2.BeginInvoke((Action)delegate ()
            {
                button2.Enabled = false;
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel(); 
            tokenSource.Dispose();
            tokenSource = new();
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}