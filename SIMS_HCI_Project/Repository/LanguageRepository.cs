using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace BookingApp.Repository
{
    public class LanguageRepository : ILanguageRepository
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

        public void Create(Language language)
        {
            language.Id = NextId();
            _languages.Add(language);
            _serializer.ToCSV(FilePath, _languages);
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

        public void Update(Language language)
        {
            int index = _languages.FindIndex(gd => language.Id == gd.Id);
            if (index != -1)
            {
                _languages[index] = language;
                _serializer.ToCSV(FilePath, _languages);
            }
        }

        public Language? GetLanguageById(int languageId)
        {
            return _languages.Find(c => c.Id == languageId);
        }

        public Language? GetById(int languageId)
        {
            return _languages.Find(c => c.Id == languageId);
        }

        public List<Language> GetAllLanguages()
        {
            _languages = _serializer.FromCSV(FilePath);
            return _languages;
        }
    
            
        public Language GetLanguageByName(string name)
        {
            foreach(Language language in _languages)
            {
                if (language.Name == name) { 
                    return language;
                }
            }
            return null;

        }
        public int ExistsLanguage(string name)
        {
            var existingLanguage = _languages.FirstOrDefault(language => language.Name == name);
            return existingLanguage != null ? existingLanguage.Id : 0;
        }
    }
}
