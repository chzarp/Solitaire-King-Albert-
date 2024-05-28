using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ПасьянсТЕСТ
{
    public partial class Game : Form
    {
        private Deck deck;
        private List<List<Card>> columns;
        private List<Card> reserve;
        private List<Card>[] foundations;
        private Card[] freeCells;

        private string playerName;

        private PictureBox selectedCardPictureBox;
        private Card selectedCard;
        private List<Card> selectedCardSource;

        private System.Windows.Forms.Timer gameTimer;
        private int elapsedTime;

        public Game()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void CardPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox cardPictureBox = sender as PictureBox;
            if (cardPictureBox != null)
            {
                selectedCardPictureBox = cardPictureBox;
                selectedCard = (Card)cardPictureBox.Tag;

                if (selectedCardPictureBox != null)
                {
                    selectedCardPictureBox.BorderStyle = BorderStyle.None;
                }
                selectedCardSource = null;
                for (int i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Contains(selectedCard) && columns[i][columns[i].Count - 1] == selectedCard)
                    {
                        selectedCardSource = columns[i];
                        break;
                    }
                }
                if (reserve.Contains(selectedCard))
                {
                    selectedCardSource = reserve;
                }
                for (int i = 0; i < freeCells.Length; i++)
                {
                    if (freeCells[i] == selectedCard)
                    {
                        selectedCardSource = new List<Card> { freeCells[i] };
                        break;
                    }
                }
                for (int i = 0; i < foundations.Length; i++)
                {
                    if (foundations[i].Contains(selectedCard) && foundations[i][foundations[i].Count - 1] == selectedCard)
                    {
                        selectedCardSource = foundations[i];
                        break;
                    }
                }
                if (selectedCardSource == null)
                {
                    selectedCardPictureBox = null;
                    selectedCard = null;
                }
                else
                {
                    selectedCardPictureBox.BorderStyle = BorderStyle.Fixed3D;
                }
            }
        }

        private void CardPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox cardPictureBox = sender as PictureBox;
            if (cardPictureBox != null && selectedCard != null)
            {
                Card targetCard = (Card)cardPictureBox.Tag;
                bool moved = false;

                if (CanMoveToFoundation(selectedCard))
                {
                    MoveToFoundation(selectedCard);
                    selectedCardSource.Remove(selectedCard);
                    moved = true;
                }
                else
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if ((columns[i].Contains(targetCard) || columns[i].Count == 0) && CanMoveToColumn(selectedCard, i))
                        {
                            MoveToColumn(selectedCard, i);
                            selectedCardSource.Remove(selectedCard);
                            moved = true;                           
                            break;
                        }
                    }
                }
                if (moved)
                {
                    UpdateUI();
                }
            }

            if (selectedCardPictureBox != null)
            {
                selectedCardPictureBox.BorderStyle = BorderStyle.None;
            }
            selectedCardPictureBox = null;
            selectedCard = null;
            selectedCardSource = null;
        }

        private void FreeCellPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (selectedCard != null && CanMoveToFreeCell())
            {
                MoveToFreeCell(selectedCard);
                selectedCardSource.Remove(selectedCard);
                UpdateUI();
            }

            if (selectedCardPictureBox != null)
            {
                selectedCardPictureBox.BorderStyle = BorderStyle.None;
            }
            selectedCardPictureBox = null;
            selectedCard = null;
            selectedCardSource = null;
        }

        private void FoundationCardPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox cardPictureBox = sender as PictureBox;
            if (cardPictureBox != null)
            {
                selectedCardPictureBox = cardPictureBox;
                selectedCard = (Card)cardPictureBox.Tag;

                
                selectedCardSource = null;
                for (int i = 0; i < foundations.Length; i++)
                {
                    if (foundations[i].Contains(selectedCard) && foundations[i][foundations[i].Count - 1] == selectedCard)
                    {
                        selectedCardSource = foundations[i];
                        break;
                    }
                }
                selectedCardPictureBox.BorderStyle = BorderStyle.Fixed3D;
            }
        }

        private void FoundationCardPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox cardPictureBox = sender as PictureBox;
            if (cardPictureBox != null && selectedCard != null)
            {
                Card targetCard = (Card)cardPictureBox.Tag;

                for (int i = 0; i < columns.Count; i++)
                {
                    if ((columns[i].Count == 0 || columns[i][columns[i].Count - 1].CardRank == selectedCard.CardRank + 1) &&
                        (columns[i].Count == 0 || (columns[i][columns[i].Count - 1].CardSuit == Card.Suit.Hearts || columns[i][columns[i].Count - 1].CardSuit == Card.Suit.Diamonds) != (selectedCard.CardSuit == Card.Suit.Hearts || selectedCard.CardSuit == Card.Suit.Diamonds)))
                    {
                        MoveToColumn(selectedCard, i); 
                        selectedCardSource.Remove(selectedCard); 
                        UpdateUI(); 
                        break;
                    }
                }
            }

            // Сброс выделения карты
            if (selectedCardPictureBox != null)
            {
                selectedCardPictureBox.BorderStyle = BorderStyle.None;
            }
            selectedCardPictureBox = null;
            selectedCard = null;
            selectedCardSource = null;
        }

        private void InitializeGame()
        {
            columns = new List<List<Card>>();
            for (int i = 0; i < 9; i++)
            {
                columns.Add(new List<Card>());
            }

            reserve = new List<Card>();

            foundations = new List<Card>[4];
            for (int i = 0; i < 4; i++)
            {
                foundations[i] = new List<Card>();
            }

            freeCells = new Card[4];

            gameTimer = new System.Windows.Forms.Timer();
            gameTimer.Interval = 1000;
            gameTimer.Tick += OnTimerTick;
            elapsedTime = 0;

            UpdateUI();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            elapsedTime++;
            UpdateTimerLabel();
        }

        private void UpdateUI()
        {
            this.Controls.Clear();
            this.Controls.Add(startButton);
            this.Controls.Add(timerLabel);

            int xOffset = 10;
            int yOffset = 10;

            // Отображение карт бельгийского резерва
            if (reserve.Count == 0) 
            {
                PictureBox emptyReservePictureBox = new PictureBox
                {
                    Location = new Point(500, 10),
                    Size = new Size(70, 100),
                    BorderStyle = BorderStyle.FixedSingle,
                };
                this.Controls.Add(emptyReservePictureBox);
            }
            else
            {
                for (int i = 0; i < reserve.Count; i++)
                {
                    var card = reserve[i];
                    PictureBox cardPictureBox = new PictureBox
                    {
                        Image = LoadCardImage(card),
                        Location = new Point(500 - (i * 30), 10),
                        Size = new Size(70, 100),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Tag = card,
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.Transparent
                    };
                    cardPictureBox.MouseDown += CardPictureBox_MouseDown;
                    cardPictureBox.MouseUp += CardPictureBox_MouseUp;
                    this.Controls.Add(cardPictureBox);
                }
            }


            // Отображение баз (фундаментов) в правом верхнем углу
            for (int i = 0; i < foundations.Length; i++)
            {
                if (foundations[i].Count > 0)
                {
                    var card = foundations[i][foundations[i].Count - 1];
                    PictureBox cardPictureBox = new PictureBox
                    {
                        Image = LoadCardImage(card),
                        Location = new Point(650 + (i * 80), 10),
                        Size = new Size(70, 100),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Tag = card,
                    };
                    cardPictureBox.MouseDown += CardPictureBox_MouseDown;
                    cardPictureBox.MouseUp += FoundationCardPictureBox_MouseUp;
                    this.Controls.Add(cardPictureBox);
                }
                else
                {
                    
                    PictureBox emptyFoundationPictureBox = new PictureBox
                    {
                        Location = new Point(650 + (i * 80), 10),
                        Size = new Size(70, 100),
                        BorderStyle = BorderStyle.FixedSingle, 
                    };
                    this.Controls.Add(emptyFoundationPictureBox);
                }
            }


            // Отображение колонок под резервом и базами
            yOffset = 150;
            xOffset = 10;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].Count == 0) // Если столбец пустой - отображаем его как свободный столбец
                {
                    PictureBox freeColumnPictureBox = new PictureBox
                    {
                        Location = new Point(xOffset + (i * 80), yOffset),
                        Size = new Size(70, 100),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        BackColor = Color.LightGreen, 
                        BorderStyle = BorderStyle.FixedSingle 
                    };
                    freeColumnPictureBox.Click += FreeColumnPictureBox_Click; 
                    this.Controls.Add(freeColumnPictureBox);
                }
                else
                {
                    for (int j = columns[i].Count - 1; j >= 0; j--) 
                    {
                        var card = columns[i][j];
                        PictureBox cardPictureBox = new PictureBox
                        {
                            Image = LoadCardImage(card),
                            Location = new Point(xOffset + (i * 80), yOffset + (j * 30)), 
                            Size = new Size(70, 100),
                            SizeMode = PictureBoxSizeMode.StretchImage,
                            Tag = card,
                            BorderStyle = BorderStyle.FixedSingle,
                            BackColor = Color.Transparent
                        };
                        cardPictureBox.MouseDown += CardPictureBox_MouseDown;
                        cardPictureBox.MouseUp += CardPictureBox_MouseUp;
                        this.Controls.Add(cardPictureBox);
                    }
                }
            }

            // Отображение свободных ячеек
            yOffset = 10;
            xOffset = 10 + (reserve.Count * 80);
            for (int i = 0; i < freeCells.Length; i++)
            {
                PictureBox cellPictureBox = new PictureBox
                {
                    Location = new Point(xOffset + (i * 80), yOffset),
                    Size = new Size(70, 100),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                };
                this.Controls.Add(cellPictureBox);

                if (freeCells[i] != null)
                {
                    PictureBox cardPictureBox = new PictureBox
                    {
                        Image = LoadCardImage(freeCells[i]),
                        Location = new Point(xOffset + (i * 80), yOffset),
                        Size = new Size(70, 100),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Tag = freeCells[i],
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.Transparent
                    };
                    cardPictureBox.MouseDown += CardPictureBox_MouseDown;
                    cardPictureBox.MouseUp += FreeCellPictureBox_MouseUp;
                    this.Controls.Add(cardPictureBox);
                }
            }
        }


        private void FreeColumnPictureBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
        }


        private Image LoadCardImage(Card card)
        {
            string cardImagePath = $"CardTextures/card_{card.CardSuit}_{card.CardRank}.png";
            return Image.FromFile(cardImagePath);
        }

        private void UpdateTimerLabel()
        {
            timerLabel.Text = $"Время: {elapsedTime / 60:D2}:{elapsedTime % 60:D2}";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PlayerNamePrompt playerNamePrompt = new PlayerNamePrompt();
            if (playerNamePrompt.ShowDialog() == DialogResult.OK)
            {
                playerName = playerNamePrompt.PlayerName;
                this.Text = $"Пасьянс Король Альберт - игра игрока {playerName}";
                InitializeGame();
            }
            else
            {
                this.Close();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            deck = new Deck();
            deck.Shuffle();

            for (int i = 0; i < 9; i++)
            {
                columns[i].Clear();
                for (int j = 0; j < 9 - i; j++)
                {
                    columns[i].Add(deck.DrawCard());
                }
            }

            reserve.Clear();
            for (int i = 0; i < 7; i++)
            {
                reserve.Add(deck.DrawCard());
            }

            foreach (var foundation in foundations)
            {
                foundation.Clear();
            }

            Array.Clear(freeCells, 0, freeCells.Length);

            elapsedTime = 0;
            gameTimer.Start();

            UpdateUI();
        }

        private void CheckForWin()
        {
            bool hasWon = true;
            foreach (var foundation in foundations)
            {
                if (foundation.Count != 13)
                {
                    hasWon = false;
                    break;
                }
            }

            if (hasWon)
            {
                gameTimer.Stop();
                MessageBox.Show($"Поздравляем {playerName}!, вы прошли игру за {elapsedTime / 60:D2}:{elapsedTime % 60:D2}", "Победа!");
            }
        }

        private bool CanMoveToFoundation(Card card)
        {
            int foundationIndex = (int)card.CardSuit;
            if (foundations[foundationIndex].Count == 0)
            {
                return card.CardRank == Card.Rank.Ace;
            }
            Card topCard = foundations[foundationIndex][foundations[foundationIndex].Count - 1];
            return topCard.CardRank + 1 == card.CardRank && topCard.CardSuit == card.CardSuit;
        }

        private void MoveToFoundation(Card card)
        {
            int foundationIndex = (int)card.CardSuit;
            foundations[foundationIndex].Add(card);
            CheckForWin();
        }

        private bool CanMoveToColumn(Card card, int columnIndex)
        {
            if (columns[columnIndex].Count == 0)
            {
                return true;
            }
            Card topCard = columns[columnIndex][columns[columnIndex].Count - 1];
            return topCard.CardRank == card.CardRank + 1 &&
                   (topCard.CardSuit == Card.Suit.Hearts || topCard.CardSuit == Card.Suit.Diamonds) !=
                   (card.CardSuit == Card.Suit.Hearts || card.CardSuit == Card.Suit.Diamonds);
        }

        private void MoveToColumn(Card card, int columnIndex)
        {
            columns[columnIndex].Add(card);
        }

        private bool CanMoveToFreeCell()
        {
            for (int i = 0; i < freeCells.Length; i++)
            {
                if (freeCells[i] == null)
                {
                    return true;
                }
            }
            return false;
        }

        private void MoveToFreeCell(Card card)
        {
            for (int i = 0; i < freeCells.Length; i++)
            {
                if (freeCells[i] == null)
                {
                    freeCells[i] = card;
                    break;
                }
            }
        }
    }
}
