using BookingApp.Model;
using BookingApp.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace BookingApp.Repository
{
    public class LanguageRepository
    {
        private const string FilePath = "../../../Resources/Data/languages.csv";

        private readonly Serializer<Language> _serializer;

        private List<Language> _languages;

        public LanguageRepository()
        {
            _serializer = new Serializer<Language>();
            _languages = _serializer.FromCSV(FilePath);
        }

        public List<Language> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public Language Save(Language language)
        {
            language.Id = NextId();
            _languages = _serializer.FromCSV(FilePath);
            _languages.Add(language);
            _serializer.ToCSV(FilePath, _languages);
            return language;
        }

        public int NextId()
        {
            _languages = _serializer.FromCSV(FilePath);
            if (_languages.Count < 1)
            {
                return 1;
            }
            return _languages.Max(c => c.Id) + 1;
        }

        public void Delete(Language language)
        {
            _languages = _serializer.FromCSV(FilePath);
            Language found = _languages.Find(c => c.Id == language.Id);
            _languages.Remove(found);
            _serializer.ToCSV(FilePath, _languages);
        }

        public Language Update(Language language)
        {
            _languages = _serializer.FromCSV(FilePath);
            Language current = _languages.Find(c => c.Id == language.Id);
            int index = _languages.IndexOf(current);
            _languages.Remove(current);
            _languages.Insert(index, language);       // keep ascending order of ids in file 
            _serializer.ToCSV(FilePath, _languages);
            return language;
        }

        public Language? GetLanguageById(int languageId)
        {
            return _languages.Find(c => c.Id == languageId);
        }

        public List<Language> GetAllLanguages()
        {
            _languages = _serializer.FromCSV(FilePath);
            return _languages;
        }
    }
}
