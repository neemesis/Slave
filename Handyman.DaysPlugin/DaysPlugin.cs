﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Handyman.Framework.Entities;
using Handyman.Framework.Interfaces;

namespace Handyman.DaysPlugin {
    public class DaysPlugin : IMaster {
        public string Name => "Days Plugin";
        public string Description => "Count from or to date.";
        public string Author => "Mirche Toshevski";
        public string Version => "1.0.0.0";
        public string HelpUrl => "https://github.com/neemesis/Handyman/blob/master/Handyman.DaysPlugin/README.MD";
        public Shortcut HotKey { get; set; }
        public string Alias { get; set; }
        public IParse Parser { get; set; }
        public List<string> Suggestions { get; set; }
        private List<DaysModel> Days { get; set; }

        public DaysPlugin() {
            Alias = "days";
            HotKey = Shortcut.None;
        }

        public void Initialize() {
            Days = Framework.Persistence.Persist.Load<List<DaysModel>>(Alias);
            Suggestions = new List<string> {"days add", "days delete", "days set"};
        }

        private void Add(string name, DateTime date) {
            Days.Add(new DaysModel {Name = name, Date = date});
            Framework.Persistence.Persist.Save(Days, Alias);
        }

        public void TestMethod(string res) {
            Console.WriteLine(res);
        }

        public void Execute(string[] args, Action<string, DisplayData, List<string>, Action<string>> display) {
            if (args[0] == "q") {
                display("This is the question?", DisplayData.Question, new List<string> { "Option 1", "Option 2", "Option 3"}, TestMethod);
                return;
            }
            if (args[0] == "add") {
                Add(args[1], DateTime.Parse(args[2]));
                display("Done", DisplayData.Launcher, null, null);
            } else if (args[0] == "delete") {
                Delete(args[1]);
                display("Done", DisplayData.Launcher, null, null);
            } else if (args[0] == "set") {
                Set(args[1], DateTime.Parse(args[2]));
                display("Done", DisplayData.Launcher, null, null);
            } else {
                display(Calc(args[0]), DisplayData.Launcher, null, null);
            } 
        }

        private void Delete(string name) {
            Days.Remove(Days.Single(x => x.Name == name));
            Framework.Persistence.Persist.Save(Days, Alias);
        }

        private void Set(string name, DateTime date) {
            Days.Remove(Days.Single(x => x.Name == name));
            Add(name, date);
        }

        private string Calc(string name) {
            var dt = Days.Single(x => x.Name == name).Date;
            if (dt < DateTime.Now) {
                var diff = DateTime.Now - dt;
                return (int) Math.Floor(diff.TotalDays) + " days";
            }
            if (dt == DateTime.Now) {
                return "today";
            }
            if (dt > DateTime.Now) {
                var diff = dt - DateTime.Now;
                return (int) Math.Floor(diff.TotalDays) + " days more";
            }
            return "no dates found!";
        }
    }
}
