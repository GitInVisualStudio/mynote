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

namespace MyNote
{
    public partial class MyNote : Form
    {
        private const int ANIMATION_UPDATES = 60;
        private GuiScreen currentScreen;
        private System.Windows.Forms.Timer timer;
        private bool shouldUpdate;
        private Thread animationThread;
        private bool opend;

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
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000;
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
            OpenScreen(new GuiHeader());
        }

        private void AddEvents()
        {
            MouseClick += (object sender, MouseEventArgs e) => currentScreen?.Component_OnClick(new Utils.Math.Vector(e.X, e.Y));
            MouseDown += (object sender, MouseEventArgs e) => currentScreen?.Component_OnRelease(new Utils.Math.Vector(e.X, e.Y));
            MouseMove += (object sender, MouseEventArgs e) => currentScreen?.Component_OnMove(new Utils.Math.Vector(e.X, e.Y));
            KeyDown += (object sender, KeyEventArgs args) => currentScreen?.Component_OnKeyPress((char)args.KeyValue);
            KeyUp += (object sender, KeyEventArgs args) => currentScreen?.Component_OnKeyRelease((char)args.KeyValue);
            Resize += (object sender, EventArgs e) => currentScreen?.Component_OnResize(new Utils.Math.Vector(Width, Height));
        }

        public void OpenScreen(GuiScreen screen)
        {
            new Thread(() =>
            {
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
            StateManager.Update(e.Graphics);
            currentScreen?.OnRender();
            //TODO: StateManager Objekt orientiert??
            ShouldUpdate = false;
        }
    }
}
