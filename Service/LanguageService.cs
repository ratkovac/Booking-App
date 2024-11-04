using BookingApp.DependencyInjection;
using BookingApp.Model;
using BookingApp.Repository.RepositoryInterface;
using CLI.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Service
{
    public class LanguageService
    {
        private ILanguageRepository languageRepository;

        public LanguageService()
        {
            languageRepository = Injector.CreateInstance<ILanguageRepository>();
        }
        public int NextId()
        {
            return languageRepository.NextId();
        }
        public List<Language> GetAll()
        {
            return languageRepository.GetAll();
        }
        public Language GetById(int id)
        {
            return languageRepository.GetById(id);
        }
        public void Create(Language language)
        {
            languageRepository.Create(language);
        }
        public void Delete(Language language)
        {
            languageRepository.Delete(language);
        }
        public void Update(Language language)
        {
            languageRepository.Update(language);
        }
        public void Subscribe(IObserver observer)
        {
            languageRepository.Subscribe(observer);
        }
        public int ExistsLanguage(string name)
        {
            return languageRepository.ExistsLanguage(name);
        }
        public Language GetLanguageByName(string name)
        {
            return languageRepository.GetLanguageByName(name);
        }
    }
}
