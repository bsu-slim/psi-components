using Microsoft.Psi;
using Microsoft.Psi.Audio;
using Microsoft.Psi.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PipelineControl;
using GoogleASRComponent;
using GoogleTranslateComponent;

namespace PsiComponents
{
    public partial class GUI : Form
    {
        private bool Speak;
        private DataFaucet<AudioBuffer> faucet;
        private AggregateDump dump;
        private GoogleASR speechRec;
        private GoogleTranslate translate;

        public GUI(DataFaucet<AudioBuffer> df, AggregateDump dump, GoogleASR asr, GoogleTranslate gt)
        {
            Speak = false;
            InitializeComponent();
            this.faucet = df;
            this.dump = dump;
            this.speechRec = asr;
            this.translate = gt;
            System.Timers.Timer myTimer = new System.Timers.Timer();
        }

        public void DisplayText(string text)
        {
            this.TextDisplay.Text = text;
        }

        private void SwapButton_Click(object sender, EventArgs e)
        {
            if (Speak)
            {
                StopSpeaking();
            }
            var selectedInputLang = inputLang.SelectedIndex;
            var selectedOutpuLang = outputLang.SelectedIndex;
            inputLang.SetSelected(selectedOutpuLang, true);
            outputLang.SetSelected(selectedInputLang, true);

            //Here we adjust the pipeline language wrapper component to start wrapping the audio with the newly selected languages
            string audioLang = this.languages[inputLang.SelectedIndex];
            this.speechRec.AudioLanguage = audioLang;
            this.translate.InputLanguageCode = audioLang;
            this.translate.OutputLanguageCode = this.languages[outputLang.SelectedIndex];
        }


        private void SpeakButton_Click(object sender, EventArgs e)
        {
            if (Speak)
            {
                StopSpeaking();
            }
            else
            {
                StartSpeaking();
            }
        }

        private void InputLang_Click(object sender, EventArgs e)
        {
            StopSpeaking();
            string audioLang = this.languages[inputLang.SelectedIndex];
            this.speechRec.AudioLanguage = audioLang;
            this.translate.InputLanguageCode = audioLang;
        }

        private void OutputLang_Click(object sender, EventArgs e)
        {
            StopSpeaking();
            this.translate.OutputLanguageCode = this.languages[outputLang.SelectedIndex];
        }

        private void StartSpeaking()
        {
            if (!Speak) //If we are speaking already we don't need to start... This is just a safeguard from future programmers
            {
                speakButton.Text = "Stop";
                Speak = !Speak;
                this.faucet.Open = true;
            }
        }

        private void StopSpeaking()
        {
            if (Speak) //If we aren't speaking we don't need to stop... This is just a safeguard from future programmers
            {
                speakButton.Text = "Speak";
                Speak = !Speak;
                this.faucet.Open = false;
                this.dump.Dump();
            }
        }
    }
}
