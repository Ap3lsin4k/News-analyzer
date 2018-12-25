﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Text_analyzer
{
    public partial class Form1 : Form
    {
        // category, and words in each text
//        Dictionary<string, WordFreq> texts = new Dictionary<string, WordFreq>();
        TextJson newsJson = new TextJson();

        public Form1()
        {
            InitializeComponent();
            newsJson.load();
        }

       
        

        private void btnTf_Click(object sender, EventArgs e)
        {

           // newsJson = new TextJson();
            string catg = categoryName.Text; // category
            string toOutToLbWords="", toOutToLbBig="";

            lbWords.Text = "";
            lbBig.Text = "";

            string peeledText = getRawText(tbNews.Text.ToString());
            lbRawNews.Text = peeledText;
            newsJson.texts[catg] = new WordFreq(peeledText.Split());

            foreach (string word in newsJson.texts[catg].allWords)
            {
                toOutToLbWords += word + "\n";
            
            }
            foreach (string word in newsJson.texts[catg].allWords)
            {
                if (newsJson.texts[catg].n.ContainsKey(word))
                {
                    ++newsJson.texts[catg].n[word];
                }
                else
                {
                    newsJson.texts[catg].n[word] = 1;
                }

            }

                
            // TF
            foreach (KeyValuePair<string, int> item in newsJson.texts[catg].n.OrderByDescending(key => key.Value))
            {
                // do something with item.Key and item.Value
                toOutToLbBig += item.Key + " : " + newsJson.texts[catg].n[item.Key] + ",    "
                    + newsJson.texts[catg].getTf(item.Key)
                    + "%" + "\n";
            }
            lbWords.Text = toOutToLbWords;
            lbBig.Text = toOutToLbBig;

            btnIdf.Enabled = true;
        }



        
        private void btnIdf_Click(object sender, EventArgs e)
        {
            lbBig.Text = "";
            string catg = categoryName.Text, textToOut="Idf\n"; // category
            // IDF
            int categories = 0;
            //foreach (WordFreq text in texts.Values)

            foreach (string word in newsJson.texts[catg].allWords)
            {
                categories = 1;
                foreach (KeyValuePair<string, WordFreq> text in newsJson.texts)
                {
                    if (text.Key == catg) continue;
                    foreach (string wordInOtherTexts in text.Value.allWords)
                    {
                        if (word == wordInOtherTexts)
                        {
                            categories++;
                            break;
                        }
                    }
                }
                textToOut +=word + ":" +
                newsJson.texts[catg].getIdf(word, newsJson.texts.Count, categories).ToString("0.####")
                + "\n";
                /* IDF*/
            }

            lbBig.Text = textToOut;

            if(newsJson.texts[catg].flagTf && newsJson.texts[catg].flagIdf)
            {
                btnTfIdf.Enabled = true;
            }

        }

        private void btnTfIdf_Click(object sender, EventArgs e)
        {
            lbBig.Text = "TFIDF Error\n";
            string toOutLbBig = "TFIDF\n";

            string catg = categoryName.Text; // category
            if (newsJson.texts[catg].flagTf && newsJson.texts[catg].flagIdf)
            {
                newsJson.texts[catg].getTfIdf();
                string toOutName = "", toOutValue = "";

                foreach (KeyValuePair<string, double> wordTI in newsJson.texts[catg].TFIDF.OrderByDescending(key => key.Value))
                {
                    toOutName += wordTI.Key + "_*_";
                    toOutValue += (wordTI.Value).ToString("0.#####") + "_*_";
                    toOutLbBig += wordTI.Key + " : " + (wordTI.Value).ToString("0.####") + "\n"; ;
                }
                //textBox1.Text = toOutName;
                //textBox2.Text = toOutValue;
                lbBig.Text = toOutLbBig;
           
            }
            else
            {
                MessageBox.Show("Error. Please press TF, than IDF, and finally TFIDF to get the TFIDF of " + catg + " category");
            }
            
        }

        private bool isCyrillic(int letterCode)
        {
            switch (letterCode)
            {
                case 1028: // Є
                case 1108: // є
                case 1030: // І
                case 1110: // і
                case 1168: // Ґ
                case 1169: // ґ
                case 32:  // " "
                case 39:  // '
                          //case 45:  // -
                    return true;
                default:
                    return
                        1040 <= letterCode && letterCode <= 1103 &&
                        letterCode != 1066 &&  // Ъ
                        letterCode != 1067 &&  // Ы
                        letterCode != 1099 &&  // ы
                        letterCode != 1098  // ъ
                        ;
            }

        }

        private string getRawText(string notClearedText)
        {
            string news = Regex.Replace(notClearedText, @"\s+", " ");
            string peeledText = "";


            foreach (char sumbol in news)
            {
                if (isCyrillic(sumbol))
                    peeledText += Char.ToLower(sumbol);
            }

            peeledText = Regex.Replace(peeledText, @"\s+", " "); // To delete redundant space
                                                                 //            peeledText = peeledText;


            return peeledText;
        }

        //      ========PROGRAM_#2========
        private void btnAnalysis_Click(object sender, EventArgs e)
        {
            string unknownText = getRawText(tbNewText.Text);  // Text with unknown category
            string[] unknownWords = unknownText.Split();
            myGrid.Rows.Clear();


            foreach (KeyValuePair<string, WordFreq> text in newsJson.texts)
            {
                // here we have Category(text.Key) and array of unrepeated words(text.Value.allWords)
                // on the other hand, we have array of repeated words(unknownWords)
                int n = myGrid.Rows.Add(), countOfCommonElem = 0;
                double score =0;
                myGrid.Rows[n].Cells[0].Value = text.Key;  // category
                foreach (string word in unknownWords)
                {
                    int break_counter = 0;
                    foreach (KeyValuePair<string, int> wordCategory in text.Value.n.OrderByDescending(key => key.Value))
                    {
                        if (word == wordCategory.Key) // if word from unknown category equals word from category we know
                        {
                            ++countOfCommonElem;
                            score += wordCategory.Value;
                        }
                        break_counter++;
                        if (break_counter >= 30) break;
                    }
                    
                }

                myGrid.Rows[n].Cells[1].Value = score;  // category
                myGrid.Rows[n].Cells[2].Value = (float)
                    countOfCommonElem / unknownWords.Length * 100;
                    /*/ 
                    (unknownWords.Length + text.Value.allWords.Length-countOfCommonElem) ;
                    */
            }
            //            

        }

        
        private void btnSave_Click(object sender, EventArgs e)
        {
            newsJson.save();
            /*FileInfo f = new FileInfo("Mytext.txt");
            StreamWriter w = f.CreateText();
            w.WriteLine("This is from");
            w.WriteLine("Chapter 6");
            w.WriteLine("Of C# Module");
            w.Write(w.NewLine);
            w.WriteLine("Thanks for your time");
            w.Close();*/
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            newsJson.load();
        }
    }
}


/*
    Кирилиця Десятковий код Шістнадцятеричний код Символ Символьне позначення Ё Ё Ё А А А Б Б Б В В В Г Г Г Д Д Д Е Е Е Ж Ж Ж З З З И И И Й Й Й К К К Л Л Л М М М Н Н Н О О О П П П Р Р Р С С С Т Т Т У У У Ф Ф Ф Х Х Х Ц Ц Ц Ч Ч Ч Ш Ш Ш Щ Щ Щ Ъ Ъ Ъ Ы Ы Ы Ь Ь Ь Э Э Э Ю Ю Ю Я Я Я а а а б б б в в в г г г д д д е е е ж ж ж з з з и и и й й й к к к л л л м м м н н н о о о п п п р р р с с с т т т у у у ф ф ф х х х ц ц ц ч ч ч ш ш ш щ щ щ ъ ы ь э ю я ё 
                  if(1040 <= letterCode && letterCode <= 1103)
                {

                    peeledText += sumbol;
                }   
*/
/*
 * using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text_analyzer
{
    public partial class Form1 : Form
    {

        private string peeledText = "";
        public Form1()
        {
            InitializeComponent();
        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            string news = tbNews.Text.ToString();

            lbRawNews.Text = "";
            peeledText = "";
            foreach (char sumbol in news)
            {
                int letterCode = (int)sumbol;
                lbRawNews.Text = lbRawNews.Text + letterCode + " ";
                peeledText += sumbol;
            }

            MessageBox.Show(peeledText);
        }



        private bool isCyrillic(int letterCode)
        {
                    

        }
    }
}

/*
    Кирилиця Десятковий код Шістнадцятеричний код Символ Символьне позначення Ё Ё Ё А А А Б Б Б В В В Г Г Г Д Д Д Е Е Е Ж Ж Ж З З З И И И Й Й Й К К К Л Л Л М М М Н Н Н О О О П П П Р Р Р С С С Т Т Т У У У Ф Ф Ф Х Х Х Ц Ц Ц Ч Ч Ч Ш Ш Ш Щ Щ Щ Ъ Ъ Ъ Ы Ы Ы Ь Ь Ь Э Э Э Ю Ю Ю Я Я Я а а а б б б в в в г г г д д д е е е ж ж ж з з з и и и й й й к к к л л л м м м н н н о о о п п п р р р с с с т т т у у у ф ф ф х х х ц ц ц ч ч ч ш ш ш щ щ щ ъ ы ь э ю я ё 
     
*/
