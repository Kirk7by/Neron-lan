﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Repository;
using System.Collections;

namespace ExpertCore
{
    //Вся работа происходит ТУТ
    internal class OperatingMechanism : IMechanism
    {
        static string TESTg;

        static List<Heroes> lHeroes = new List<Heroes>();
        static List<Questions> lQuestions = new List<Questions>();
        static List<Questions> Quest1 = new List<Questions>();
        static List<Questions> Quest2 = new List<Questions>();

        static int indexQuest2;
        private static int position = -1;
        private static List<Questions> StQestions;

        internal OperatingMechanism()
        {

            EntityStorage ent = new Repository().GetEntityStorage();
            if(lHeroes.Count==0)
                lHeroes = ent.Heroes;
            if (lQuestions.Count==0)
                lQuestions = ent.Qestion;
            GetSortListHero();
        }

        public string GetQuestion(int otv)
        {
            string Qestion = "";
            position++;
            if (position == 0)
            {
                Quest2 = GetQuestionDistinctList(lQuestions);
                Quest1.Add(Quest2[0]);
                Quest1[0].OtvetSelected = otv;
                Quest2.RemoveAt(0);
            }
            else
            {
                if (Quest2.Count == 0)
                {
                    GetProbabilityProizvHero(Quest1);
                    string str="";
                    foreach(var s in lHeroes)
                    {
                        str = str + ") " + s.NameHeroes + ": (" + s.ProbabilityHero + ": " + s.ProbabilityProizvHero+": " + s.ProbabilityProizvHero;
                    }
                    return str;
                }

                Quest2[indexQuest2].OtvetSelected = otv;
                Quest1.Add(Quest2[indexQuest2]);
                Quest2.RemoveAt(indexQuest2);
            }

            //угадай героя
            GetProbabilityProizvHero(Quest1);



            //поиск из оставшихся вопросов вопроса с минимальной энтропией, только если есть в хоть один вопрос в списке вопросов
            double? MinEntropy = 0;
            if (Quest2.Count != 0)
            {

                List<Questions> qe3 = new List<Questions>();

                foreach(var qq2 in Quest2)
                {
                    qe3.Add(qq2);
                }
                qe3 = GetСonditionalEntropy(qe3);

                MinEntropy = qe3[0].ProbabilityQustion;
                int i = -1;
                foreach (var q2 in qe3)
                {
                    i++;
                    if (MinEntropy >= q2.ProbabilityQustion)
                    {
                        indexQuest2 = i;
                        Qestion = q2.NameQestion;
                        MinEntropy = q2.ProbabilityQustion;
                    }
                }
            }
            //

            //поиск наиболее вероятного героя
            string MaxprobalityHero = "ничего";
            double? MaxheroProb = 0;
            foreach (var hero in lHeroes)
            {
                if (hero.ProbabilityHero >= MaxheroProb)
                {
                    MaxheroProb = hero.ProbabilityHero;
                    MaxprobalityHero = hero.NameHeroes;
                }
            }
            //

            string str1 = "",str2="";
      /*      foreach(var qs in Quest2)
            {
                foreach (var qe in lQuestions)
                {
                    if (entr.NameQestion == entr.NameQestion)
                    {
                        str2 = str2 + " (" + lHeroes.Find(item => item.NameHeroes == qs.NameHeroes).ProbabilityHero + ") ";
                    }
                }
            }*/
            foreach (var s in lHeroes)
            {
                str1 = str1+ s.NameHeroes + ": (" + s.ProbabilityHero + ") Poizv: (" + s.ProbabilityProizvHero + ") sum: " + TESTg + Convert.ToString(lHeroes[0].ProbabilityProizvHero) + "\n" + "\n";
                
            }
            return str1+ "\n"+ Qestion + Convert.ToString(MaxheroProb) + "(" + MaxprobalityHero + ")" + Convert.ToString(MinEntropy)+"\n"+str2;
        }


        #region ВЫБОР ПЕРСОНАЖА ПО ЗАДАННЫМ ВОПРОСАМ
        //--------------------------input: NULL/Otput: ProbabilityAprioryHero
        private void GetSortListHero()    //Получение отсортированного списка Hero с априорной вероятностью
        {
            lHeroes.Sort((x, y) => Convert.ToInt32(x.WeigthHero).CompareTo(y.WeigthHero));
            foreach (var l in lHeroes)
            {
                l.ProbabilityAprioryHero = (double)l.WeigthHero / lHeroes.Select(p => p.WeigthHero).Sum();
            }
            
            //   lHeroes[0].ProbabilityAprioryHero =(double?)lHeroes[0].WeigthHero / lHeroes.Select(p => p.WeigthHero).Sum();
            //ВАЖНОЕ УТОЧНЕНИЕ: деление осуществлять только с (double)
            //TODO: Не забыть стереть РЕТУРН после тестов
        }

        //--------------------------input:OtvetSelected, List QuestionSelected /Otput: ProbabilityProizvHero
        private void GetProbabilityProizvHero(List<Questions> QuestionSelected)  //Получение списка героев с расчитанным произведением вероятностей за сессию
        {                                               //Где List<Questions> QuestionSelected - Список выбранных вопросов
            int i=0;
            double? tmpProbabilityOtvetSelected = 0;
            double tmp=1;
            foreach (var lH in lHeroes)
            {
                tmpProbabilityOtvetSelected = 0;
                lH.ProbabilityProizvHero = 1;       //ибо умножение
                tmp = 1;
                foreach (var qs in QuestionSelected)
                {
                    foreach (var qall in lQuestions)
                    {
                        if ((qs.NameQestion == qall.NameQestion) && (qall.NameHeroes == lH.NameHeroes))
                        {
                            i++;
                            TESTg = Convert.ToString(lHeroes[0].ProbabilityProizvHero);
                            if (qs.OtvetSelected==1)
                                tmpProbabilityOtvetSelected = qall.OtvetQuest1;
                            if (qs.OtvetSelected == 2)
                                tmpProbabilityOtvetSelected = qall.OtvetQuest2;
                            if (qs.OtvetSelected == 3)
                                tmpProbabilityOtvetSelected = qall.OtvetQuest3;
                            if (qs.OtvetSelected == 4)
                                tmpProbabilityOtvetSelected = 0.2;
                            if (qs.OtvetSelected == 5)
                                tmpProbabilityOtvetSelected = 0.2;
                                //TODO: ТУТ УТЕЧКА!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            
              /*              switch (qs.OtvetSelected)
                            {
                                case 1:
                                    tmpProbabilityOtvetSelected = qall.OtvetQuest1;
                                    break;
                                case 2:
                                    tmpProbabilityOtvetSelected = qall.OtvetQuest2;
                                    break;
                                case 3:
                                    tmpProbabilityOtvetSelected = qall.OtvetQuest3;
                                    break;
                                case 4:
                                    tmpProbabilityOtvetSelected = qall.OtvetQuest4;
                                    break;
                                case 5:
                                    tmpProbabilityOtvetSelected = qall.OtvetQuest5;
                                    break;      //больше 5 невозможно
                            }*/
                            
                            tmp = tmp * (double)tmpProbabilityOtvetSelected;
                        }
                    }
                }
                lH.ProbabilityProizvHero = tmp;
            }
            
            GetListHeroProbability();
            //Логическое продолжение метода ниже
        }
        //--------------------------input: ProbabilityProizvHero/Otput: ProbabilityHero
        public void GetListHeroProbability()    //Получение списка с ответов с расчитанной вероятностью
        {

            double? SumProbabilityProizvHero = 0;

            foreach (var l in lHeroes)
            {
                SumProbabilityProizvHero = SumProbabilityProizvHero + l.ProbabilityProizvHero;
            }
            
            foreach (var l in lHeroes)
            {
                l.ProbabilityHero = (l.ProbabilityAprioryHero * l.ProbabilityProizvHero) / SumProbabilityProizvHero;

                //априоритиВероятность * произведение вероятности всех отвеченных вопросов по данному персонажу l / сумма всех произведений вероятностий всех вопросов
                //ходим по всем отвеченным вопросам и находим вероятность для каждого персонажа
            }
        }
        #endregion





        #region Выбор вопроса

        public List<Questions> GetСonditionalEntropy(List<Questions> QuestionsNext) //Получение условной энтропии для каждого вопроса
        {
            double? tmpprobalityHero;
            // List<Questions> EntropQuestions = GetQuestionDistinctList(QuestionsNext);    //создаём список без повторяющихся вопросов
            List<Questions> qe2 = new List<Questions>();
            qe2.AddRange(QuestionsNext.ToArray());
            double? q1, q2, q3, q4, q5;
            foreach (var entr in qe2)
            {
                q1 = 0;
                q2 = 0;
                q3 = 0;
                q4 = 0;
                q5 = 0;
                foreach (var qe in lQuestions)
                {
                    if (entr.NameQestion == qe.NameQestion)
                    {
                        tmpprobalityHero = lHeroes.Find(item => (string)item.NameHeroes == (string)entr.NameHeroes).ProbabilityHero;

                        q1 += qe.OtvetQuest1 * tmpprobalityHero;
                        q2 += qe.OtvetQuest2 * tmpprobalityHero;
                        q3 += qe.OtvetQuest3 * tmpprobalityHero;
                        q4 += qe.OtvetQuest4 * tmpprobalityHero;
                        q5 += qe.OtvetQuest5 * tmpprobalityHero;
                    }
                }
                //находим нашу условную энтропию
                entr.ProbabilityQustion = Math.Abs(Math.Log(1 / (double)q1) + Math.Log(1 / (double)q2) + Math.Log(1 / (double)q3) +
                    Math.Log(1 / (double)q4) + Math.Log(1 / (double)q5));
            }
            return qe2;
        }


        //--------------------------input: NULL/Otput: NULL
        private List<Questions> GetQuestionDistinctList(List<Questions> lQuestionsNext)   //Получение неповторяющегося списка вопросов
        {
            List<Questions> l = new List<Questions>();
            foreach (var lq in lQuestionsNext)
            {
                if (l.Find(item => item.NameQestion == lq.NameQestion) == null)
                    l.Add(lq);
            }
            return l;
        }

        #endregion
    }
}
