using System;
using System.Threading.Tasks;
using FitBot.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace FitBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string name;
        private int age;
        private double weight;
        private double height;

        private double BMI;
        private double roundedBMI;
        private double heightInFeet;

        Random rnd = new Random();

        public async Task StartAsync(IDialogContext context)
        {
            //wait until the first message is receieved from the conversation and call MessageReceievedAsync to process that message
            context.Wait(this.MessageReceivedAsync);
            System.Threading.Thread.Sleep(2000);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //When MessageReceievedAsync is called, it's passed an IWaitable<IMessageActivity>. To get message, await the result. 
            var msg = await result;
            await this.SendWelcomeMessageAsync(context);
        }

        private async Task SendWelcomeMessageAsync(IDialogContext context)
        {
            string helloArray = FitBotResponses.greetOptsArray[rnd.Next(FitBotResponses.greetOptsArray.Length)];
            //after recieving and registering MessageReceivedAsync, the following is produced: 
            await context.SayAsync(helloArray);

            //after that message is displayed, context.Call will call the NameDialog
            context.Call(new NameDialog(), this.ResumeAfterNameDialog);
        }

        private async Task ResumeAfterNameDialog(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                //chaining constructor - this.name = await result is the name from Name Dialog we declared previously
                this.name = await result;
                context.Call(new AgeDialog(this.name), this.ResumeAfterAgeDialog);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding. Let's try again.");
                await this.SendWelcomeMessageAsync(context);
            }
        }

        private async Task ResumeAfterAgeDialog(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this.age = await result;
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("Im sorry. I'm having trouble understanding. Let's try that again.");
            }
            finally
            {
                context.Call(new HeightDialog(this.name), this.ResumeAfterHeightDialog);
            }
        }

        private async Task ResumeAfterHeightDialog(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this.height = await result;
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding.");
            }
            finally
            {
                context.Call(new WeightDialog(this.name), this.ResumeAfterWeightDialog);
            }
        }

        private async Task ResumeAfterWeightDialog(IDialogContext context, IAwaitable<int> result)
        {
            try
            {
                this.weight = await result;
                heightInFeet = (this.height) / 12;

                await context.PostAsync($"Name: { FitBotResponses.UppercaseFirst(this.name) } \nAge: {age} yrs\nHeight: {Math.Round(heightInFeet, 2)} ft \n Weight: {weight} lbs");
                System.Threading.Thread.Sleep(1000);
                await context.PostAsync(FitBotResponses.calcBMIOptsArray[rnd.Next(FitBotResponses.calcBMIOptsArray.Length)]);
                System.Threading.Thread.Sleep(2000);

                BMI = ((this.weight / this.height) / this.height) * 703;
                roundedBMI = (Math.Round(BMI, 2));


                await context.PostAsync($"Current BMI: {roundedBMI.ToString()}");
                System.Threading.Thread.Sleep(2000);


                //BMI Standards: Underweight -> Normalweight -> Overweight -> Heavyweight
                if (roundedBMI <= 18.5)
                {
                    await context.PostAsync($"With a BMI of {roundedBMI}, you are considered underweight.");
                }
                else if (roundedBMI >= 18.5 && roundedBMI <= 24.5)
                {
                    await context.PostAsync($"With a BMI of {roundedBMI}, you are considered normal weight.");
                }
                else if (roundedBMI >= 25 && roundedBMI <= 29.9)
                {
                    await context.PostAsync($"With a BMI of {roundedBMI}, you are considered overweight.");
                }
                else
                {
                    await context.PostAsync("With a BMI over 30, you are considered heavyweight, open a can of whoop ass.");
                }

                System.Threading.Thread.Sleep(5000);
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm sorry, I'm having issues understanding. Let's try that again.");
            }
            finally
            {
                await this.StartAsync(context);

            }
        }
    }
}