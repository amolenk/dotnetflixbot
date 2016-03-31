using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Bot_Application1.Controllers
{
    //[BotAuthentication]
    public class DotnetflixController : ApiController
    {
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                //if (message.Text.StartsWith("find"))
                //{
                //    var subject = message.Text.Substring("find".Length).Trim();

                //    var episode = (await DotnetflixDB.FindEpisodesAsync(subject)).FirstOrDefault();
                //    if (episode != null)
                //    {
                //        return message.CreateReplyMessage("Found episode: " + episode.Title);
                //    }
                //    else
                //    {
                //        return message.CreateReplyMessage("Sorry, can't find an episode.");
                //    }
                //}

                //return null;

                #region Dialog
                return await Conversation.SendAsync(message, () => new FindEpisodeDialog());
                #endregion
            }
            else
            {
                return HandleSystemMessage(message);
            }
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }

            return null;
        }
    }
}