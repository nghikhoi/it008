using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models {

    public delegate TModel ModelCreator<TModel>();
    public interface IModelFactory
    {

        TModel Create<TModel>();

    }
}
