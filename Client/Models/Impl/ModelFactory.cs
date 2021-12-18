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

        public ModelFactory(ModelCreator<AbstractConversation> conversationCreator)
        {
            _conversationCreator = conversationCreator;
        }

        public TModel Create<TModel>()
        {
            return (TModel) Convert.ChangeType(_conversationCreator.Invoke(), typeof(TModel));
        }
    }
}
