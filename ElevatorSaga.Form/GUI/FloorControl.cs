﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ElevatorSaga.Core.Classes;

namespace ElevatorSaga.GUI
{
    public partial class FloorControl : UserControl
    {

        private readonly Floor Floor;

        public FloorControl(Floor f)
        {
            InitializeComponent();

            Floor = f;

            if (f.IsBottomFloor)
                chkDown.Visible = chkDown.Enabled = false;

            if (f.IsTopFloor)
                chkUp.Visible = chkUp.Enabled = false;

            Floor.OnButtonStateChanged += Ev_OnButtonStateChanged;

            lblLevel.Text = string.Format("{0} ({1})", f.Level, f.AwaitingUsers.Length);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawLine(Pens.Black, new Point(0, 0), new Point(Width, 0));
        }

        public void Update()
        {
            lblLevel.Text = string.Format("{0} ({1})", Floor.Level, Floor.AwaitingUsers.Length);
        }

        bool fromEvent = false;
        private void Ev_OnButtonStateChanged(object sender, FloorButtonStateChangedEventArgs eargs)
        {
            string checkBoxName = eargs.ChangedDirection == Direction.Up ? "chkUp" : "chkDown";
            CheckBox box = buttonContainer.Controls.OfType<CheckBox>().FirstOrDefault(x => x.Name == checkBoxName);
            bool isPressed = box.Checked;

            if (isPressed != eargs.ChangedValue)
            {
                fromEvent = true;
                box.Invoke(new Action(() =>
                {
                    box.Checked = eargs.ChangedValue;
                    box.Enabled = !box.Checked;
                    fromEvent = false;
                }));
            }
        }

        private void OnButtonStateChanged(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (fromEvent) return;

            if (cb.Name == "chkUp" && cb.Checked)
            {
                cb.Enabled = false;
                Floor.PressUpButton();
            }
            if (cb.Name == "chkDown" && cb.Checked)
            {
                cb.Enabled = false;
                Floor.PressDownButton();
            }
        }
    }
}
