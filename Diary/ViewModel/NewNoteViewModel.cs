﻿using Diary.Model;
using System;
using System.Threading.Tasks;

namespace Diary.ViewModel

            get { return newText; }
            set
            {
                {
                    nameElementWhoClose = "Отменить";
                    ItCanSave = false;
                }
                {
                    int countSpacing = 0;
                    int countEnterR = 0;
                    int countEnterN = 0;
                    for (int i = 0; i < newText.Length; i++)
                    {
                        if (newText[i] == ' ')
                        {
                            countSpacing++;
                        }
                        else if (newText[i] == '\r')
                        {
                            countEnterR++;
                        }
                        else if (newText[i] == '\n')
                        {
                            countEnterN++;
                        }
                    }
                    if (countSpacing == newText.Length || countSpacing + countEnterR + countEnterN == newText.Length)
                    {
                        nameElementWhoClose = "Отменить";
                        ItCanSave = false;
                    }
                    else
                    {
                        nameElementWhoClose = "Сохранить";
                        ItCanSave = true;
                    }
                }
        {
            get
            {
                return new RelayCommand(() =>
                {
                    nameElementWhoClose = "Сохранить";
                    Condition = "Collapsed";
                    IsClose = true;
                });
            }
        }

        public ICommand Cancel
        {
            get
            {
                return new RelayCommand(() =>
                {
                    nameElementWhoClose = "Отменить";
                    Condition = "Collapsed";
                    IsClose = true;
                });
            }
        }