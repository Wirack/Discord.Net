﻿namespace Discord.WebSocket
{
    internal static class EntityExtensions
    {
        public static IActivity ToEntity(this API.Game model)
        {
            // Rich Game
            if (model.ApplicationId.IsSpecified)
            {
                ulong appId = model.ApplicationId.Value;
                var assets = model.Assets.GetValueOrDefault()?.ToEntity(appId);
                return new RichGame
                {
                    ApplicationId = appId,
                    Name = model.Name,
                    Details = model.Details.GetValueOrDefault(),
                    State = model.State.GetValueOrDefault(),
                    SmallAsset = assets?[0],
                    LargeAsset = assets?[1],
                    Party = model.Party.GetValueOrDefault()?.ToEntity(),
                    Secrets = model.Secrets.GetValueOrDefault()?.ToEntity(),
                    Timestamps = model.Timestamps.GetValueOrDefault()?.ToEntity()
                };
            }
            // Stream Game
            if (model.StreamUrl.IsSpecified)
            {
                return new StreamingGame(
                    model.Name, 
                    model.StreamUrl.Value, 
                    model.StreamType.Value.GetValueOrDefault());
            }
            // Normal Game
            return new Game(model.Name);
        }

        // (Small, Large)
        public static GameAsset[] ToEntity(this API.GameAssets model, ulong appId)
        {
            return new GameAsset[]
            {
                model.SmallImage.IsSpecified ? new GameAsset
                {
                    ApplicationId = appId,
                    ImageId = model.SmallImage.GetValueOrDefault(),
                    Text = model.SmallText.GetValueOrDefault()
                } : null,
                model.LargeImage.IsSpecified ? new GameAsset
                {
                    ApplicationId = appId,
                    ImageId = model.LargeImage.GetValueOrDefault(),
                    Text = model.LargeText.GetValueOrDefault()
                } : null,
            };
        }

        public static GameParty ToEntity(this API.GameParty model)
        {
            return new GameParty
            {
                Id = model.Id,
                Size = model.Size
            };
        }

        public static GameSecrets ToEntity(this API.GameSecrets model)
        {
            return new GameSecrets(model.Match, model.Join, model.Spectate);
        }

        public static GameTimestamps ToEntity(this API.GameTimestamps model)
        {
            return new GameTimestamps(model.Start.ToNullable(), model.End.ToNullable());
        }
    }
}
