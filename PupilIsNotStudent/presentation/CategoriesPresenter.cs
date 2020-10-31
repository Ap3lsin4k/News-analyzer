﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PupilIsNotStudent.automated_testing;
using PupilIsNotStudent.model.core;
using PupilIsNotStudent.model.interactor;

namespace PupilIsNotStudent.presentation
{
    internal class CategoriesPresenter
    {

       // TextJsonRepository newsJson;
       private readonly CategoriesView view;
       private readonly CategoriesInteractor interactor;


        public CategoriesPresenter(CategoriesView view, CategoriesInteractor interactor) {
            this.view = view;
            this.interactor = interactor;
        }

        public void onBtnTermFrequencyClicked(string catg, string textToBeAnalyzed)
        {
            // calculate TermFrequency to all categories and then press IDF. Then TermFrequencyIDF.
            
            if (string.IsNullOrWhiteSpace(catg))
            {
                view.show("Please enter or chose the category");
                return;
            }


            //feature if the field is not empty then learn
            if (!string.IsNullOrWhiteSpace(textToBeAnalyzed))
            {
                /*
                 * create new category &save only unique words
                 * rewrite if category already exists
                */
                interactor.addCategory(catg, interactor.getSplitWords(textToBeAnalyzed));
                
                interactor.computeTermFrequencyAltogether(catg);
                

                view.setCategories(interactor.getCategories().ToArray());
            }
            else
            {
                //feature if the textBox is empty then show saved data

                if (interactor.whetherCategoryExist(catg))
                {
                    view.show("Write some texts to update the network");
                }
                else
                {
                    view.show("The category \"" + catg + "\" was not created. To create this category enter text of news in the TextBox");
                }

            }
        }



        public void onBtnIdfClicked(string currentCategory)
        {

            if (interactor.getNumberOfShelvesInLibrary() != 0)
            {
                // calculate IDF for each category
                interactor.IDFForEachBook();
                view.show("Success");
            }
            else
                view.show("There is no category to calculate the Inverse Document Frequency");
        }


        public void onBtnTermFrequencyidfClicked(string catg)
        {

            if (interactor.getNumberOfShelvesInLibrary() != 0)
            {
                foreach (string shelf in interactor.getCategories())
                {
                    if (interactor.TermFrequencyExist(shelf) && interactor.idfExist(shelf))
                    {
                        interactor.calculateTermFrequencyIdf(shelf);
                    }
                    else
                    {
                        view.show("Error. TermFrequency exist:"
                                  + interactor.TermFrequencyExist(shelf)
                                  +",\tIDF exist:"
                                  +interactor.idfExist(shelf)
                                  +"; for category: \"" + catg + "\". " 
                                  + "TermFrequency and IDF needs to be computed before proceeding.");
                    }
                }
            }
            else
                view.show("There is no categories to calculate TermFrequency*IDF");
        }


        public void onBtnSaveClicked()
        {
            interactor.saveToJsonFile();
            view.show("Successfully saved");
        }

        public void onBtnLoadTexTermFrequencyromFileClicked()
        {
            /*
             * open explorer to choose a file. It freezes all the process until the OK button will be pressed.
             * "false" means that user pressed "Cancel"
             */
            if (interactor.openFileDialog())
            {
                string text = interactor.readTexTermFrequencyromFile();
                view.loadEditableText(text);
            }
        }

    }
}
