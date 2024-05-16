using clear.Forms;
using Newtonsoft.Json.Linq;
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

namespace clear.UCs
{
    public partial class Home : UserControl
    {
        DiscordAPI discordAPI = new DiscordAPI();
        CancellationTokenSource cts = new CancellationTokenSource();
        public Home()
        {
            InitializeComponent();
            guna2Button2.Click += guna2Button2_Click;
        }
        public Guna.UI2.WinForms.Guna2GradientPanel MyGradientPanel => guna2GradientPanel2;

        public void showToast(string type, string message)
        {
            Not toast = new Not(type, message);
            toast.Show();
        }

        private async Task handle_guna2Button1_Click()
        {
            try
            {
                cts = new CancellationTokenSource();
                guna2ProgressBar1.Value = 0;

                bool nukeDMS = guna2ToggleSwitch2.Checked;
                bool nukeGUILDS = guna2ToggleSwitch1.Checked;

                string warningMessage = $"Essa operação deletará todas as mensagens em " +
                    $"{(nukeDMS ? "DMs" : "")}{(nukeGUILDS && nukeDMS ? " e " : "")}{(nukeGUILDS ? "servidores" : "")}. Você tem certeza que deseja continuar?";
                if ((nukeDMS || nukeGUILDS) && MessageBox.Show(warningMessage, "Aviso!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                {
                    return;
                }

                await UpdateAuthIDFromUI();

                var finalIDList = await BuildIDListFromUI(nukeDMS, nukeGUILDS);

                await discordAPI.DeleteMessagesFromMultipleChannels(finalIDList.ToArray(),
                    (int allChannelCount, int searchedChannelCount, int foundMessageCount, int deletedMessageCount) => this.InvokeIfRequired(() =>
                    {
                        if (deletedMessageCount > 0)
                        {
                            guna2ProgressBar1.Value = (int)((float)deletedMessageCount / foundMessageCount * guna2ProgressBar1.Maximum);
                        }
                    }),
                    (int rlSeconds) => { },
                    cts.Token);

                showToast("SUCCESS", "Finalizado!");
            }
            catch (Exception exc)
            {
                if (exc is TaskCanceledException || exc is OperationCanceledException)
                {
                    showToast("ERROR", "Operação cancelada!");
                }
                else
                {
                    showToast("ERROR", "Insira um token válido!");
                }
            }
        }

        private async Task<IEnumerable<DiscordAPI.ChannelAndGuild>> BuildIDListFromUI(bool EraseAllDMs, bool EraseAllGuilds)
        {
            var idTextboxInput = guna2TextBox1.Text.Replace(" ", "").Split(',');
            var userIDList = idTextboxInput.Where(x => x.StartsWith("U")).Select(x => x.Substring(1)).Distinct();
            var guildIDList = idTextboxInput.Where(x => x.StartsWith("G")).Select(x => x.Substring(1)).Distinct();
            var channelIDList = idTextboxInput.Where(x => x.All(char.IsDigit)).Distinct();

            var allDMsList = await discordAPI.GetUserDMList(null, cts.Token);
            var allGuildsList = await discordAPI.GetUserGuilds(null, cts.Token);
            var finalIDList = new List<DiscordAPI.ChannelAndGuild>();

            if (EraseAllGuilds)
                finalIDList.AddRange(allGuildsList.Select(x => new DiscordAPI.ChannelAndGuild(x.Id, true)));
            else
                finalIDList.AddRange(guildIDList.Select(x => new DiscordAPI.ChannelAndGuild(x, true)));

            if (EraseAllDMs)
                finalIDList.AddRange(allDMsList.Select(x => new DiscordAPI.ChannelAndGuild(x.Id, false)));
            else
                finalIDList.AddRange(allDMsList.Where((QuickType.DmChatGroup dm) =>
                {
                    return dm.Recipients != null && dm.Recipients.Count() == 1 && userIDList.Contains(dm.Recipients.First().Id);
                }).Select(x => new DiscordAPI.ChannelAndGuild(x.Id, false)));

            finalIDList.AddRange(channelIDList.Select(x => new DiscordAPI.ChannelAndGuild(x, false)));

            return finalIDList.Distinct();
        }

        private async Task UpdateAuthIDFromUI()
        {
            string authID = guna2TextBox2.Text.Replace(" ", "").Replace("\"", "");
            if (string.IsNullOrWhiteSpace(authID))
            {
                showToast("ERROR", "Insira um token válido!");
            }
            await discordAPI.SetAuthID(authID);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            FreezeUI();
            await handle_guna2Button1_Click();
            UnfreezeUI();
        }

        private void FreezeUI()
        {
            guna2TextBox2.Enabled = false;
            guna2Button1.Enabled = false;
            guna2ToggleSwitch2.Enabled = false;
            guna2ToggleSwitch1.Enabled = false;
            guna2TextBox1.Enabled = false;
            UseWaitCursor = true;
            Application.DoEvents();
        }

        private async void UnfreezeUI()
        {
            guna2TextBox2.Enabled = true;
            guna2ToggleSwitch2.Enabled = true;
            guna2ToggleSwitch1.Enabled = true;
            guna2Button1.Enabled = true;
            guna2TextBox1.Enabled = true;
            UseWaitCursor = false;
            await Task.Yield();
            Application.DoEvents();
        }
    }
}