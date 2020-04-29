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
using MyNoteBase.Utils.API;
using MyNoteBase.Canvasses.Content;

namespace MyNote
{
    public partial class MyNote : Form
    {
        private const int ANIMATION_UPDATES = 60;
        private GuiScreen currentScreen;
        private System.Windows.Forms.Timer timer;
        private bool shouldUpdate;
        private Thread animationThread;

        public bool ShouldUpdate
        {
            get
            {
                return shouldUpdate;
            }

            set
            {
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
            Semester s = new Semester("Q2", new DateTime(2020, 04, 27));
            Course k = new Course("Deutsch Bauschke", new DateTime(2020, 04, 27), Color.Red, new MyNoteBase.Utils.Graphic.Icon("heft", new Bitmap(32, 32), 1), s);
            Excercise c = new Excercise(new DateTime(2020, 04, 27), "Vormärz", k, new TestIManager(), "AB Vormärz");
            c.Content.Add(new TextContent());

            new IOManager(new SaveLoader()).SaveCanvas(c);

            c = (Excercise)new IOManager(new SaveLoader()).LoadCanvas("G:\\mynote\\MyNote\\bin\\Debug\\userSaves\\Q2\\Deutsch Bauschke\\Vormärz.myn", new TestIManager());
        }

        public void Init()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10000;
            timer.Start();

            animationThread = new Thread(() =>
            {
                while (true)
                {
                    AnimationManager.Update();
                    Thread.Sleep((int)(1000.0f / ANIMATION_UPDATES));
                    if (AnimationManager.animations.Count != 0)
                        shouldUpdate = true;
                }
            });
            animationThread.Start();
        }

        public void OpenScreen(GuiScreen screen)
        {
            new Thread(() =>
            {
                screen.Init();
                currentScreen.Close();
                while (currentScreen.Opend) ;
                currentScreen = screen;
            }).Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //TODO: StateManager Objekt orientiert // edit von miriam: ja bitte, implementieren von MyNoteBase.Utils.Graphic.IManager danke lg
            shouldUpdate = false;
        }
    }
}
