﻿using Avalonia.Media;
using MinecraftLaunch.Modules.Models.Launch;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wonderlab.Class.Utils;
using wonderlab.control.Animation;
using wonderlab.Views.Pages;

namespace wonderlab.ViewModels.Pages
{
    public class GameCoreConfigPageViewModel : ReactiveObject {   
        public GameCoreConfigPageViewModel(GameCore core) {
            PropertyChanged += OnPropertyChanged;

            Current = core;
            CurrentPage = new ModConfigPage(core);

            try {
                AsyncRunAction();
                ModLoaders = core.HasModLoader ? string.Join(",", core.ModLoaderInfos.Select(x => x.ModLoaderType)) : "Vanllia";
            }
            catch (Exception) {           

            }
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) {       

        }

        [Reactive]
        public object CurrentPage { get; set; }

        [Reactive]
        public GameCore Current { get; set; }

        [Reactive]
        public string ModLoaders { get; set; }

        [Reactive]
        public string GameCoreTotalSize { get; set; } = "0.00 MB";

        public async void AsyncRunAction() {
            var result = await GameCoreUtils.GetTotalSizeAsync(Current);
            GameCoreTotalSize = result;
        }

        public void GoBackAction() {
            new HomePage().Navigation();
            MainWindow.Instance.OpenTopBar();
        }

        public void OpenFolderAction() {
            Process.Start(new ProcessStartInfo(Current.Root!.FullName) {           
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public void GoModPackAction() {
            CurrentPage = new ModConfigPage(Current);
        }

        public void GoSharePackAction() {
            CurrentPage = new SharePackConfigPage();
        }

        public void GoResourePackAction() {
            CurrentPage = new ResourePackConfigPage(Current);
        }

        public void GoSaveAction() {
            CurrentPage = new SaveConfigPage();
        }
    }
}