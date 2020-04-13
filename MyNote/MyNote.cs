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

            this.s = new Semester("Q2", DateTime.Now);
            this.k = new Course("new name", DateTime.Now, Color.Red, new MyNoteBase.Utils.Graphic.Icon("Heft", new Bitmap(32, 32), 1), s)
            {
                OnlineID = 29
            };
            this.c = new Note(DateTime.Now, "als wir noch unterricht hatten lol", k, new TestIManager());
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

        private void btnTest_Click(object sender, EventArgs e) // DEBUG
        {
            TestAPI().Wait();
        }

        private APIManager manager = new APIManager("test", "");
        private Semester s;
        private Course k;
        private Canvas c;
        private Test t;
        private async Task TestAPI()
        {
            //await manager.UploadTest(t).ConfigureAwait(false);
            await manager.Test(k).ConfigureAwait(false);
        }
    }
}
