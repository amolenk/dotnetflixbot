using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Application1
{
    [Serializable]
    public class FindEpisodeDialog : IDialog
    {
        private List<Episode> _episodes;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<Message> argument)
        {
            var message = await argument;
            if (message.Text.StartsWith("find"))
            {
                var subject = message.Text.Substring("find".Length).Trim();

                _episodes = await DotnetflixDB.FindEpisodesAsync(subject);
                if (_episodes.Any())
                {
                    var options = _episodes.Select((e, i) => i + 1).ToArray();

                    var markdownText = string.Format("I've found {0} episodes, which one are you looking for:\n{1}",
                        _episodes.Count,
                        string.Join("\n", _episodes.Select((e, i) => string.Format("{0}. {1}", i + 1, e.Title))));

                    PromptDialog.Choice<int>(
                        context,
                        AfterSelectAsync,
                        options,
                        markdownText);
                }
                else
                {
                    await context.PostAsync("Found nothing on " + subject);
                    context.Wait(MessageReceivedAsync);
                }
            }
            else
            {
                await context.PostAsync("I didn't get that.");
                context.Wait(MessageReceivedAsync);
            }
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<int> argument)
        {
            var episodeIndex = await argument;

            var episode = _episodes[episodeIndex - 1];

            await context.PostAsync("Ok, here's the link: " + episode.Link);

            context.Wait(MessageReceivedAsync);
        }
    }
}