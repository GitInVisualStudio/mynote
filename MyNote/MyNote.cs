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
            //Semester s = new Semester("200325_test_semester");
            //Course k = new Course("200325_test_course", Color.White, new MyNoteBase.Utils.Graphic.Icon("icon", new Bitmap(10, 10)), s);
            //Canvas c = new Excercise(DateTime.Now, "200325_test", k, new TestIManager());
            //new IOManager(new SaveLoader()).SaveCanvas(c);
            Canvas c = new IOManager(new SaveLoader()).LoadCanvas("D:\\KurzerAufenthalt\\Mynote\\saves\\200325_test_semester\\200325_test_course\\200325_test.myE", new TestIManager());
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

            //TODO: StateManager Objekt orientiert??
            shouldUpdate = false;
        }
    }
}
