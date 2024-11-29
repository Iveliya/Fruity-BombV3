namespace FormForFruityBomb
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer[] slotTimers;
        private PictureBox[] slots;
        private readonly Image[] slotImages;
        private Random random;
        private bool isSpinning = false;
        private int[] currentImageIndexes;
        private int[] nextImageIndexes;
        private int[] slideOffsets;

        // Variables for balance, bet, and speed
        private decimal balance = 1000m;
        private decimal currentBet = 0.50m;
        private int slotSpeed = 10; // Smaller values make it spin faster

        public Form1()
        {
            InitializeComponent();

            // Initialize images
            slotImages = new Image[]
            {
                Properties.Resources.Leonardo_Phoenix_Create_a_stylized_Bell_symbol_for_a_casino_ga_3_removebg_preview,
                Properties.Resources.Leonardo_Phoenix_create_a_vibrant_stylized_eggplant_symbol_on_3_removebg_preview,
                Properties.Resources.Leonardo_Phoenix_design_a_vibrant_and_stylized_watermelon_slic_0_removebg_preview,
                Properties.Resources.Leonardo_Phoenix_A_stylized_vibrant_cherry_symbol_isolated_on_1_removebg_preview
            };

            random = new Random();
            slots = new PictureBox[] { pictureBox2, pictureBox3, pictureBox4, pictureBox5 };

            slotTimers = new System.Windows.Forms.Timer[slots.Length];
            currentImageIndexes = new int[slots.Length];
            nextImageIndexes = new int[slots.Length];
            slideOffsets = new int[slots.Length];

            for (int i = 0; i < slots.Length; i++)
            {
                slotTimers[i] = new System.Windows.Forms.Timer();
                slotTimers[i].Interval = slotSpeed;  // Set slot speed here
                int slotIndex = i;
                slotTimers[i].Tick += (sender, e) => AnimateSlot(slotIndex);
                slots[i].Paint += (sender, e) => DrawSlot(sender, e, slotIndex);
                currentImageIndexes[i] = random.Next(slotImages.Length);
                nextImageIndexes[i] = (currentImageIndexes[i] + 1) % slotImages.Length;
            }

            UpdateUI();
        }

        private void AnimateSlot(int slotIndex)
        {
            slideOffsets[slotIndex] += 10;

            if (slideOffsets[slotIndex] >= slots[slotIndex].Height)
            {
                slideOffsets[slotIndex] = 0;
                currentImageIndexes[slotIndex] = nextImageIndexes[slotIndex];
                nextImageIndexes[slotIndex] = random.Next(slotImages.Length);
            }

            slots[slotIndex].Invalidate();
        }

        private void DrawSlot(object sender, PaintEventArgs e, int slotIndex)
        {
            var g = e.Graphics;
            int offset = slideOffsets[slotIndex];
            int height = slots[slotIndex].Height;

            g.DrawImage(slotImages[currentImageIndexes[slotIndex]], new Rectangle(0, -offset, slots[slotIndex].Width, height));
            g.DrawImage(slotImages[nextImageIndexes[slotIndex]], new Rectangle(0, height - offset, slots[slotIndex].Width, height));
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            StartSpin();
        }

        private async void StartSpin()
        {
            if (isSpinning || balance < currentBet)
            {
                return;
            }

            isSpinning = true;
            balance -= currentBet;
            UpdateUI();

            foreach (var timer in slotTimers)
            {
                timer.Start();
            }

            await Task.Delay(2000);

            for (int i = 0; i < slots.Length; i++)
            {
                await Task.Delay(250);
                slotTimers[i].Stop();

                slideOffsets[i] = 0;
                currentImageIndexes[i] = random.Next(slotImages.Length);
                nextImageIndexes[i] = (currentImageIndexes[i] + 1) % slotImages.Length;
                slots[i].Invalidate();
            }

            isSpinning = false;
            UpdateUI();
        }

        private void UpdateUI()
        {

            label3.Text = $"${balance:F2}";
            button1.Text = $"Spin (${currentBet:F2})";
        }

        private void SetBet(decimal betAmount)
        {
            currentBet = betAmount;
            UpdateUI();
            StartSpin();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e) { /* Empty */ }
        private void pictureBox2_Click(object sender, EventArgs e) { /* Empty */ }
        private void pictureBox3_Click(object sender, EventArgs e) { /* Empty */ }
        private void pictureBox4_Click(object sender, EventArgs e) { /* Empty */ }
        private void pictureBox5_Click(object sender, EventArgs e) { /* Empty */ }
        private void label1_Click(object sender, EventArgs e) { /* Empty */ }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            SetBet(0.50m);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            SetBet(1.00m);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SetBet(2.50m);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            SetBet(5.00m);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}

