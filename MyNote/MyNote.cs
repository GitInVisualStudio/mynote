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
        private bool shouldUpdate;
        private Thread animationThread;
        private bool opend;
        public new Vector Size => new Vector(Width, Height);
        private GuiMainScreen mainScreen;
        //System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

        public MyNote()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            //FormBorderStyle = FormBorderStyle.None;
            DoubleBuffered = true;
            Width = 1280;
            Height = 720;
            opend = true;
            animationThread = new Thread(() =>
            {
                while (opend)
                {
                    AnimationManager.Update();
                    Thread.Sleep((int)(1000.0f / ANIMATION_UPDATES));
                    if (AnimationManager.animations.Count != 0)
                    {
                        //Refresh(); IDK how to fix this lol
                    }
                }
            });
            AddEvents();
            animationThread.Start();
            StateManager.Push();
            mainScreen = new GuiMainScreen(this)
            {
                RWidth = 1,
                RHeight = 1
            };
            mainScreen.Init();
        }

        private void AddEvents()
        {
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                mainScreen?.Component_OnClick(location);
            };
            MouseUp += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                mainScreen?.Component_OnRelease(location);
            };
            KeyDown += (object sender, KeyEventArgs args) =>
            {
                mainScreen?.Component_OnKeyPress((char)args.KeyValue);
            };
            KeyUp += (object sender, KeyEventArgs args) =>
            {
                mainScreen?.Component_OnKeyRelease((char)args.KeyValue);
            };
            MouseMove += (object sender, MouseEventArgs e) =>
            {
                Vector location = new Vector(e.X, e.Y);
                mainScreen?.Component_OnMove(location);
            };
            Resize += (object sender, EventArgs args) => 
            {
                mainScreen?.Component_OnResize(Size);
            };
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
            StateManager.SetFont(FontUtils.DEFAULT_FONT);
            mainScreen.OnRender();
            //StateManager.SetColor(Color.LightGray);
            //StateManager.FillRect(0, 0, Width, 20);
            //StateManager.SetColor(Color.Black);
            //StateManager.DrawCenteredString("Titel + Exit usw", Width / 2, 10);
            //StateManager.Translate(0, 20);
            //StateManager.SetColor(Color.DarkGray);
            //StateManager.FillRect(0, 0, Width, 30);
            //StateManager.SetColor(Color.Black);
            //StateManager.DrawCenteredString("OptionPanel mit Zurück, Neu, Suche...", Width / 2, 10);
            //StateManager.Translate(0, 30);

            ////TODO: Drawing the Screens
            //int var1 = 100, var2 = 30, var3 = 12, var4 = 5;
            //StateManager.SetColor(Color.LightGray);
            //StateManager.FillRoundRect(0, 0, var1, var2, var3);
            //StateManager.SetColor(Color.Black);
            //StateManager.DrawCenteredString("+", var1 / 2, var2 / 2 - 3);
            //for (int i = 0; i < guiScreens.Count; i++)
            //{
            //    StateManager.SetColor(guiScreens[i] == currentScreen ? Color.Gray : Color.LightGray);
            //    StateManager.FillRoundRect((i + 1) * (var1 + var4), 0, var1, var2, var3);
            //    StateManager.SetColor(Color.Black);
            //    StateManager.DrawCenteredString(guiScreens[i].Name, (i + 1) * (var1 + var4) + var1 / 2, var2 / 2 - 3);
            //}
            //StateManager.SetColor(Color.Gray);
            //StateManager.FillRect(0, var2 - var3, Width, var3 + 1);

            //currentScreen?.OnRender();
            #endregion stopDrawing
            StateManager.Pop();

            //TODO: StateManager Objekt orientiert??NEIN
        }
    }
}
