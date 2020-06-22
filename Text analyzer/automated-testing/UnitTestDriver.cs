﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_analyzer.model.interactor;
using Text_analyzer.model.repository;
using Text_analyzer.presentation;

namespace Text_analyzer.automated_testing
{
    class UnitTestDriver
    {
        public GuessPresenter guessPresenter;
        private TextRepository textRepository;
        UnitTestFormGuess unitTestFormGuess;

        public UnitTestDriver(UnitTestFormGuess unitTestFormGuess)
        {
            this.unitTestFormGuess = unitTestFormGuess;
            model.repository.TextJsonRepository textJson = new model.repository.TextJsonRepository();

            guessPresenter = new GuessPresenter(this.unitTestFormGuess,
                new model.interactor.GuessInteractor(
                            new model.repository.TextRepository(),
                            new model.repository.FileRepository(),
                            new model.repository.LogRepository(),
                            in textJson
                        ));
            textRepository = new TextRepository();
        }

        #region
        //helper functions controlling test results
        private String NotEqualsErrorMessage(String Expected, String Actual, ref String Msg)
        {
            if (Msg != "")
            {
                Msg = Msg + " : expected " + Expected + ", but was: " + Actual;
            }
            return Msg;
        }

        private String EqualsErrorMessage(String Expected, String Actual, ref String Msg)
        {
            if (Msg != "")
            {
                Msg = Msg + " : expected " + Expected + " and actual were: " + Actual;
            }
            return Msg;
        }

        private void CheckEquals(String Expected, String Actual, ref String Msg)
        {
            if (Expected == Actual) return;
            NotEqualsErrorMessage(Expected, Actual, ref Msg);
            Exception exception = new Exception(Msg);
            throw exception;
        }
        private void CheckEquals(in List<string> expected, in List<string> actual, ref String msg)
        {
            if (expected.SequenceEqual(actual)) return;
            string expectedString="", actualString="";

            // list to string
            expectedString = expected.Aggregate(expectedString, (current, word) => current + '|' + word + '|');
            actualString = actual.Aggregate(actualString, (current, word) => current + '|'+ word +'|');

            NotEqualsErrorMessage(expectedString, actualString, ref msg);
            var exception = new Exception(msg);
            throw exception;
        }

        private void CheckNotEquals(String Expected, String Actual, ref String Msg)
        {
            if (Expected == Actual)
            {
                EqualsErrorMessage(Expected, Actual, ref Msg);
                Exception exception = new Exception(Msg);
                throw exception;
            }
        }
        #endregion


        public void run()
        {
            //Test1();
            //Test2();
            //Test3();
            //Test4();
            //Test5();
            //Test6();
            Test7();
        }

        //tests
        //Business Rule 1
        public void Test1()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextSport);
            CheckEquals("Спорт", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);
            
        }

        //Business Rule 2
        public void Test2()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextScience);
            CheckEquals("Наука", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);
        }
        //Business Rule 3
        public void Test3()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextEconomy);
            CheckEquals("Економіка", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);

        }
        //Business Rule 4
        public void Test4()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextHealth);
            CheckEquals("Здоров'я", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);

        }
        //Business Rule 5
        public void Test5()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextPolitics);
            CheckEquals("Політика", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);
        }
        //Business Rule 6
        public void Test6()
        {
            String Msg = "Wrong category";
            guessPresenter.onBtnGuessCategoryClicked(RawTextExamples.longTextTourism);
            CheckEquals("Туризм", unitTestFormGuess.getFirstCategoryNameFromMyGrid(), ref Msg);
        }
        //Business Rule 7
        public void Test7()
        {
            List<string> splitText = textRepository.getRawTextSplit(ResourceApostrophe.String1_Demyan);
            var expected = new List<string> {"дем'ян"};
            var msg = "Text is not split correctly";
            CheckEquals(in expected, in splitText, ref msg);
        }


        //Business Rule 8
        public void Test8()
        {
        }
    }
}