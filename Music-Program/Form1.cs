using System;
using System.Windows.Forms;
using WMPLib;  // Windows Media Player library

namespace MusicPlayer
{
    public partial class Form1 : Form
    {
        // === MODEL ===
        private WindowsMediaPlayer player;  // WMP plays MP3, WMA, MP4, etc.
        private string currentFilePath = "";
        private bool isPlaying = false;

        public Form1()
        {
            InitializeComponent();
            InitializePlayer();
        }

        // Create the WMP player
        private void InitializePlayer()
        {
            player = new WindowsMediaPlayer();
            player.PlayStateChange += Player_PlayStateChange;
        }

        // Handle play state changes
        private void Player_PlayStateChange(int newState)
        {
            // Update UI when state changes (0 = stopped, 3 = playing, etc.)
            if (newState == 1) // Stopped
            {
                isPlaying = false;
                UpdateButtonStates();
            }
        }

        // === CONTROLLER METHODS ===

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Support common audio formats
            openFileDialog.Filter = "Audio Files|*.mp3;*.wma;*.wav;*.m4a;*.mp4|MP3 Files|*.mp3|All Files|*.*";
            openFileDialog.Title = "Select a Music File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileName(currentFilePath);
                lblTrackDisplay.Text = "Track: " + fileName;

                // Load file into player
                player.URL = currentFilePath;
                player.controls.stop();  // Don't auto-play yet

                btnPlay.Enabled = true;
                btnStop.Enabled = false;
                btnPause.Enabled = false;
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentFilePath))
            {
                player.controls.play();
                isPlaying = true;
                UpdateButtonStates();
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (isPlaying)
            {
                player.controls.pause();
                isPlaying = false;
                UpdateButtonStates();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            player.controls.stop();
            isPlaying = false;
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            btnPlay.Enabled = !isPlaying;
            btnPause.Enabled = isPlaying;
            btnStop.Enabled = isPlaying;
        }
    }
}