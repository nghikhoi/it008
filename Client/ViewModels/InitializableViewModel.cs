using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Command;

namespace UI.ViewModels {
    public abstract class InitializableViewModel : ViewModelBase {

        private ICommand _initializeCommand;
        public ICommand InitializeCommand {
            get => _initializeCommand;
            private set {
                _initializeCommand = value;
                OnPropertyChanged(nameof(InitializeCommand));
            }
        }

        protected InitializableViewModel() {
            InitializeCommand = new InitializeCommand(Initialize);
        }

        protected abstract void Initialize(object parameter = null);

    }
}
