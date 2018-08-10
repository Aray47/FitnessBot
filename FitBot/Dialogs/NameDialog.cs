using FitBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace FitBot.Dialogs
{
    [Serializable]
    public class NameDialog : IDialog<string>
    {
        Random rnd = new Random();

        //first method after name dialog is called upon
        public async Task StartAsync(IDialogContext context)
        {

            System.Threading.Thread.Sleep(2000);
            string nameArray = FitBotResponses.nameOptsArray[rnd.Next(FitBotResponses.nameOptsArray.Length)];
            await context.PostAsync(nameArray);
            context.Wait(this.MessageReceivedAsync);        //waiting for user response, once that happens, MessageReceievedAsync begins
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string nameResponseArray = FitBotResponses.nameResponseOptsArray[rnd.Next(FitBotResponses.nameResponseOptsArray.Length)];

            var message = await result;
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                System.Threading.Thread.Sleep(2000);
                await context.PostAsync($"{ FitBotResponses.UppercaseFirst(message.Text) }, {nameResponseArray}");
                context.Done(message.Text);
            }
        }
    }
}