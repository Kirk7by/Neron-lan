﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepository
    {
        EntityStorage GetEntityStorage(); //метод реализующий вывод массива двух сущностей
        Exception AddHeroesAndQuestion(string nameHero, string nameQuestion, List<Questions> QustSelected);   //Добавление героя и вопроса
        void AddQuestion(Questions que);


  //      IEnumerable<HeroesAndQuestions> GetStudentsClasses(EntityStorage storage); //что-то, что нужно удалить

    }
}
