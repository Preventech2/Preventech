using System;

namespace Preventech.Core.Services
{
    public class HeaderService
    {
        private string _title = "Preventech";

        public string Title
        {
            get => _title;
            private set
            {
                if (_title != value)
                {
                    _title = value;
                    TitleChanged?.Invoke();
                }
            }
        }

        public event Action? TitleChanged;

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void ResetTitle()
        {
            Title = "Preventech";
        }
    }
}