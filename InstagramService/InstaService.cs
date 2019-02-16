using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Models;
using InstaSharper.Classes.Models;

namespace InstagramService
{
    public class InstaService
    {
        private readonly InstaApiFactory _instaApiFactory;
        private static readonly Dictionary<string, DateTime> ThreadStatus = new Dictionary<string, DateTime>();

        public InstaService(InstaApiFactory instaApiFactory)
        {
            _instaApiFactory = instaApiFactory;
        }

        public async Task<List<InstaActivityDto>> GetAllNewActivity()
        {
            var api = await _instaApiFactory.Create();

            var inbox = await api.GetDirectInboxAsync();
            if (!inbox.Succeeded) throw new InstaException("Fail getting inbox");

            var newActivity = new List<InstaActivityDto>();

            foreach (var thread in inbox.Value.Inbox.Threads)
            {
                if (!DoesThreadContainNewData(thread)) continue;

                var items = await GetNewItemsInThread(thread.ThreadId);

                newActivity.AddRange(
                    from item in items
                    let user = thread.Users.SingleOrDefault(u => u.Pk == item.UserId)
                    select CreateActivityDto(item, user)
                );
                ThreadStatus[thread.ThreadId] = thread.LastActivity;
            }

            return newActivity;
        }

        private static InstaActivityDto CreateActivityDto(InstaDirectInboxItem item, InstaUserShort user)
        {
            var activity = new InstaActivityDto
            {
                User = new InstaUserDto
                {
                    Fullname = user.FullName,
                    Username = user.UserName,
                    ProfilePicture = user.ProfilePicture
                },
                Text = item.Text,
                Timestamp = item.TimeStamp
            };

            if (item.Media?.Images != null)
            {
                activity.Urls.AddRange(item.Media.Images.Select(i => i.URI));
            }

            return activity;
        }

        private async Task<List<InstaDirectInboxItem>> GetNewItemsInThread(string threadId)
        {
            var api = await _instaApiFactory.Create();

            var thread = await api.GetDirectInboxThreadAsync(threadId);

            if (!thread.Succeeded) throw new InstaException("Could not get thread.");

            return thread.Value.Items.Where(t => t.TimeStamp > ThreadStatus[threadId]).ToList();
        }

        private static bool DoesThreadContainNewData(InstaDirectInboxThread thread)
        {
            if (!ThreadStatus.TryGetValue(thread.ThreadId, out var lastAnnounceActivity))
            {
                ThreadStatus.Add(thread.ThreadId, DateTime.Now);
            }

            return lastAnnounceActivity != thread.LastActivity;
        }
    }
}
