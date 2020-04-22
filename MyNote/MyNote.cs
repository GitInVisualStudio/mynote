using MyNote.Utils.IO;
using MyNoteBase.Canvasses;
using MyNoteBase.Utils.IO;
﻿using MyNote.Gui;
using MyNote.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNoteBase.Classes;
using MyNote.Utils.Render;
using MyNote.Gui.Screen;
using MyNote.Gui.Screens;
using MyNote.Utils.Math;

namespace MyNote
{
    public partial class MyNote : Form
    {
        private const int ANIMATION_UPDATES = 60;
        private System.Windows.Forms.Timer timer;
        private bool shouldUpdate;
        private Thread animationThread;
        private bool opend;
        private Vector Size => new Vector(Width - 16, Height - 39);
        private GuiScreen currentScreen;
        private List<GuiScreen> guiScreens; //Liste von Screens die offen sind => tabs lol
        //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

        public bool ShouldUpdate
        {
            get
            {
                return shouldUpdate;
            }

            set
            {
                if(timer != null)
                    if (value)
                        timer.Start();
                    else
                        timer.Stop();
                shouldUpdate = value;
            }
        }

        public MyNote()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            FormBorderStyle = FormBorderStyle.None;
            Width = 1280;
            Height = 720;
            timer = new System.Windows.Forms.Timer
            {
                Interval = 100
            };
            timer.Start();
            opend = true;
            animationThread = new Thread(() =>
            {
                while (opend)
                {
                    AnimationManager.Update();
                    Thread.Sleep((int)(1000.0f / ANIMATION_UPDATES));
                    if (AnimationManager.animations.Count != 0)
                        ShouldUpdate = true;
                }
            });
            animationThread.Start();
            guiScreens = new List<GuiScreen>();
            StateManager.Push();
            guiScreens.Add(new GuiStartScreen(Size));
            guiScreens.Add(new GuiStartScreen(Size));
            guiScreens.Add(new GuiStartScreen(Size));
            OpenScreen(new GuiStartScreen(Size));
        }

        private void AddEvents()
        {
            MouseClick += (object sender, MouseEventArgs e) => currentScreen?.Component_OnClick(new Utils.Math.Vector(e.X, e.Y));
            MouseDown += (object sender, MouseEventArgs e) => currentScreen?.Component_OnRelease(new Utils.Math.Vector(e.X, e.Y));
            MouseMove += (object sender, MouseEventArgs e) => currentScreen?.Component_OnMove(new Utils.Math.Vector(e.X, e.Y));
            KeyDown += (object sender, KeyEventArgs args) => currentScreen?.Component_OnKeyPress((char)args.KeyValue);
            KeyUp += (object sender, KeyEventArgs args) => currentScreen?.Component_OnKeyRelease((char)args.KeyValue);
        }

        //Ich liebe WinForms <3
        protected override void OnResize(EventArgs e)
        {
            currentScreen?.Component_OnResize(Size);
            this.Refresh();
        }

        public void OpenScreen(GuiScreen screen)
        {
            new Thread(() =>
            {
                if(!guiScreens.Contains(screen))
                    guiScreens.Add(screen);
                screen.Init();
                currentScreen?.Close();
                while (currentScreen != null && currentScreen.Opend)
                    Thread.Sleep(50);
                currentScreen = screen;
                ShouldUpdate = true;
            }).Start();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            opend = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            StateManager.Push();
            #region drawing
            StateManager.Update(e.Graphics);
            StateManager.SetColor(Color.LightGray);
            StateManager.FillRect(0, 0, Width, 20);
            StateManager.SetColor(Color.Black);
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            StateManager.DrawCenteredString("Titel + Exit usw", Width / 2, 10);
            StateManager.Translate(0, 20);
            StateManager.SetColor(Color.DarkGray);
            StateManager.FillRect(0, 0, Width, 30);
            StateManager.SetColor(Color.Black);
            StateManager.DrawCenteredString("OptionPanel mit Zurück, Neu, Suche...", Width / 2, 10);
            StateManager.Translate(0, 30);

            //TODO: Drawing the Screens
            int var1 = 100, var2 = 30, var3 = 12, var4 = 5;
            for (int i = 0; i < guiScreens.Count; i++)
            {
                StateManager.SetColor(guiScreens[i] == currentScreen ? Color.Gray : Color.LightGray);
                StateManager.FillRoundRect(i * (var1 + var4), 0, var1, var2, var3);
                StateManager.SetColor(Color.Black);
                StateManager.DrawCenteredString(guiScreens[i].Name, i * (var1 + var4) + var1 / 2, var2 / 2 - 3);
            }
            StateManager.SetColor(Color.Gray);
            StateManager.FillRect(0, var2 - var3, Width, var3 + 1);

            //currentScreen?.OnRender();
            #endregion stopDrawing
            StateManager.Pop();

            //TODO: StateManager Objekt orientiert??NEIN
            ShouldUpdate = false;
        }
    }
}
