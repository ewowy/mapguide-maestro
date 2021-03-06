﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//

#endregion Disclaimer / License

using ICSharpCode.Core;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using System;
using System.IO;
using System.Windows.Forms;

namespace Maestro.Base.UI
{
    internal partial class MessageViewer : SingletonViewContent
    {
        /// <summary>
        /// Internal use only. Do not invoke directly. Use <see cref="T:Maestro.Base.Services.ViewContentManager"/> for that
        /// </summary>
        public MessageViewer()
        {
            InitializeComponent();
            this.Title = this.Description = Strings.Content_Messages;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var btw = BroadcastTextWriter.Instance;
            btw.LogMessage += WeakEventHandler.Wrap<LogBroadcastEventHandler>(OnLogMessage, (eh) => btw.LogMessage -= eh);
            btw.FlushMessages();
        }

        private void OnLogMessage(object sender, LogMessageEventArgs msg)
        {
            if (!txtMessages.IsDisposed)
            {
                txtMessages.AppendText($"[{msg.LogDate.ToString("dd MMM yyyy hh:mm:ss")}]: {msg.Message}"); //NOXLATE
                txtMessages.ScrollToCaret();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMessages.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var save = DialogFactory.SaveFile())
            {
                save.Filter = string.Format(OSGeo.MapGuide.MaestroAPI.Strings.GenericFilter, OSGeo.MapGuide.MaestroAPI.Strings.PickLog, "log"); //NOXLATE
                if (save.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(save.FileName, txtMessages.Text);
                    MessageService.ShowMessage(string.Format(Strings.Log_Saved, save.FileName));
                }
            }
        }

        public override ViewRegion DefaultRegion
        {
            get
            {
                return ViewRegion.Bottom;
            }
        }
    }
}