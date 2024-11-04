using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BookingApp.Cache
{
    public class Cache : INotifyPropertyChanged
    {
        private Dictionary<string, object> cache = new Dictionary<string, object>();

        public object this[string key]
        {
            get => cache.ContainsKey(key) ? cache[key] : null;
            set
            {
                if (!cache.ContainsKey(key) || cache[key] != value)
                {
                    cache[key] = value;
                    OnPropertyChanged(key);
                }
            }
        }

        public void Add(string key, object value)
        {
            cache[key] = value;
        }

        public bool ContainsKey(string key)
        {
            return cache.ContainsKey(key);
        }

        public object Get(string key)
        {
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
