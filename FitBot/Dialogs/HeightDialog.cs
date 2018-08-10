using System;
using System.Threading.Tasks;
using FitBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace FitBot.Dialogs
{
    [Serializable]
    public class HeightDialog : IDialog<int>
    {
        private string name;
        private int attempts = 3;

        public HeightDialog(string name)
        {
            this.name = name;
        }

        public async Task StartAsync(IDialogContext context)
        {

            System.Threading.Thread.Sleep(2000);
            await context.PostAsync($"{FitBotResponses.UppercaseFirst(this.name)}, what is your height in inches?");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            int height;

            if (Int32.TryParse(message.Text, out height) && (height > 0))
            {
                context.Done(height);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry. I don't understand. Give me your height in inches.");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a valid height"));
                }
            }
        }
    }
}