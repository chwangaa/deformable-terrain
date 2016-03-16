using System;
using Improbable.Core.Network;
using Improbable.Fapi.Protocol;
using Improbable.Messages;
using Improbable.Unity.Entity;
using log4net;

namespace Improbable.Unity.MessageProcessors
{
    internal class AssetLoadRequestProcessor : MessageProcessorDispatcher<AssetLoadRequest>
    {
        private readonly IMessageSender MessageSender;
        private readonly IEntityTemplateProvider entityTemplateProvider;
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AssetLoadRequestProcessor));

        public AssetLoadRequestProcessor(IMessageSender messageSender, IEntityTemplateProvider entityTemplateProvider)
        {
            MessageSender = messageSender;
            this.entityTemplateProvider = entityTemplateProvider;
        }

        protected override void ProcessMsg(AssetLoadRequest assetLoadRequest)
        {
            var name = assetLoadRequest.Name;
            var context = assetLoadRequest.Context;
            var assetType = assetLoadRequest.AssetType;

            try
            {
                entityTemplateProvider.PrepareTemplate(new EntityAssetId(name, context),
                                                       (assetId) => AssetLoaded.Enqueue(MessageSender, assetType, name, context),
                                                       (err) => Logger.ErrorFormat("Failed to load asset '{0}' (type: '{1}', context: '{2}')\n {3}.", name, assetType, context, err));
            }
            catch (Exception e)
            {
                Logger.Error(String.Format("Error trying to prepare template for {0} context {1} : ", name, context), e);
            }
            assetLoadRequest.ReturnToPool();
        }
    }
}