using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Models.Message;

namespace UI.Models.Impl {
    public class ModelFactory : IModelFactory
    {

        private readonly ModelCreator<AbstractConversation> _conversationCreator;
        private readonly ModelCreator<GroupConversation> _groupCreator;

        public ModelFactory(ModelCreator<AbstractConversation> conversationCreator, ModelCreator<GroupConversation> groupCreator)
        {
            _conversationCreator = conversationCreator;
            _groupCreator = groupCreator;
        }

        public TModel Create<TModel>()
        {
            Type type = typeof(TModel);
            if (type == typeof(GroupConversation))
                return (TModel) Convert.ChangeType(_groupCreator.Invoke(), type);
            return (TModel) Convert.ChangeType(_conversationCreator.Invoke(), type);
        }
    }
}
