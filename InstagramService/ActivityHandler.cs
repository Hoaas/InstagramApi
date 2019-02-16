using System.Linq;
using Common.Models;
using InstagramApiSharp.Classes.Models;

namespace InstagramService
{
    internal class ActivityHandler
    {
        internal static void AddMediaToActivityBasedOnType(InstaDirectInboxItem item, InstaActivityDto activity)
        {
            switch (item.ItemType)
            {
                case InstaDirectThreadItemType.Text:
                case InstaDirectThreadItemType.Like:
                    activity.Text = item.Text;
                    break;

                case InstaDirectThreadItemType.MediaShare:
                    activity.Urls.AddRange(item.MediaShare.Images.Select(i => i.Uri));
                    activity.Text = $"{item.MediaShare.Caption.Text} - {item.MediaShare.Title}";
                    break;

                case InstaDirectThreadItemType.Link:
                    activity.Text = $"{item.LinkMedia.Text} - {item.LinkMedia.LinkContext.LinkTitle}";
                    activity.Text = item.LinkMedia.Text;
                    activity.Urls.Add(item.LinkMedia.LinkContext.LinkImageUrl);
                    activity.Urls.Add(item.LinkMedia.LinkContext.LinkUrl);
                    break;

                case InstaDirectThreadItemType.Media:
                    activity.Urls.AddRange(item.Media.Images.Select(i => i.Uri));
                    activity.Urls.AddRange(item.Media.Videos.Select(i => i.Uri));
                    break;

                case InstaDirectThreadItemType.StoryShare:
                    activity.Text = $"{item.StoryShare.Title} - {item.StoryShare.Text} - {item.StoryShare.Message} - {item.StoryShare.Media.Title} - {item.StoryShare.Media.Caption.Text}";
                    activity.Urls.AddRange(item.StoryShare.Media.Images.Select(i => i.Uri));
                    break;

                case InstaDirectThreadItemType.RavenMedia:
                    // ravenmedia if api version < 61
                    activity.Urls.AddRange(item.VisualMedia.Media.Images.Select(i => i.Uri));
                    activity.Urls.AddRange(item.VisualMedia.Media.Videos.Select(i => i.Uri));
                    break;

                case InstaDirectThreadItemType.ActionLog:
                    activity.Text = item.ActionLog.Description;
                    break;

                case InstaDirectThreadItemType.Profile:
                    activity.Urls.Add(item.ProfileMedia.ProfilePicUrl);
                    break;

                case InstaDirectThreadItemType.Placeholder:
                    activity.Text = item.Placeholder.Message;
                    break;

                case InstaDirectThreadItemType.Location:
                    var lm = item.LocationMedia;
                    activity.Text = $"{lm.ShortName}, {lm.City}, ({lm.X},{lm.Y})";
                    break;

                case InstaDirectThreadItemType.FelixShare:
                    activity.Urls.AddRange(item.FelixShareMedia.Images.Select(i => i.Uri));
                    activity.Urls.AddRange(item.FelixShareMedia.Videos.Select(i => i.Uri));
                    activity.Text = $"{item.FelixShareMedia.Title} - {item.FelixShareMedia.Caption.Text}";
                    break;

                case InstaDirectThreadItemType.ReelShare:
                    activity.Text = item.ReelShareMedia.Text;
                    activity.Urls.AddRange(item.ReelShareMedia.Media.ImageList.Select(i => i.Uri));
                    activity.Urls.AddRange(item.ReelShareMedia.Media.VideoList.Select(i => i.Uri));
                    break;

                case InstaDirectThreadItemType.VoiceMedia:
                    activity.Text = "VoiceMedia received, but not supported. Sorry.";
                    break;

                case InstaDirectThreadItemType.AnimatedMedia:
                    activity.Urls.Add(item.AnimatedMedia.Media.Url);
                    break;

                case InstaDirectThreadItemType.Hashtag:
                    //item.HashtagMedia.Media.
                    activity.Text = $"{item.HashtagMedia.Name} - {item.HashtagMedia.Media.Title}";
                    activity.Urls.AddRange(item.HashtagMedia.Media.Images.Select(i => i.Uri));
                    activity.Urls.AddRange(item.HashtagMedia.Media.Videos.Select(i => i.Uri));
                    break;

                case InstaDirectThreadItemType.LiveViewerInvite:
                    activity.Text = item.LiveViewerInvite.Text;
                    activity.Urls.Add(item.LiveViewerInvite.Broadcast.CoverFrameUrl);
                    activity.Urls.Add(item.LiveViewerInvite.Broadcast.DashAbrPlaybackUrl);
                    activity.Urls.Add(item.LiveViewerInvite.Broadcast.DashPlaybackUrl);
                    activity.Urls.Add(item.LiveViewerInvite.Broadcast.RtmpPlaybackUrl);
                    break;
            }
        }
    }
}