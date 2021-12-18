using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ViewModels
{
    class DownloadViewmodel:ViewModelBase
    {
        private string _fileName;
        private double _downloadProgress;
        public string FileName
        {
            get => _fileName;
            set
            {
                OnPropertyChanged(nameof(FileName));
                _fileName = value;
            }
        }

        public Double DownloadProgress
        {
            get => _downloadProgress;
            set
            {
                OnPropertyChanged(nameof(DownloadProgress));
                _downloadProgress = value;
            }
        }
        public DownloadViewmodel(string filename, double progress=0)
        {
            this.FileName = filename;
            this.DownloadProgress = progress;
        }

    }
}
