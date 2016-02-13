using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using KtoOsel.Lib;
using KtoOsel.Logic;
using KtoOsel.Strategies;

namespace KtoOsel
{
    public class GameLogicVm : INotifyPropertyChanged
    {
        private GameLogic _gameLogic;

        private readonly List<object> _playerControls = new List<object>();

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _newGame = true;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public GameLogicVm()
        {
            PlayGameToEndCommand = new RelayCommand(o => PlayGameToEnd());
            NextMoveCommand = new RelayCommand(o => NextMove());

            CreateGameLogic();
        }

        private void CreateGameLogic()
        {
            var strategies = new List<IStrategy>
            {
                new FirstTestStrategy("First"),
                new FirstTestStrategy("Second"),
                new FirstTestStrategy("Third")
            };

            _newGame = true;

            _playerControls.Clear();

            foreach (var strategy in strategies)
            {
                _playerControls.Add(strategy.Control);
            }

            _gameLogic = new GameLogic(strategies);
        }

        public ICommand NextMoveCommand { get; private set; }

        public ICommand PlayGameToEndCommand { get; private set; }

        private bool NextMove()
        {
            if (_newGame)
            {
                GameLog = String.Empty;
                _newGame = false;
            }

            var turnRes = _gameLogic.Next();
            Debug.WriteLine(turnRes.TurnSummary);
            GameLog += "\n" + turnRes.TurnSummary;
            if (turnRes.EndGame)
            {
                GameLog += "\nGame finished";
                Debug.WriteLine("Game finished");
                CreateGameLogic();
                return false;
            }
            return true;
        }
        

        private void PlayGameToEnd()
        {
            while (NextMove()) { }
        }

        private string _gameLog;
        
        public string GameLog
        {
            get { return _gameLog; }

            set
            {
                if (value == _gameLog)
                    return;

                _gameLog = value;

                OnPropertyChanged("GameLog");
            }
        }

        public IEnumerable<object> PlayerControls { get { return _playerControls; } }
    }
}
