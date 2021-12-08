using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.ViewModels;

namespace UI.Command {
    public class InitializeCommand : BaseCommand {

        private readonly Action<object> _executor;
        private bool executed = false;

        public InitializeCommand(Action<object> executor) {
            this._executor = executor;
        }

        public override void Execute(object parameter) {
            Console.WriteLine("Executed InitializeCommand");
            lock (_executor) {
                if (!executed) {
                    _executor.Invoke(parameter);
                    executed = true;
                }
            }
        }

        public bool IsExecuted() {
            return executed;
        }

    }

    public static class StaticInitializeCommand {
        
        public static RoutedCommand InitializeCommand { get; set; }

        static StaticInitializeCommand() {
            InitializeCommand = new RoutedCommand();
            CommandManager.RegisterClassCommandBinding(typeof(Window)
                , new CommandBinding(InitializeCommand, InitializeCommandHandle, InitializeCommandCanExecute));
        }

        private static void InitializeCommandCanExecute(object sender, CanExecuteRoutedEventArgs args) {
            object parameter = args.Parameter;
            if (parameter == null || !(parameter is InitializableViewModel)) {
                args.CanExecute = false;
                return;
            }
            args.CanExecute = true;
        }

        private static void InitializeCommandHandle(object sender, ExecutedRoutedEventArgs args) {
            object parameter = args.Parameter;
            ((InitializableViewModel) parameter).InitializeCommand.Execute(null);
        }
        
    }
    
}
