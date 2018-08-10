using System;
using System.Threading.Tasks;
using FitBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web;

namespace FitBot.Dialogs
{
    [Serializable]
    public class WeightDialog : IDialog<int>
    {
        private string name;
        private int attempts = 3;

        public WeightDialog(string name)
        {
            this.name = name;
        }

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Round your weight to the nearest pound... I really don't feel like dealing with floats and decimals and all that other bullshit.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            int weight;

            if (Int32.TryParse(message.Text, out weight) && (weight > 0))
            {
                context.Done(weight);
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("I'm sorry, I don't understand your reply. What is your weight (e.g. '115', '190')?");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    context.Fail(new TooManyAttemptsException("Message was not a valid weight."));
                }
            }
        }
    }
}