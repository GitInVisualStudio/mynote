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
            IOManager manager = new IOManager(new SaveLoader());

            //Semester s = new Semester("Q2", new DateTime(2020, 4, 8));
            //Course k = new Course("Deutsch Reinke", new DateTime(2020, 4, 8), Color.Red, new MyNoteBase.Utils.Graphic.Icon("Heft", new Bitmap(32, 32)), s);
            //Note n = new Note(new DateTime(2020, 4, 8, 11, 40, 0), "als wir noch unterricht hatten lol", k, new TestIManager());
            //Test t = new Test(new DateTime(2020, 4, 21), k, "Zuhausearbeit", TestType.Exam);
            //manager.SaveCanvas(n);

            Canvas o = manager.LoadCanvas("userSaves\\Q2\\Deutsch Reinke\\als wir noch unterricht hatten lol.myn", new TestIManager());
            
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
