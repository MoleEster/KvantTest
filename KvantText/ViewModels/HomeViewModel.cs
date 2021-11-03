using KvantText.Interfaces;
using KvantText.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace KvantText.ViewModels
{
    class HomeViewModel : BaseViewModel
    {
        #region Variables
        private readonly Stack<StrokeCollection> StrokesStack;
        private readonly Stack<StrokeCollection> FutureStrokesStack;
        public bool _drawRectangle;
        private StrokeCollection _Strokes;
        private StrokeCollection _SelectedStrokes;
        private string _CommandHistory = "";
        private IDelegateCommand lastCommand;
        private InkCanvasEditingMode _currentEditingMode;
        private double _panelX;
        private double _panelY;
        #endregion

        #region Fields
        public StrokeCollection MyStrokes
        {
            get
            {
                if (_Strokes == null)
                    _Strokes = new StrokeCollection();
                return _Strokes;
            }
            set
            {
                _Strokes = value;
                OnPropertyChanged("MyStrokes");
            }
        }
        public StrokeCollection SelectedStrokes
        {
            get
            {
                if (_SelectedStrokes == null)
                    _SelectedStrokes = new StrokeCollection();
                return _SelectedStrokes;
            }
            set
            {
                _SelectedStrokes = value;
                OnPropertyChanged("SelectedStrokes");
            }
        }
        public bool SelectrionEnabled
        {
            get
            {
                if (CurrentEditingMode == InkCanvasEditingMode.Select)
                    return true;
                else
                    return false;
            }
        }
        public string CommandHistory
        {
            get
            {
                return _CommandHistory;
            }
            set
            {
                _CommandHistory = '\n' + value ;
                OnPropertyChanged("CommandHistory");
            }
        }
        public InkCanvasEditingMode CurrentEditingMode
        {
            get
            {
                return _currentEditingMode;
            }
            set
            {
                _currentEditingMode = value;
                OnPropertyChanged("CurrentEditingMode");
            }
        }
        public double PanelX
        {
            get { return _panelX; }
            set
            {
                if (value.Equals(_panelX)) return;
                _panelX = value;
                OnPropertyChanged("PanelX");
            }
        }
        public double PanelY
        {
            get { return _panelY; }
            set
            {
                if (value.Equals(_panelY)) return;
                _panelY = value;
                OnPropertyChanged("PanelY");
            }
        }
        #endregion

        public HomeViewModel()
        {
            StrokesStack = new Stack<StrokeCollection>();
            FutureStrokesStack = new Stack<StrokeCollection>();
            _drawRectangle = false;
            CurrentEditingMode = InkCanvasEditingMode.None;
            OnCreateNewCanvasButtonClicked = new DelegateCommand(ExecuteCreateNewCanvasButtonClicked);
            OnSelectButtonClicked = new DelegateCommand(ExecuteSelectButtonClicked);
            OnAddNewRectangleButtonClicked = new DelegateCommand(ExecuteAddNewRectangleButtonClicked);
            OnRedoButtonClicked = new DelegateCommand(ExecuteRedoButtonClicked);
            OnUndoButtonClicked = new DelegateCommand(ExecuteUndoButtonClicked);
            OnReturnButtonClicked = new DelegateCommand(ExecuteReturnButtonClicked);
            OnDeleteButtonClicked = new DelegateCommand(ExecuteDeleteButtonClicked);
            OnMyCanvasPreviewMouseDown = new DelegateMouseCommand(ExecuteMyCanvasPreviewMouseDown);
            OnSelectChange = new DelegateEvent(ExecuteSelectChange);
        }

        #region DelegateCommands
        public IDelegateCommand OnCreateNewCanvasButtonClicked { protected set; get; }
        public IDelegateCommand OnAddNewRectangleButtonClicked { protected set; get; }
        public IDelegateCommand OnRedoButtonClicked { protected set; get; }
        public IDelegateCommand OnUndoButtonClicked { protected set; get; }
        public IDelegateCommand OnSelectButtonClicked { protected set; get; }
        public IDelegateCommand OnReturnButtonClicked { protected get; set; }
        public IDelegateCommand OnMyCanvasPreviewMouseDown { protected set; get; }
        public IDelegateCommand OnSelectChange { protected get; set; }
        public IDelegateCommand OnDeleteButtonClicked { protected get; set; }
        #endregion

        #region ExecuteCommands
        private void ExecuteCreateNewCanvasButtonClicked(object param)
        {
            MyStrokes = new StrokeCollection();
            CommandHistory = "Create new canvas command" + CommandHistory;
            UpdateCommandsList(OnCreateNewCanvasButtonClicked);
        }
        private void ExecuteAddNewRectangleButtonClicked(object param)
        {
            if (CurrentEditingMode.Equals(InkCanvasEditingMode.Select))
                CurrentEditingMode = InkCanvasEditingMode.None;
            if (_drawRectangle)
                _drawRectangle = false;
            else
                _drawRectangle = true;
        }
        private void ExecuteMyCanvasPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_drawRectangle)
            {
                Stroke newRectanle = AddRectangle(PanelY, PanelX);
                DrawingAttributes attributes = new DrawingAttributes
                {
                    Color = Color.FromRgb(0, 0, 0),
                    FitToCurve = false,
                    Width = 1.0,
                    Height = 1.0
                };
                newRectanle.DrawingAttributes = attributes;
                MyStrokes.Add(newRectanle);
                CommandHistory = "Add new rectangle command" + CommandHistory;
                UpdateCommandsList(OnAddNewRectangleButtonClicked);
            }
        }
        private void ExecuteRedoButtonClicked(object param)
        {
            if (lastCommand != null && StrokesStack.Count != 0)
            {
                CommandHistory = "Redo last command" + CommandHistory;
                lastCommand.Execute(param);
            }
        }
        private void ExecuteReturnButtonClicked(object obj)
        {
            if(FutureStrokesStack.Count != 0)
            {
                CommandHistory = "Return command" + CommandHistory;
                StrokesStack.Push(FutureStrokesStack.Peek().Clone());
                MyStrokes = StrokesStack.Peek();
                if (FutureStrokesStack.Count != 0)
                    FutureStrokesStack.Pop();
            }
        }
        private void ExecuteUndoButtonClicked(object param)
        {
            if (StrokesStack.Count != 0)
            {
                FutureStrokesStack.Push(StrokesStack.Peek().Clone());
                StrokesStack.Pop();
            }
            
            if (StrokesStack.Count != 0)
            {
                CommandHistory = "Undo last command" + CommandHistory;
                MyStrokes = new StrokeCollection();
                MyStrokes = StrokesStack.Peek();
                lastCommand = OnUndoButtonClicked;
            }
            else
            {
                MyStrokes = new StrokeCollection();
            }
        }
        private void ExecuteSelectButtonClicked(object param)
        {
            if (CurrentEditingMode != InkCanvasEditingMode.Select)
            {
                CurrentEditingMode = InkCanvasEditingMode.Select;
                _drawRectangle = false;
            }
            else
                CurrentEditingMode = InkCanvasEditingMode.None;
        }
        private void ExecuteSelectChange(object sender)
        {
            StrokesStack.Push(MyStrokes.Clone());
        }

        private void ExecuteDeleteButtonClicked(object obj)
        {
            MyStrokes.Remove(SelectedStrokes);
        }
        #endregion

        #region OtherFuncs
        public Stroke AddRectangle(double top,double left)
        {
            StylusPointCollection strokePoints = new StylusPointCollection
            {
                new StylusPoint(left, top),
                new StylusPoint(left + 50, top),
                new StylusPoint(left + 50, top + 50),
                new StylusPoint(left, top + 50),
                new StylusPoint(left, top)
            };

            Stroke newStroke = new Stroke(strokePoints);
            return newStroke;

        }
        private void UpdateCommandsList(IDelegateCommand command)
        {
            lastCommand = command;
            StrokesStack.Push(MyStrokes.Clone());
        }
        #endregion
    }
}
