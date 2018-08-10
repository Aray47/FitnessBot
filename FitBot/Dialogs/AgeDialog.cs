using FitBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace FitBot.Dialogs
{
    [Serializable]
    public class AgeDialog : IDialog<int>
    {
        private string name;
        private int attempts = 3;
        Random rnd = new Random();

        public AgeDialog(string name)
        {
            this.name = name;
        }

        public async Task StartAsync(IDialogContext context)
        {

            System.Threading.Thread.Sleep(2000);
            await context.PostAsync("What is your age?");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            string ageConfusionArray = FitBotResponses.dontUnderstandAge[rnd.Next(FitBotResponses.dontUnderstandAge.Length)];
            string tooOldArray = FitBotResponses.tooOldOptsArray[rnd.Next(FitBotResponses.tooOldOptsArray.Length)];

            var message = await result;
            int age;

            if (Int32.TryParse(message.Text, out age) && (age > 0 && age < 99))
            {
                context.Done(age);
            }
            else if (Int32.TryParse(message.Text, out age) && (age >= 100))
            {
                await context.PostAsync(tooOldArray);
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("Let's try that again.");
                    context.Wait(this.MessageReceivedAsync);
                }
            }
            //else
            //{
            //    --attempts;
            //    if (attempts > 0)
            //    {
            //        await context.PostAsync(ageConfusionArray);
            //        context.Wait(this.MessageReceivedAsync);
            //    }
            //    else
            //    {
            //        context.Fail(new TooManyAttemptsException("Message was not a valid age."));
            //    }
            //}
        }
    }
}
