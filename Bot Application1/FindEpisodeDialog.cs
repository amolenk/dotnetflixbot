using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Application1
{
    [LuisModel("2e70a61d-545d-49b5-a95a-0c8044deec65", "6484e25743944bbb88d7b52949c36e2b")]
    [Serializable]
    public class FindEpisodeDialog : LuisDialog
    {
        private List<Episode> _episodes;

        [LuisIntent("Find an episode")]
        public async Task FindAnEpisode(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity;
            if (result.TryFindEntity("subject", out entity))
            {
                var subject = entity.Entity;

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
                    context.Wait(MessageReceived);
                }
            }
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I didn't get that.");
            context.Wait(MessageReceived);
        }

        public async Task AfterSelectAsync(IDialogContext context, IAwaitable<int> argument)
        {
            var episodeIndex = await argument;

            var episode = _episodes[episodeIndex - 1];

            await context.PostAsync("Ok, here's the link: " + episode.Link);

            context.Wait(MessageReceived);
        }
    }
}