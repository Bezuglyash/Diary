﻿using Diary.DataBase;
using Diary.Model;
using System;
using System.Threading.Tasks;

namespace Diary.ViewModel

        //public ObservableCollection<Note> Notes { get; set; }
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
            <ListBox.ItemTemplate>
                <DataTemplate>
                    <UniformGrid Columns = "3" >
                        < UniformGrid.Resources >
                            < Style TargetType="TextBlock">
                                <Setter Property = "Margin" Value="5"/>
                                <Setter Property = "TextAlignment" Value="Center"/>
                            </Style>
                        </UniformGrid.Resources>
                        <TextBlock Text = "{Binding NoteTitle}" />
                        < TextBlock Text="{Binding NoteContent}"/>
                        <TextBlock Text = "{Binding CreationOrEditingDate}" />
                    </ UniformGrid >
                </ DataTemplate >
            </ ListBox.ItemTemplate >
        </ ListBox > */